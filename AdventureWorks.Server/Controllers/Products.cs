using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Server.DAL;
using AdventureWorks.Server.DAL.QueryParameters;
using AdventureWorks.Server.Entities;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdventureWorks.Server.Controllers
{
    public class ProductResponse
    {
        public Product? data { get; set; }
    }


    [Route("api/[controller]")]
    [ApiController]
    public class Products : ControllerBase
    {
        private ISqlClientFactory _sqlClientFactory;
        private Repository<Product> _repository;
        
        public Products(IConfiguration config)
        {
            _sqlClientFactory = new DAL.SqlClientFactory(config);
            _repository = new Repository<Product>(_sqlClientFactory);
        }

        private string ViewPropertyToColumnName(string prop)
        {
            return prop switch
            {
                "productID" => "ProductID",
                "name" => "Name",
                "price" => "ListPrice",
                _ => throw new ArgumentException($"Invalid property name: {prop}")
            };
        }

        // GET: api/<Products>
        [HttpGet]
        [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(
            [FromQuery] string? search = null,
            [FromQuery] string? category = null,
            [FromQuery] int page = 1,
            [FromQuery] int length = 20,
            [FromQuery] string sort = "ListPrice",
            [FromQuery] bool reverse = false
        )
        {
            WhereExpression filter = new();
            GetParameters args = new();
            if (!string.IsNullOrEmpty(search)) { 
                filter.AddCondition(
                    new WhereCondition("Name", ComparerOperators.Like, $"%{search}%")
                );
            }
            if (!string.IsNullOrEmpty(category))
            {
                filter.AddCondition(
                    new WhereCondition("ProductSubcategoryID", ComparerOperators.Equal, category)
                );
            }
            args.Select = [
                new SelectParameter("ProductID"),    
                new SelectParameter("Name"),    
                new SelectParameter("ListPrice"),
            ];
            if (page < 1) return BadRequest("First page is 1");
            try
            {
                args.OrderBy = ViewPropertyToColumnName(sort);
                args.Ascending = !reverse;
                args.Page = page-1;
                args.PageSize = length;
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            if (filter.Conditions.Any())
            {
                args.Filter = filter;
            }
            var products = await _repository.GetAllAsync(args);
            return Ok(products);
        }

        [HttpGet("{id}/Photo")]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var command = new SqlCommand("SELECT p.LargePhotoFileName, p.LargePhoto FROM Production.ProductProductPhoto AS t"
                + " LEFT JOIN Production.ProductPhoto AS p ON t.ProductPhotoID = p.ProductPhotoID"
                + " WHERE t.ProductID = @productID");
            command.Parameters.AddWithValue("productID", id);
            var stream = new SqlReaderStream(_sqlClientFactory, command);
            try
            {
                Response.Headers.Append("Content-Disposition", $"inline; filename={stream.FileName}");
                await stream.ExecuteReaderAsync();
                return new FileStreamResult(stream, stream.Mimetype);
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
                stream.Dispose();
                return NotFound();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                stream.Dispose();
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{id}/Thumbnail")]
        [ProducesResponseType(typeof(byte[]), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetThumbnail(int id)
        {
            var command = new SqlCommand("SELECT p.ThumbNailPhotoFileName, p.ThumbNailPhoto FROM Production.ProductProductPhoto AS t"
                + " LEFT JOIN Production.ProductPhoto AS p ON t.ProductPhotoID = p.ProductPhotoID"
                + " WHERE t.ProductID = @productID");
            command.Parameters.AddWithValue("productID", id);
            var stream = new SqlReaderStream(_sqlClientFactory, command);
            try
            {
                await stream.ExecuteReaderAsync();
                return new FileStreamResult(stream, "image/*");
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine(ex.Message);
                stream.Dispose();
                return NotFound();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                stream.Dispose();
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        // GET api/<Products>/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            ProductResponse response = new();
            response.data = await _repository.GetByIdAsync(id);
            if(response.data == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        // POST api/<Products>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<Products>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<Products>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
