using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using System.Threading.Tasks;

namespace BlogApp.Data.Interfaces
{
    public interface IRoleRepository : IBlogRepository<Role>
    {
        Task<Role> GetRoleByNameAsync(string name);
        Task<bool> RoleExistsAsync(int id);
    }
}