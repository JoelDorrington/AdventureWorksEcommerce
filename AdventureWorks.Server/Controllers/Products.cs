using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Server.DAL;
using AdventureWorks.Server.DAL.QueryParameters;
using AdventureWorks.Server.Entities;
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
        private Repository<Product> _repository;
        
        public Products(IConfiguration config)
        {
            var sqlClientFactory = new SqlClientFactory(config);
            _repository = new Repository<Product>(sqlClientFactory);
        }

        // GET: api/<Products>
        [HttpGet]
        public async Task<IReadOnlyList<Product>> Get(
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
                    new WhereCondition("Name", ComparerOperators.Like, search)
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
            args.OrderBy = "ListPrice";
            args.Ascending = !reverse;
            args.Page = page;
            args.PageSize = length;
            if (filter.Conditions.Any())
            {
                args.Filter = filter;
            }
            var products = await _repository.GetAllAsync(args);
            return products;
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
