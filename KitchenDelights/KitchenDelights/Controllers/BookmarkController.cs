using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BookmarkController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBookmarkManager _bookmarkManager;

        public BookmarkController(IConfiguration configuration, IBookmarkManager bookmarkManager)
        {
            _configuration = configuration;
            _bookmarkManager = bookmarkManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetBookmarkOfUser(int id)
        {
            BookmarkDTO? bookmark = await _bookmarkManager.GetBookmarkOfUser(id);
            return bookmark == null ? NotFound("There not have any recipe in bookmark!") : Ok(bookmark);
        }


        //type 1 to add, 2 to remove 
        [HttpPut]
        public async Task<IActionResult> ModifyRecipeInBookMark(int userId, int recipeId, int type)
        {
            try
            {
                if(type == 1)
                {
                    await _bookmarkManager.AddRecipeToBookmark(userId, recipeId);
                }
                else if(type == 2)
                {
                    await _bookmarkManager.RemoveRecipeFromBookmark(userId, recipeId);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            if (type == 1)
            {
                return Ok("Add recipe to bookmark sucessful");
            }
            else if (type == 2)
            {
                return Ok("Remove recipe to bookmark sucessful");
            }
            return BadRequest("Please try again make sure type 1 to add and 2 to reomove");
        }
    }
}
