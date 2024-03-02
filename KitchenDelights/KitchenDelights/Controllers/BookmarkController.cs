﻿using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]")]
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

        [HttpPut]
        public async Task<IActionResult> AddRecipeToBookMark(int userId, int recipeId)
        {
            try
            {
                await _bookmarkManager.AddRecipeToBookmark(userId, recipeId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Add recipe to bookmark sucessful");
        }
    }
}
