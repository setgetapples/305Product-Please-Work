using Microsoft.EntityFrameworkCore;
using _305Product.Models;

namespace _305Product.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {
            
        }
        public DbSet<Product> Products { get; set; }

        // identical to the table name
    }
}
