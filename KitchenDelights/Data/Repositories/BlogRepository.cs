using Data.Entity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private KitchenDelightsContext _context;

        public BlogRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public void CreateBlog(Blog blog)
        {
            try
            {
                _context.Blogs.Add(blog);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateBlog(Blog blog)
        {
            try
            {
                _context.Blogs.Update(blog);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteBlog(Blog blog)
        {
            try
            {
                _context.Blogs.Remove(blog);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task<Blog?> GetBlog(int id)
        {
            return await _context.Blogs.AsNoTracking()
                .Include(x => x.User).Include(x => x.Category)
                .Where(x => x.BlogStatus != 0)
                .FirstOrDefaultAsync(x => x.BlogId == id);
        }

        public async Task<List<Blog>> GetBlogs()
        {
            return await _context.Blogs.AsNoTracking()
                .Include(x => x.User).Include(x => x.Category)
                .Where(x => x.BlogStatus != 0)
                .ToListAsync();
        }

        public async Task<List<Blog>> GetBlogs(int categoryId)
        {
            return await _context.Blogs.AsNoTracking()
                .Include(x => x.User).Include(x => x.Category)
                .Where(x => x.CategoryId == categoryId)
                .Where(x => x.BlogStatus != 0)
                .ToListAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
