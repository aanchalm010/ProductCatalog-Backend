using Microsoft.EntityFrameworkCore;
using ProductCatalog.Api.Data;
using ProductCatalog.Api.Models;

namespace ProductCatalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _db;
        public ProductRepository(AppDbContext db) { _db = db; }

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _db.Products.AsNoTracking().ToListAsync();

        public async Task<Product?> GetByIdAsync(int id)
            => await _db.Products.FindAsync(id);

        public async Task AddAsync(Product product)
            => await _db.Products.AddAsync(product);

        public void Update(Product product)
            => _db.Products.Update(product);

        public void Remove(Product product)
            => _db.Products.Remove(product);

        public async Task<bool> SaveChangesAsync()
            => (await _db.SaveChangesAsync()) > 0;
    }
}
