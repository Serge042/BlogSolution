using BlogApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Data.Interfaces
{
    public interface ICommentRepository : IBlogRepository<Comment>
    {
        Task<IEnumerable<Comment>> GetCommentsByPostAsync(int postId);
        Task<IEnumerable<Comment>> GetCommentsByUserAsync(int userId);
        Task<Comment> GetCommentWithUserAsync(int id);
    }
}