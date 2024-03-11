using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IIngredientManager _ingredientManager;

        public IngredientController(IConfiguration configuration, IIngredientManager ingredientManager)
        {
            _configuration = configuration;
            _ingredientManager = ingredientManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllIngredient()
        {
            List<IngredientDTO> ingredients = [];
            try
            {
                ingredients = await _ingredientManager.GetAllIngredients();
                if (ingredients.Count < 0)
                {
                    return NotFound("There are not exist any ingredient in database");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(ingredients);
        }

        [HttpGet]
        public async Task<IActionResult> GetIngredientById(int ingredientId)
        {
            try
            {
                if(ingredientId != 0)
                {
                    IngredientDTO? ingredient = await _ingredientManager.GetIngredientById(ingredientId);
                    if (ingredient == null)
                    {
                        return NotFound("ingredient not exist");
                    }
                    return Ok(ingredient);
                }
                else
                {
                    List<IngredientDTO>  ingredients = await _ingredientManager.GetAllIngredients();
                    if (ingredients.Count < 0)
                    {
                        return NotFound("There are not exist any ingredient in database");
                    }
                    return Ok(ingredients);
                }
                
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetIngredientByName(string name)
        {
            List<IngredientDTO> ingredients = [];
            try
            {
                ingredients = await _ingredientManager.GetIngredientsByName(name);
                if (ingredients.Count <= 0)
                {
                    return NotFound("There are not exist any ingredient in database");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(ingredients);
        }
    }
}
