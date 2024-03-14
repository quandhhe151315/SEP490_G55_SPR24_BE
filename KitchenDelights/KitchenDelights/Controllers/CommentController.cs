using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ICommentManager _commentManager;

        public CommentController(IConfiguration configuration, ICommentManager commentManager)
        {
            _configuration = configuration;
            _commentManager = commentManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            List<BlogCommentDTO> comments;
            if(id == null)
            {
                comments = await _commentManager.GetComments();
            } else
            {
                comments = await _commentManager.GetComments(id.Value);
            }

            return Ok(comments);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BlogCommentDTO comment)
        {
            comment.CreateDate = DateTime.Now;

            try
            {
                _commentManager.CreateComment(comment);
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(500, $"Create commend failed!\n{ex.StackTrace}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(BlogCommentDTO comment)
        {
            bool isUpdated = await _commentManager.UpdateComment(comment);
            if (!isUpdated) return StatusCode(500, "Update comment failed!");
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await _commentManager.DeleteComment(id);
            if (!isDeleted) return StatusCode(500, "Delete comment failed!");
            return Ok();
        }
    }
}
