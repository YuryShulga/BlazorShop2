
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace BlazorShop.Models
{
    public class EfCatalog : ICatalog
    {
        private readonly AppDbContext _context;

        public EfCatalog(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task AddProduct(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<Product>?> GetProducts()
        {
            var list = await _context.Products.ToListAsync();
            return list.AsReadOnly();
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}
