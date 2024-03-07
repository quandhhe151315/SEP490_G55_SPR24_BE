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

        [HttpGet]
        public async Task<IActionResult> GetAllRecipe()
        {
            List<RecipeDTO> recipes = [];
            try
            {
                recipes = await _recipeManager.GetRecipes();
                if (recipes.Count <= 0)
                {
                    return NotFound("There are not exist any recipe in database");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(recipes);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipeFree()
        {
            List<RecipeDTO> recipes = [];
            try
            {
                recipes = await _recipeManager.GetRecipeFree();
                if (recipes.Count <= 0)
                {
                    return NotFound("There are not exist any recipe in database");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(recipes);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipePaid()
        {
            List<RecipeDTO> recipes = [];
            try
            {
                recipes = await _recipeManager.GetRecipePaid();
                if (recipes.Count <= 0)
                {
                    return NotFound("There are not exist any recipe in database");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(recipes);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipeByTitle(string? title)
        {
            List<RecipeDTO> recipes = [];
            try
            {
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
                    return NotFound("Recipe not exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(recipes);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRecipeByCategory(int category)
        {
            List<RecipeDTO> recipes = [];
            try
            {
                recipes = await _recipeManager.GetRecipeByCategory(category);
                if (recipes.Count <= 0)
                {
                    return NotFound("Recipe not exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(recipes);
        }

        [HttpGet]
        public async Task<IActionResult> GetRecipeById(int recipeId)
        {
            RecipeDTO recipe;
            try
            {
                recipe = await _recipeManager.GetRecipe(recipeId);
                if (recipe == null)
                {
                    return NotFound("Recipe not exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(recipe);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRecipe(RecipeRequestDTO recipe)
        {
            recipe.RecipeStatus = false;
            recipe.CreateDate = DateTime.Now;
            try
            {
                _recipeManager.CreateRecipe(recipe);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(recipe);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRecipe(RecipeRequestDTO recipe)
        {
            try
            {
                if (await _recipeManager.GetRecipe(recipe.RecipeId.Value) == null)
                {
                    return NotFound("Recipe not exist");
                }
                await _recipeManager.UpdateRecipe(recipe);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Update sucessfully");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStatusRecipe(int recipeId, bool status)
        {
            try
            {
                if (await _recipeManager.GetRecipe(recipeId) == null)
                {
                    return NotFound("Recipe not exist");
                }
                await _recipeManager.UpdateStatusRecipe(recipeId, status);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Update sucessfully");
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCategoryRecipe(int recipeId, int categoryId, int type)
        {
            try
            {
                if (await _recipeManager.GetRecipe(recipeId) == null)
                {
                    return NotFound("Recipe not exist");
                }
                await _recipeManager.UpdateCategoryRecipe(recipeId, categoryId, type);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Update sucessfully");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            try
            {
                if (await _recipeManager.GetRecipe(recipeId) == null)
                {
                    return NotFound("Recipe not exist");
                }
                await _recipeManager.DeleteRecipe(recipeId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Delete sucessfully");
        }
    }
}
