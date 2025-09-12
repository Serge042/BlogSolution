using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using System.Threading.Tasks;

namespace BlogApp.Data.Interfaces
{
    public interface IUserRoleRepository : IBlogRepository<UserRole>
    {
        Task<UserRole> GetUserRoleAsync(int userId, int roleId);
        Task<bool> UserRoleExistsAsync(int userId, int roleId);
        Task RemoveUserRolesByUserIdAsync(int userId);
    }
}