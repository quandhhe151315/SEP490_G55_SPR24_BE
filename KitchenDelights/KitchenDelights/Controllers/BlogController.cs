﻿using Business.DTO;
using Business.Interfaces;
using KitchenDelights.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BlogController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IBlogManager _blogManager;

        public BlogController(IConfiguration configuration, IBlogManager blogManager)
        {
            _configuration = configuration;
            _blogManager = blogManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? id, int? category, string? sort)
        {
            if (id == null)
            {
                List<BlogDTO> blogs = await _blogManager.GetBlogs(category, sort);
                if (blogs.Count == 0) return NotFound("There's no blog here!");
                return Ok(blogs);
            }

            BlogDTO? blog = await _blogManager.GetBlog(id.Value);
            if (blog == null) return NotFound("Blog doesn't exist!");
            return Ok(blog);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? search)
        {
            List<BlogDTO> blogs;
            if(search.IsNullOrEmpty()) {
                blogs = await _blogManager.GetBlogs(null, null);
            } else
            {
                blogs = await _blogManager.SearchBlogs(StringHelper.Process(search));
            }
            return Ok(blogs);
        }

        [HttpGet]
        public async Task<IActionResult> Lastest(int count)
        {
            return Ok(await _blogManager.GetBlogsLastest(count));
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogDTO blog)
        {
            blog.CreateDate = DateTime.Now;
            blog.BlogStatus = 1;           
            try
            {
                _blogManager.CreateBlog(blog);
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(BlogDTO blog)
        {
            bool isUpdated = await _blogManager.UpdateBlog(blog);
            return isUpdated ? Ok() : StatusCode(500, "Update blog failed!");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await _blogManager.DeleteBlog(id);
            return isDeleted ? Ok() : StatusCode(500, "Delete blog failed!");
        }
    }
}
