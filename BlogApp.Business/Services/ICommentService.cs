using BlogApp.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApp.Business.Services
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllCommentsAsync();
        Task<Comment> GetCommentByIdAsync(int id);
        Task<Comment> CreateCommentAsync(Comment comment);
        Task UpdateCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id);
        Task<bool> CommentExistsAsync(int id);
        Task<IEnumerable<Comment>> GetCommentsByPostAsync(int postId);
        Task<IEnumerable<Comment>> GetCommentsByUserAsync(int userId);
    }
}