using Ecommerce.Models;

namespace Ecommerce.Interfaces
{
    public interface IAppUserRepository
    {
        Task<IEnumerable<AppUser>> GetAll();
        Task<IEnumerable<AppUser>> GetFilterList(string id);
        Task<AppUser> GetByIdAsync(string id);
        bool Add(AppUser appUser);
        bool Update(AppUser appUser);
        bool Delete(AppUser appUser);
        bool Save();
    }
}
