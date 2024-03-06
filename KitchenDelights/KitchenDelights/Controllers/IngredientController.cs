using Business.DTO;
using Business.Interfaces;
using Business.Managers;
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
                ingredients = _ingredientManager.GetAllIngredients();
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
            IngredientDTO ingredient;
            try
            {
                ingredient = _ingredientManager.GetIngredientById(ingredientId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(ingredient);
        }

        [HttpGet]
        public async Task<IActionResult> GetIngredientByName(string name)
        {
            List<IngredientDTO> ingredients = [];
            try
            {
                ingredients = _ingredientManager.GetIngredientsByName(name);
                if (ingredients.Count < 0)
                {
                    return NotFound("Ingredient not exist");
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
