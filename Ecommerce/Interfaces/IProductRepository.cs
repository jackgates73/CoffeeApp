using Ecommerce.Models;

namespace Ecommerce.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> GetFilterList(string id);
        Task<Product> GetByIdAsync(string id);
        bool Add(Product product);
        bool Update(Product product);
        bool Delete(Product product);
        bool Save();
    }
}
