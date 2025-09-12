using BlogApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Business.Services
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllTagsAsync();
        Task<Tag> GetTagByIdAsync(int id);
        Task<Tag> CreateTagAsync(Tag tag);
        Task UpdateTagAsync(Tag tag);
        Task DeleteTagAsync(int id);
        Task<bool> TagExistsAsync(int id);
        Task<Tag> GetTagByNameAsync(string name);
        Task<IEnumerable<Post>> GetPostsByTagAsync(string tagName);
    }
}