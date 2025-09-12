using BlogApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Data.Interfaces
{
    public interface IPostRepository : IBlogRepository<BlogApp.Data.Entities.Post>
    {
        Task<IEnumerable<BlogApp.Data.Entities.Post>> GetPostsWithUsersAsync();
        Task<BlogApp.Data.Entities.Post> GetPostWithUserAndCommentsAsync(int id);
        Task<IEnumerable<BlogApp.Data.Entities.Post>> GetPostsByTagAsync(string tagName);
        Task<IEnumerable<BlogApp.Data.Entities.Post>> GetPostsByAuthorAsync(int authorId);
        Task<IEnumerable<BlogApp.Data.Entities.Post>> GetPostsWithTagsAsync();
        Task<BlogApp.Data.Entities.Post> GetPostWithCommentsAsync(int id);
        Task<IEnumerable<BlogApp.Data.Entities.Post>> SearchPostsAsync(string searchTerm);
    }
}