using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;

namespace Ecommerce.Repository
{
    public class AppUserRepository : IAppUserRepository
    {
        private readonly ApplicationDbContext _context;

        public AppUserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(AppUser appUser)
        {
            _context.Add(appUser);
            return Save();
        }

        public bool Delete(AppUser appUser)
        {
            _context.Remove(appUser);
            return Save();
        }

        public async Task<IEnumerable<AppUser>> GetAll()
        {
            return await _context.AppUsers.ToListAsync();
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            return await _context.AppUsers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<AppUser>> GetFilterList(string id)
        {
            //return await _context.AppUsers.Where(c => c.Title.Contains(id)).ToListAsync();
            throw new NotImplementedException();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser appUser)
        {
            throw new NotImplementedException();
        }
    }
}
