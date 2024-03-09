using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBlogRepository
    {
        void CreateBlog(Blog blog);
        void UpdateBlog(Blog blog);
        void DeleteBlog(Blog blog);
        Task<Blog?> GetBlog(int id);
        Task<List<Blog>> GetBlogs();
        Task<List<Blog>> GetBlogs(int categoryId);
        void Save();
    }
}
