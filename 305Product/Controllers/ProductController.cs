using _305Product.Data;
using _305Product.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> GetProducts() // GET
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
        public async Task<IActionResult> addProduct([FromBody] Product value) // POST
        {
            if (value == null)
            {
                return BadRequest("Product cannot be null");
            }
            try
            {
                var model = new Product
                {
                    ProductName = value.ProductName,
                    Price = value.Price
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
        public async Task<IActionResult> updateProduct(int id, [FromBody] Product value) // PUT
        {
            if(value == null)
            {
                return BadRequest("product data can't be null.");
            }
            try
            {
                var product = await _db.Products.FindAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with id: {id} was not found");
                }

                product.ProductName = value.ProductName;
                product.Price = value.Price;

                await _db.SaveChangesAsync();

                Console.WriteLine($"Incoming value.Price: {value.Price}");
                Console.WriteLine($"Existing product.Price: {product.Price}");

                return Ok(product);

            }
            catch (Exception e)
            {
                Console.WriteLine("exception occured: ", e.Message);
                return BadRequest("check message");
            }
        }

        [HttpGet("[action]/{name}")]
        public async Task<IActionResult> GetProducts(string  name) // GET
        {
            // run query within try clause
            // use ToListAsync() to gather all items that contain the name

            
            if (name == null)
            {
                return BadRequest("Product name cannot be null.");
            }
            try
            {
                var results = await _db.Products.Where(p => p.ProductName.Contains(name)).ToListAsync();
                return Ok(results);
            }
            catch (Exception e)
            {
                Console.WriteLine("Got exception: " + e.Message);
                return BadRequest();
            }
        }
    }
}