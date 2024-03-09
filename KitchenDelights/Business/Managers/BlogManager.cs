using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class BlogManager : IBlogManager
    {
        private readonly IBlogRepository _blogRepository;
        private readonly IMapper _mapper;

        public BlogManager(IBlogRepository blogRepository, IMapper mapper)
        {
            _blogRepository = blogRepository;
            _mapper = mapper;
        }

        public void CreateBlog(BlogDTO blog)
        {
            _blogRepository.CreateBlog(_mapper.Map<BlogDTO, Blog>(blog));
            _blogRepository.Save();
        }

        public async Task<bool> UpdateBlog(BlogDTO blogDTO)
        {
            Blog? blog = await _blogRepository.GetBlog(blogDTO.BlogId);
            if (blog == null) return false;

            //Update partially
            blog.CategoryId = blogDTO.CategoryId;
            blog.BlogTitle = blogDTO.BlogTitle;
            blog.BlogContent = blogDTO.BlogContent;
            blog.BlogImage = blogDTO.BlogImage;

            _blogRepository.UpdateBlog(blog);
            _blogRepository.Save();
            return true;
        }

        public async Task<bool> DeleteBlog(int id)
        {
            Blog? blog = await _blogRepository.GetBlog(id);
            if (blog == null) return false;

            blog.BlogStatus = 0;
            _blogRepository.UpdateBlog(blog);
            _blogRepository.Save();
            return true;
        }

        public async Task<BlogDTO?> GetBlog(int id)
        {
            Blog? blog = await _blogRepository.GetBlog(id);
            return blog is null ? null : _mapper.Map<Blog, BlogDTO>(blog);
        }

        public async Task<List<BlogDTO>> GetBlogs()
        {
            List<Blog> blogs = await _blogRepository.GetBlogs();
            List<BlogDTO> blogDTOs = [];
            foreach (Blog blog in blogs)
            {
                blogDTOs.Add(_mapper.Map<Blog, BlogDTO>(blog));
            }
            return blogDTOs;
        }
    }
}
