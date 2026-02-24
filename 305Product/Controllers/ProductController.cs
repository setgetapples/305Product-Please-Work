using _305Product.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using _305Product.Models;

namespace _305Product.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProductController(AppDbContext db) // before calling method, ASP.NET needs to build ProductController
        {
            _db = db;
        }

        [HttpGet("[action]")] // as shown below, "[action]" turns into GetProducts()
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _db.Products.ToListAsync();
                return Ok(products);
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception: " + e.Message);
                return BadRequest();
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> addProduct([FromBody] Product value)
        {
            if (value == null)
            {
                return BadRequest("Product cannot be null");
            }
            try
            {
                var model = new Product
                {
                    ProductName = value.ProductName
                };
                _db.Products.Add(model);
                await _db.SaveChangesAsync();

                return Ok(model);
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception: " + e.Message);
                return BadRequest();
            }
        }

        [HttpPut("[action]/{id}")]
        public async Task<IActionResult> updateProduct([FromBody] Product value, int id)
        {
            if(value == null)
            {
                return BadRequest("product data can't be null");
            }
            try
            {
                var product = await _db.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with id: {id} was not found");
                }

                product.ProductName = value.ProductName;

                await _db.SaveChangesAsync();
                return Ok(product);

            }
            catch (Exception e)
            {
                Console.WriteLine("exception occured: ", e.Message);
                return BadRequest("check message");
            }
        }
    }
}