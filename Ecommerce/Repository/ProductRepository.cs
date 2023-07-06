using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;

namespace Ecommerce.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Product product)
        {
            _context.Add(product);
            return Save();
        }

        public bool Delete(Product product)
        {
            _context.Remove(product);
            return Save();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _context.Product.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            return await _context.Product.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Product>> GetFilterList(string id)
        {
            return await _context.Product.Where(c => c.Title.Contains(id)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
