using BlogApp.Data.Entities;
using System.Threading.Tasks;

namespace BlogApp.Data.Interfaces
{
    public interface IUserRepository : IBlogRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByUsernameAsync(string username);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UsernameExistsAsync(string username);
        Task<User> GetUserWithRolesAsync(int userId);
        Task<bool> UserExistsAsync(int id);
    }
}