using BlogApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Data.Interfaces
{
    public interface IPostTagRepository : IBlogRepository<PostTag>
    {
        Task<IEnumerable<PostTag>> GetPostTagsByPostIdAsync(int postId);
        Task<IEnumerable<PostTag>> GetPostTagsByTagIdAsync(int tagId);
        Task RemovePostTagsByPostIdAsync(int postId);
        Task RemovePostTagsByTagIdAsync(int tagId);
    }
}