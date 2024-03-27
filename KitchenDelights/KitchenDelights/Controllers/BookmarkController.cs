using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> GetBookmarkOfUser(int id)
        {
            BookmarkDTO? bookmark = await _bookmarkManager.GetBookmarkOfUser(id);
            return Ok(bookmark);
        }


        //type 1 to add, 2 to remove 
        [HttpPut]
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> ModifyRecipeInBookMark(int userId, int recipeId, int type)
        {
            try
            {
                if (userId == 0 || recipeId == 0)
                {
                    return BadRequest("please enter require input");
                }
                BookmarkDTO? bookmark = await _bookmarkManager.GetBookmarkOfUser(userId);
                if (bookmark == null)
                {
                    return NotFound("User not exist");
                }
                switch (type)
                {
                    case 1:
                        foreach (RecipeDTO recipe in bookmark.Recipes)
                        {
                            if (recipe.RecipeId == recipeId)
                            {
                                return StatusCode(StatusCodes.Status406NotAcceptable, "Already exist this recipe in bookmark");
                            }
                        }
                        bool isAdded = await _bookmarkManager.AddRecipeToBookmark(userId, recipeId);
                        if (isAdded) return Ok("Add recipe to bookmark sucessful");
                        break;
                    case 2:
                        int count = 0;
                        foreach (RecipeDTO recipe in bookmark.Recipes)
                        {
                            if (recipe.RecipeId == recipeId)
                            {
                                count++;
                                await _bookmarkManager.RemoveRecipeFromBookmark(userId, recipeId);
                            }
                        }
                        if (count <= 0)
                            return StatusCode(StatusCodes.Status406NotAcceptable, "Please enter exist recipe in bookmark to delete");
                        else
                            return Ok("Remove recipe from bookmark sucessful");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            };
            return BadRequest("Please try again make sure type 1 to add and 2 to remove");
        }
    }
}
