using BlogApp.Data;
using BlogApp.Data.Entities;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlogApp.Data.Repositories
{
    public class TagRepository : BlogRepository<Tag>, ITagRepository
    {
        public TagRepository(BlogDbContext context) : base(context)
        {
        }

        public async Task<Tag> GetTagByNameAsync(string name)
        {
            return await _context.Tags
                .FirstOrDefaultAsync(t => t.Name == name);
        }

        public async Task<bool> TagExistsAsync(string name)
        {
            return await _context.Tags
                .AnyAsync(t => t.Name == name);
        }
    }
}