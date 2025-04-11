using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Server.DAL;
using AdventureWorks.Server.DAL.QueryParameters;
using AdventureWorks.Server.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdventureWorks.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductCategories : ControllerBase
    {
        private Repository<ProductSubcategory> _repository;
        public ProductCategories(IConfiguration config)
        {
            _repository = new Repository<ProductSubcategory>(new SqlClientFactory(config));
        }

        // GET: api/<ValuesController>
        [HttpGet]
        [ProducesResponseType(typeof(IReadOnlyList<ProductSubcategory>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var args = new GetParameters();
            args.Select = [
                new SelectParameter(){ Column = "ProductSubcategoryID" },
                new SelectParameter(){ Column = "Name" },
            ];

            // Better Idea - JoinParameter<ProductCategory>("ProductCategoryId") -- Can I assume that PK column name will be same as FK? Perhaps an easy override.
            // Ideal would be a [ForeignKey(typeof(ProductCategory))] attribute, enabling JoinParameter<ProductCategory>(SelectParameter[][, SupportedJoinOperators joinType=Inner])
            var joinProductCategory = new JoinParameter<ProductCategory>();
            joinProductCategory.SelectParameters = [
                new SelectParameter(){ Column = "Name", Alias = "CategoryName" },
            ];
            args.JoinParams = [
                joinProductCategory
            ];
            args.OrderBy = "CategoryName";
            return Ok(await _repository.GetAllAsync(args));
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ValuesController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
