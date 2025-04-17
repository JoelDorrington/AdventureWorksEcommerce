using AdventureWorks.Server.DAL;
using AdventureWorks.Server.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdventureWorks.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private ApplicationDbContext _context;
        public ShoppingCartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET api/<ShoppingCartController>/5
        [HttpGet("{cartId}")]
        public async Task<IActionResult> Get(string cartId)
        {
            var items = await _context.ShoppingCartItems
                .Where(x => x.ShoppingCartId == cartId
                    && x.Quantity > 0)
                .Include(x => x.Product)
                .Select(x => new ShoppingCartItem()
                {
                    ShoppingCartId = x.ShoppingCartId,
                    ProductID = x.ProductID,
                    Quantity = x.Quantity,
                    DateCreated = x.DateCreated,
                    ModifiedDate = x.ModifiedDate,
                    Product = new Product(x.ProductID)
                    {
                        Name = x.Product.Name,
                        ProductNumber = x.Product.ProductNumber,
                        Color = x.Product.Color,
                        ListPrice = x.Product.ListPrice,
                    }
                })
                .ToListAsync();

            return Ok(items);
        }

        // POST api/<ShoppingCartController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ShoppingCartController>/5
        [HttpPut("{cartId}/{productId}/{qty}")]
        [EnableRateLimiting("ShoppingCartLimiter")]
        public async Task<IActionResult> Put(string cartId, int productId, int qty)
        {
            if (qty < 1) return BadRequest("Quantity must be at least 1");
            var product = await _context.Products
                .Where(p => p.ProductID == productId)
                .Select(p => new Product(p.ProductID))
                .FirstOrDefaultAsync();
            if(product == null)
            {
                return BadRequest("Invalid Product ID");
            }

            var cartItem = await _context.ShoppingCartItems
                .Where(x => x.ShoppingCartId == cartId && x.ProductID == productId)
                .FirstOrDefaultAsync();

            if(cartItem != null)
            {
                cartItem.Quantity = qty;
                cartItem.ModifiedDate = DateTime.UtcNow;
                _context.ShoppingCartItems.Update(cartItem);
            } else
            {
                cartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = cartId.ToString(),
                    ProductID = productId,
                    Quantity = qty,
                    DateCreated = DateTime.UtcNow,
                    ModifiedDate = DateTime.UtcNow
                };
                _context.ShoppingCartItems.Add(cartItem);
            }
            _context.SaveChanges();
            return Ok(cartItem);
        }

        // DELETE api/<ShoppingCartController>/5
        [HttpDelete("{cartId}/{productId}")]
        public async Task<IActionResult> Delete(string cartId, int productId)
        {
            var result = await _context.ShoppingCartItems
                .Where(x => x.ShoppingCartId == cartId && x.ProductID == productId)
                .ExecuteDeleteAsync();

            return Ok();
        }
    }
}
