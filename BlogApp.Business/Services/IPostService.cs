using BlogApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Business.Services
{
    public interface IPostService
    {
        Task<IEnumerable<Post>> GetAllPostsAsync();
        Task<Post> GetPostByIdAsync(int id);
        Task<Post> CreatePostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task DeletePostAsync(int id);
        Task<bool> PostExistsAsync(int id);
        Task<IEnumerable<Post>> GetPostsByAuthorAsync(int authorId);
        Task<IEnumerable<Post>> GetPostsWithTagsAsync();
        Task<Post> GetPostWithCommentsAsync(int id);
        Task<IEnumerable<Post>> SearchPostsAsync(string searchTerm);
        Task<IEnumerable<Post>> GetPostsByTagAsync(string tagName);
    }
}