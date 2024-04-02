using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Administrator,Moderator")]
    public class IngMarketplaceController : ControllerBase
    {
        private readonly IIngredientMarketplaceManager _manager;
        private readonly IConfiguration _configuration;

        public IngMarketplaceController(IIngredientMarketplaceManager manager, IConfiguration configuration)
        {
            _manager = manager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            List<IngredientMarketplaceDTO> output = [];
            if(id < 0) return BadRequest("Invalid Id!");
            if(id is null) {
                output = await _manager.GetIngredientMarketplaces();
            } else {
                output = await _manager.GetIngredientMarketplaces(id.Value);
            }
            return Ok(output);
        }

        [HttpPost]
        public async Task<IActionResult> Create(IngredientMarketplaceDTO dto)
        {
            try
            {
                _manager.CreateIngredientMarketplace(dto);
                return Ok();
            }
            catch
            {
                return StatusCode(500, "Create ingredient-marketplace failed!");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> Update(IngredientMarketplaceDTO dto)
        {
            bool isUpdated = await _manager.UpdateIngredientMarketplace(dto);
            return isUpdated ? Ok() : NotFound("Ingredient-Marketplace doesn't exist!");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int ingredientId, int marketplaceId)
        {
            bool isDeleted = await _manager.DeleteIngredientMarketplace(ingredientId, marketplaceId);
            return isDeleted ? Ok() : NotFound("Ingredient-Marketplace doesn't exist!");
        }
    }
}
