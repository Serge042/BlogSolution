using BlogApp.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Data.Repositories
{
    public class PostTagRepository : BlogRepository<PostTag>, IPostTagRepository
    {
        public PostTagRepository(BlogDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<PostTag>> GetPostTagsByPostIdAsync(int postId)
        {
            return await _context.PostTags
                .Where(pt => pt.PostId == postId)
                .Include(pt => pt.Tag)
                .ToListAsync();
        }

        public async Task<IEnumerable<PostTag>> GetPostTagsByTagIdAsync(int tagId)
        {
            return await _context.PostTags
                .Where(pt => pt.TagId == tagId)
                .Include(pt => pt.Post)
                .ToListAsync();
        }

        public async Task RemovePostTagsByPostIdAsync(int postId)
        {
            var postTags = await _context.PostTags
                .Where(pt => pt.PostId == postId)
                .ToListAsync();

            _context.PostTags.RemoveRange(postTags);
            await _context.SaveChangesAsync();
        }

        public async Task RemovePostTagsByTagIdAsync(int tagId)
        {
            var postTags = await _context.PostTags
                .Where(pt => pt.TagId == tagId)
                .ToListAsync();

            _context.PostTags.RemoveRange(postTags);
            await _context.SaveChangesAsync();
        }
    }
}