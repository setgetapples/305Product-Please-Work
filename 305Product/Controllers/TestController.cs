using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using _305Product.Models;
using _305Product.Data;

using Microsoft.EntityFrameworkCore;

namespace _305Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {

        private readonly AppDbContext _db;

        public TestController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> getAllProducts()
        {
            try
            {
                var products = _db.Products.ToListAsync();
                return Ok(products);
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception in getAllProducts: ", e);
                return BadRequest();
            }
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> getProductById(int id)
        {
            if  (id == null)
            {
                Console.WriteLine("Got exception: cannot get product with null value.");
                return BadRequest();
            }
            try
            {
                var product = _db.Products.FindAsync(id);
                return Ok(product);
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception in getProductById: ", e, "\n with ID: ", id);
                return BadRequest();
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> createNewProduct([FromBody] Product value)
        {
            try
            {
                var model = new Product
                {
                    ProductName = value.ProductName
                    Price = value.Price
                };
                _db.Products.Add(model);
                await _db.SaveChangesAsync();

                return Ok(model);

            }
            catch(Exception e)
            {
                Console.WriteLine("Encountered exception for creating new product: ", e);
            }
        }
    }
}
