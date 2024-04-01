using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Data.Entity;
using Microsoft.AspNetCore.Authorization;
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
        public async Task<IActionResult> GetIngredientByName(string? name)
        {
            List<IngredientDTO> ingredients = [];
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    ingredients = await _ingredientManager.GetAllIngredients();
                    return Ok(ingredients);
                }
                else
                {
                    ingredients = await _ingredientManager.GetIngredientsByName(name);
                    if (ingredients.Count <= 0)
                    {
                        return NotFound("There are not exist any ingredient in database");
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(ingredients);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(IngredientRequestDTO ingredientDTO)
        {
            bool isCreated = await _ingredientManager.CreateIngredient(ingredientDTO);
            return !isCreated ? StatusCode(StatusCodes.Status500InternalServerError, "Create failed!") : Ok("Create sucess!");
        }

        [HttpPut]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Update(IngredientRequestDTO ingredientDTO)
        {
            IngredientDTO? IngredientDTO = await _ingredientManager.GetIngredientById(ingredientDTO.IngredientId);
            if (IngredientDTO == null) return NotFound("Ingredient doesn't exist!");

            bool isUpdated = await _ingredientManager.UpdateIngredient(ingredientDTO);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Update failed!") : Ok("Update sucess!");
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            IngredientDTO? IngredientDTO = await _ingredientManager.GetIngredientById(id);
            if (IngredientDTO == null) return NotFound("Ingredient doesn't exist!");

            bool isDeleted = await _ingredientManager.DeleteIngredient(id);
            return !isDeleted ? StatusCode(StatusCodes.Status500InternalServerError, "Delete failed!") : Ok("Delete sucess!");
        }
    }
}
