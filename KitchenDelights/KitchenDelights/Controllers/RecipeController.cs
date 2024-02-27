using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IRecipeManager _recipeManager;

        public RecipeController(IConfiguration configuration, IRecipeManager recipeManager)
        {
            _configuration = configuration;
            _recipeManager = recipeManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe(RecipeDTO recipe)
        {
            if (recipe.RecipeRating == null)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Please enter all require input");
            }
            else
            {
                try
                {
                    _recipeManager.CreateRecipe(recipe);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return Ok(recipe);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRecipe(RecipeDTO recipe)
        {
            if (recipe.RecipeRating == null)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Please enter all require input");
            }
            else
            {
                try
                {
                    if (_recipeManager.GetRecipe(recipe.RecipeId) == null)
                    {
                        return NotFound("Recipe not exist");
                    }
                    _recipeManager.UpdateRecipe(recipe);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return Ok("Update sucessfully");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            try
            {
                if (_recipeManager.GetRecipe(recipeId) == null)
                {
                    return NotFound("Recipe not exist");
                }
                _recipeManager.DeleteRecipe(recipeId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Delete sucessfully");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipe()
        {
            List<RecipeDTO> recipes = await _recipeManager.GetRecipes();
            if (recipes.Count <= 0)
            {
                return NotFound("There are not exist any recipe in database");
            }
            return Ok(recipes);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipeByTitle(string? title)
        {
            List<RecipeDTO> recipes = [];
            if (string.IsNullOrEmpty(title))
            {
                recipes = await _recipeManager.GetRecipes();
            }
            else
            {
                recipes = await _recipeManager.GetRecipeByTitle(title);
            }
            if (recipes.Count <= 0)
            {
                return NotFound("There are not exist any recipe in database");
            }
            return Ok(recipes);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipeByCategory(int category)
        {
            List<RecipeDTO> recipes = await _recipeManager.GetRecipeByCategory(category);
            if (recipes.Count <= 0)
            {
                return NotFound("There are not exist any recipe in database");
            }
            return Ok(recipes);
        }

        [HttpGet]
        public async Task<IActionResult> GetRecipeById(int recipeId)
        {
            RecipeDTO recipe = await _recipeManager.GetRecipe(recipeId);
            if (recipe.RecipeId == 0 || recipe == null)
            {
                return NotFound("Recipe not exist");
            }
            return Ok(recipe);
        }
    }
}
