using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
            if (id == null)
            {
                comments = await _commentManager.GetComments();
            } else
            {
                if (id < 0) return BadRequest("Invalid Id");
                comments = await _commentManager.GetComments(id.Value);
            }

            return Ok(comments);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(BlogCommentDTO comment)
        {
            if(comment.BlogId < 0) return BadRequest("Invalid blog Id");
            if(comment.UserId < 0) return BadRequest("Invalid user Id");
            if(comment.ParentId != null && comment.ParentId.Value < 0) return BadRequest("Invalid parent comment Id");
            comment.CreateDate = DateTime.Now;
            try
            {
                int commentId =  await _commentManager.CreateComment(comment);
                return Ok(commentId);
            } catch (Exception ex)
            {
                return StatusCode(500, $"Create commend failed!\n{ex.StackTrace}");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(BlogCommentDTO comment)
        {
            if(comment.CommentId < 0) return BadRequest("Invalid blog Id");
            bool isUpdated = await _commentManager.UpdateComment(comment);
            if (!isUpdated) return StatusCode(500, "Update comment failed!");
            return Ok();
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if(id < 0) return BadRequest("Invalid Id");
            bool isDeleted = await _commentManager.DeleteComment(id);
            if (!isDeleted) return StatusCode(500, "Delete comment failed!");
            return Ok();
        }
    }
}
