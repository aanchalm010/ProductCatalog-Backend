using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Models;
using System.Collections.Generic;

namespace ProductCatalog.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<Product> Products { get; set; } = null!;
    }
}
