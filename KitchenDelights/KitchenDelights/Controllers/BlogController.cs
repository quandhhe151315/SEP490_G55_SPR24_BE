using Business.DTO;
using Business.Interfaces;
using KitchenDelights.Helper;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> Get(int? id, int? category, string? sort, string? search, int? userId)
        {
            if (id == null && userId == null)
            {
                if(category != null && category < 0) return BadRequest("Invalid Category Id");
                List<BlogDTO> blogs = await _blogManager.GetBlogs(search.IsNullOrEmpty() ? search : StringHelper.Process(search), category, sort);
                if (!User.IsInRole("Administrator") || !User.IsInRole("Moderator")) blogs = blogs.Where(x => x.BlogStatus == 1).ToList();
                if (blogs.Count == 0) return NotFound("There's no blog here!");
                return Ok(blogs);
            } else if (id == null && userId != null) {
                List<BlogDTO> blogs = await _blogManager.GetBlogs(userId.Value);
                return Ok(blogs);
            }

            if(id != null && id < 0) return BadRequest("Id invalid!");
            BlogDTO? blog = await _blogManager.GetBlog(id.Value);
            if (blog == null) return NotFound("Blog doesn't exist!");
            return Ok(blog);
        }

        [HttpGet]
        public async Task<IActionResult> Lastest(int count)
        {
            if(count < 0) return BadRequest();
            return Ok(await _blogManager.GetBlogsLastest(count));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(BlogDTO blog)
        {
            if(StringHelper.Process(blog.BlogTitle).IsNullOrEmpty()) return StatusCode(406, "Blog Title should not be empty!");
            if(StringHelper.Process(blog.BlogImage).IsNullOrEmpty()) return StatusCode(406, "Blog Image should not be empty!");
            blog.CreateDate = DateTime.Now;
            blog.BlogStatus = 1;           
            try
            {
                _blogManager.CreateBlog(blog);
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(BlogDTO blog)
        {
            if(blog.BlogId < 0) return BadRequest("Invalid Blog Id!");
            if(StringHelper.Process(blog.BlogTitle).IsNullOrEmpty()) return StatusCode(406, "Blog Title should not be empty!");
            if(StringHelper.Process(blog.BlogImage).IsNullOrEmpty()) return StatusCode(406, "Blog Image should not be empty!");
            bool isUpdated = await _blogManager.UpdateBlog(blog);
            return isUpdated ? Ok() : StatusCode(500, "Update blog failed!");
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPatch]
        public async Task<IActionResult> Status(int id, int status) {
            if(id < 0) return BadRequest("Invalid Blog Id");
            if(status != 1 && status != 2) return BadRequest("Invalid Status");
            bool isUpdated = await _blogManager.BlogStatus(id, status);
            return isUpdated ? Ok() : StatusCode(500, "Update blog status failed!");
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if(id < 0) return BadRequest("Invalid Id");
            bool isDeleted = await _blogManager.DeleteBlog(id);
            return isDeleted ? Ok() : StatusCode(500, "Delete blog failed!");
        }
    }
}
