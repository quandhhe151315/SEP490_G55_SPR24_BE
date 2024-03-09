using Data.Entity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly KitchenDelightsContext _context;

        public CommentRepository(KitchenDelightsContext context) 
        {
            _context = context;
        }

        public Task<BlogComment?> GetComment(int id)
        {
            return _context.BlogComments.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.InverseParent)
                .ThenInclude(x => x.InverseParent)
                .Where(x => x.CommentStatus != 0)
                .FirstOrDefaultAsync(x => x.CommentId == id);
        }

        public Task<List<BlogComment>> GetComments(int id)
        {
            return _context.BlogComments.AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.InverseParent)
                .ThenInclude(x => x.InverseParent)
                .Where(x => x.BlogId == id && x.ParentId == null)
                .ToListAsync();
        }

        public void CreateComment(BlogComment comment) 
        {
            try
            {
                _context.BlogComments.Add(comment);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateComment(BlogComment comment)
        {
            try
            {
                _context.BlogComments.Update(comment);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteComment(BlogComment comment)
        {
            try
            {
                _context.BlogComments.Remove(comment);
            } catch (Exception ex) 
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}