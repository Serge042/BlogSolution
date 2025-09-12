using BlogApp.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using BlogApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogApp.Data.Repositories
{
    public class RoleRepository : BlogRepository<Role>, IRoleRepository
    {
        public RoleRepository(BlogDbContext context) : base(context)
        {
        }

        public async Task<Role> GetRoleByNameAsync(string name)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<bool> RoleExistsAsync(int id)
        {
            return await _context.Roles
                .AnyAsync(r => r.Id == id);
        }
    }
}