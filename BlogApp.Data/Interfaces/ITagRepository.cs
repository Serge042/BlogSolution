using BlogApp.Data.Entities;
using System.Threading.Tasks;

namespace BlogApp.Data.Interfaces
{
    public interface ITagRepository : IBlogRepository<Tag>
    {
        Task<Tag> GetTagByNameAsync(string name);
        Task<bool> TagExistsAsync(string name);
    }
}