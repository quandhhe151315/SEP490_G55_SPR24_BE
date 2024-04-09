using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IBlogManager
    {
        void CreateBlog(BlogDTO blog);

        Task<bool> UpdateBlog(BlogDTO blog);

        Task<bool> DeleteBlog(int id);

        Task<BlogDTO?> GetBlog(int id);

         Task<List<BlogDTO>> GetBlogs(int id);

        Task<List<BlogDTO>> GetBlogs(string? search, int? category, string? sort);

        //Task<List<BlogDTO>> SearchBlogs(string searchString);

        Task<List<BlogDTO>> GetBlogsLastest(int count);

        Task<bool> BlogStatus(int id, int status);
    }
}
