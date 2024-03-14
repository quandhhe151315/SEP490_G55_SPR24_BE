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
    public class NewsRepository : INewsRepository
    {
        private readonly KitchenDelightsContext _context;

        public NewsRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public void CreateNews(News news)
        {
            try
            {
                _context.News.Add(news);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateNews(News news)
        {
            try
            {
                _context.News.Update(news);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteNews(News news)
        {
            try
            {
                _context.News.Remove(news);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task<News?> GetNews(int id)
        {
            return await _context.News.AsNoTracking()
                .Include(news => news.User)
                .Where(x => x.NewsStatus != 0)
                .FirstOrDefaultAsync(x => x.NewsId == id);
        }

        public async Task<List<News>> GetNews()
        {
            return await _context.News.AsNoTracking()
                .Include(news => news.User)
                .Where(x => x.NewsStatus != 0)
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();
        }

        public async Task<List<News>> SearchNews(string searchString)
        {
            return await _context.News.AsNoTracking().Include(news => news.User)
                .Where(x => x.NewsStatus != 0 && x.NewsContent.ToLower().Contains(searchString))
                .OrderByDescending(x => x.CreateDate)
                .ToListAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
