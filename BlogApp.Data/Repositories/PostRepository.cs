using BlogApp.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Data.Repositories
{
    public class PostRepository : BlogRepository<Post>, IPostRepository
    {
        public PostRepository(BlogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Post>> GetPostsWithUsersAsync()
        {
            return await _context.Posts
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<Post> GetPostWithUserAndCommentsAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Post>> GetPostsByTagAsync(string tagName)
        {
            return await _context.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .Where(p => p.PostTags.Any(pt => pt.Tag.Name == tagName))
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsByAuthorAsync(int authorId)
        {
            return await _context.Posts
                .Where(p => p.UserId == authorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Post>> GetPostsWithTagsAsync()
        {
            return await _context.Posts
                .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
                .ToListAsync();
        }

        public async Task<Post> GetPostWithCommentsAsync(int id)
        {
            return await _context.Posts
                .Include(p => p.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Post>> SearchPostsAsync(string searchTerm)
        {
            return await _context.Posts
                .Where(p => p.Title.Contains(searchTerm) || p.Body.Contains(searchTerm))
                .ToListAsync();
        }
    }
}