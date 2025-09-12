using BlogApp.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using BlogApp.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Data.Repositories
{
    public class UserRoleRepository : BlogRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(BlogDbContext context) : base(context)
        {
        }

        public async Task<UserRole> GetUserRoleAsync(int userId, int roleId)
        {
            return await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }

        public async Task<bool> UserRoleExistsAsync(int userId, int roleId)
        {
            return await _context.UserRoles
                .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);
        }

        public async Task RemoveUserRolesByUserIdAsync(int userId)
        {
            var userRoles = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .ToListAsync();

            _context.UserRoles.RemoveRange(userRoles);
            await _context.SaveChangesAsync();
        }
    }
}