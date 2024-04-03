using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Data.Entity;
using KitchenDelights.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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

        //[HttpGet]
        //public async Task<IActionResult> GetAllRecipe()
        //{
        //    List<RecipeDTO> recipes = [];
        //    try
        //    {
        //        recipes = await _recipeManager.GetRecipes();
        //        if (recipes.Count <= 0)
        //        {
        //            return NotFound("There are not exist any recipe in database");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //    return Ok(recipes);
        //}

        [HttpGet]
        public async Task<IActionResult> GetAllRecipe(string? searchName, int? category, int? country, int? ingredient, int? isfree, string? orderby, string? sort)
        {
            List<RecipeDTO> recipes = [];
            try
            {
                recipes = await _recipeManager.FilterRecipe(searchName, category, country, ingredient, isfree, orderby, sort);
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

        //[HttpGet]
        //public async Task<IActionResult> GetAllRecipeDESCbyRating()
        //{
        //    List<RecipeDTO> recipes = [];
        //    try
        //    {
        //        recipes = await _recipeManager.GetRecipesDESC();
        //        if (recipes.Count <= 0)
        //        {
        //            return NotFound("There are not exist any recipe in database");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //    return Ok(recipes);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllRecipeASCbyRating()
        //{
        //    List<RecipeDTO> recipes = [];
        //    try
        //    {
        //        recipes = await _recipeManager.GetRecipesASC();
        //        if (recipes.Count <= 0)
        //        {
        //            return NotFound("There are not exist any recipe in database");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //    return Ok(recipes);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllRecipeFree()
        //{
        //    List<RecipeDTO> recipes = [];
        //    try
        //    {
        //        recipes = await _recipeManager.GetRecipeFree();
        //        if (recipes.Count <= 0)
        //        {
        //            return NotFound("There are not exist any recipe in database");
        //        }
        //        foreach (RecipeDTO recipe in recipes)
        //        {
        //            if (recipe.IsFree)
        //            {
        //                recipe.RecipePrice = 0;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //    return Ok(recipes);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllRecipePaid()
        //{
        //    List<RecipeDTO> recipes = [];
        //    try
        //    {
        //        recipes = await _recipeManager.GetRecipePaid();
        //        if (recipes.Count <= 0)
        //        {
        //            return NotFound("There are not exist any recipe in database");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //    return Ok(recipes);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllRecipeByTitle(string? title)
        //{
        //    List<RecipeDTO> recipes = [];
        //    try
        //    {
        //        if (string.IsNullOrEmpty(title))
        //        {
        //            recipes = await _recipeManager.GetRecipes();
        //        }
        //        else
        //        {
        //            recipes = await _recipeManager.GetRecipeByTitle(title);
        //        }
        //        if (recipes.Count <= 0)
        //        {
        //            return NotFound("Recipe not exist");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //    return Ok(recipes);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllRecipeByCategory(int category)
        //{
        //    List<RecipeDTO> recipes = [];
        //    try
        //    {
        //        recipes = await _recipeManager.GetRecipeByCategory(category);
        //        if (recipes.Count <= 0)
        //        {
        //            return NotFound("Recipe not exist");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //    return Ok(recipes);
        //}

        //[HttpGet]
        //public async Task<IActionResult> GetAllRecipeByCountry(int country)
        //{
        //    List<RecipeDTO> recipes = [];
        //    try
        //    {
        //        recipes = await _recipeManager.GetRecipeByCountry(country);
        //        if (recipes.Count <= 0)
        //        {
        //            return NotFound("Recipe not exist");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        //    }
        //    return Ok(recipes);
        //}

        [HttpGet]
        public async Task<IActionResult> GetRecipeById(int recipeId)
        {
            RecipeDTO? recipe;
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

        [HttpGet]
        public async Task<IActionResult> HighRating(int count)
        {
            return Ok(await _recipeManager.GetRecipeHighRating(count));
        }

        //[HttpGet]
        //public async Task<IActionResult> Search(string? search)
        //{
        //    List<RecipeDTO> recipes = search.IsNullOrEmpty()
        //        ? await _recipeManager.GetRecipes()
        //        : await _recipeManager.SearchRecipe(StringHelper.Process(search));
        //    return Ok(recipes);
        //}

        [HttpGet]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> GetNumberRecipeCreatedInThisMonth()
        {
            int count = await _recipeManager.GetNumberRecipeCreatedInThisMonth();
            return Ok(count);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Moderator,Chef")]
        public async Task<IActionResult> CreateRecipe(RecipeRequestDTO recipe)
        {
            try
            {
                await _recipeManager.CreateRecipe(recipe);
                return Ok(recipe);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Administrator,Moderator,Chef")]
        public async Task<IActionResult> UpdateRecipe(RecipeRequestDTO recipe)
        {
            RecipeDTO? recipeDTO = await _recipeManager.GetRecipe(recipe.RecipeId.Value);
            if (recipeDTO == null) return NotFound("Recipe not exist");

            bool isUpdated = await _recipeManager.UpdateRecipe(recipe);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Update failed!") : Ok("Update sucessful");
        }

        [HttpPut]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> UpdateStatusRecipe(int recipeId, int status)
        {
            RecipeDTO? recipeDTO = await _recipeManager.GetRecipe(recipeId);
            if (recipeDTO == null) return NotFound("Recipe not exist");

            bool isUpdated = await _recipeManager.UpdateStatusRecipe(recipeId, status);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Update status failed!") : Ok("Update status sucessful");
        }

        [HttpPut]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> UpdateCategoryRecipe(int recipeId, int categoryId, int type)
        {
            RecipeDTO? recipeDTO = await _recipeManager.GetRecipe(recipeId);
            if (recipeDTO == null) return NotFound("Recipe not exist");

            bool isUpdated = await _recipeManager.UpdateCategoryRecipe(recipeId, categoryId, type);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Update category failed!") : Ok("Update category sucessful");
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator,Moderator,Chef")]
        public async Task<IActionResult> DeleteRecipe(int recipeId)
        {
            RecipeDTO? recipeDTO = await _recipeManager.GetRecipe(recipeId);
            if (recipeDTO == null) return NotFound("Recipe not exist");

            bool isUpdated = await _recipeManager.UpdateStatusRecipe(recipeId, 0);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Delete failed!") : Ok("Delete sucessful");
        }
    }
}
