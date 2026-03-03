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

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> deleteProduct(int id)
        {
            if (id == null)
            {
                Console.WriteLine($"Error: could not find product with id: {id} \n Check your database size.");
            }
            try
            {
                var product = await _db.Products.FindAsync(id);
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();

                return Ok($"Product with id: {id} was removed from the database.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Encountered exception for deleting product: \n {e}");
                return BadRequest();
            }
        }

        //"real world" feature/example: find products cheaper than a given price
        [HttpGet("[action]/{price}")]
        public async Task<IActionResult> cheaperThanPrice(int price)
        {
            try
            {
                var products = await _db.Products.Where(p => p.Price < price).ToListAsync();
                return Ok(products);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Encountered exception for searching for products cheaper than given price: \n {e}");
                return BadRequest();
            }
        }
    }
}