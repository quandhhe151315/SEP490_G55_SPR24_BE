using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReviewController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IRatingManager _ratingManager;

        public ReviewController(IConfiguration configuration, IRatingManager ratingManager)
        {
            _configuration = configuration;
            _ratingManager = ratingManager;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            if(id < 0) return BadRequest("Invalid Id");
            List<RecipeRatingDTO> ratings = await _ratingManager.GetRecipeRatings(id);
            if (ratings.Count == 0) return NotFound("There's no rating here!");
            return Ok(ratings);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(RecipeRatingDTO rating)
        {
            if(rating.RecipeId < 0) return BadRequest("Invalid Recipe Id");
            if(rating.UserId < 0) return BadRequest("Invalid User Id");
            if(rating.RatingValue < 0 || rating.RatingValue > 5) return BadRequest("Invalid rating value");
            bool isCreated = await _ratingManager.CreateRating(rating);
            return isCreated ? Ok() : StatusCode(500, "Create Review Failed!");
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update(RecipeRatingDTO rating)
        {
            if(rating.RatingId < 0) return BadRequest("Invalid rating Id");
            if(rating.RatingValue < 0 || rating.RatingValue > 5) return BadRequest("Invalid rating value");
            bool isUpdated = await _ratingManager.UpdateRating(rating);
            return isUpdated ? Ok() : StatusCode(500, "Update Review Failed!");
        }
    }
}
