using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<List<BlogDTO>> GetBlogs(string? search, int? category, string? sort)
        {
            List<BlogDTO> blogDTOs = [];
            List<Blog> blogs = [];

            //Get all blogs or by category
            if (category != null)
            {
                blogs = await _blogRepository.GetBlogs(category.Value);
            } else
            {
                blogs = await _blogRepository.GetBlogs();
            }

            if (!search.IsNullOrEmpty()) blogs = blogs.Where(x => x.BlogContent.Contains(search, StringComparison.InvariantCultureIgnoreCase)).ToList();

            //Map to list of DTOs
            foreach (Blog blog in blogs)
            {
                blogDTOs.Add(_mapper.Map<Blog, BlogDTO>(blog));
            }

            //Sort list of DTOs
            blogDTOs = sort switch
            {
                "desc" => [.. blogDTOs.OrderByDescending(x => x.CreateDate)],
                "asc" => [.. blogDTOs.OrderBy(x => x.CreateDate)],
                _ => [.. blogDTOs.OrderByDescending(x => x.CreateDate)],
            };

            return blogDTOs;
        }

        //public async Task<List<BlogDTO>> SearchBlogs(string searchString)
        //{
        //    List<BlogDTO> blogDTOs = [];
        //    List<Blog> blogs = await _blogRepository.SearchBlogs(searchString);

        //    foreach(Blog blog in blogs)
        //    {
        //        blogDTOs.Add(_mapper.Map<Blog, BlogDTO>(blog));
        //    }

        //    return blogDTOs;
        //}

        public async Task<List<BlogDTO>> GetBlogsLastest(int count)
        {
            List<BlogDTO> blogDTOs = [];
            List<Blog> blogs = await _blogRepository.GetBlogs();
            blogs = blogs.OrderByDescending(x => x.CreateDate)
                         .Take(count)
                         .ToList();
            blogDTOs.AddRange(blogs.Select(_mapper.Map<Blog, BlogDTO>));
            return blogDTOs;
        }

        public async Task<bool> BlogStatus(int id, int status) {
            Blog? blog = await _blogRepository.GetBlog(id);
            if (blog == null || status != 1 && status != 2) return false;
            blog.BlogStatus = status;
            _blogRepository.UpdateBlog(blog);
            _blogRepository.Save();
            return true;
        }
    }
}
