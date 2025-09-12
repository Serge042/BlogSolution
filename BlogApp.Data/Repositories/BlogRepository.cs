using BlogApp.Data;
using BlogApp.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BlogApp.Data.Repositories
{
    public class BlogRepository<T> : IBlogRepository<T> where T : class
    {
        protected readonly BlogDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<BlogApp.Data.Entities.Post> GetByIdAsync(int id)
        {
            return await _context.Posts.FirstOrDefaultAsync(r => r.Id == id);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public virtual void Remove(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        Task<T> IBlogRepository<T>.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}