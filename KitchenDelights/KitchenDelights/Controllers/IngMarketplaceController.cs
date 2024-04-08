using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

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
            if(dto.IngredientId < 0) return BadRequest("Invalid ingredient Id");
            if(dto.MarketplaceId < 0) return BadRequest("Invalid marketplace Id");
            if(dto.MarketplaceLink.IsNullOrEmpty()) return BadRequest("Link must not be empty!");
            bool isCreated = await _manager.CreateIngredientMarketplace(dto);
            return isCreated ? Ok("Create sucessful") : StatusCode(500, "Create ingredient-marketplace failed!");
        }

        [HttpPatch]
        public async Task<IActionResult> Update(IngredientMarketplaceDTO dto)
        {
            if(dto.IngredientId < 0) return BadRequest("Invalid ingredient Id");
            if(dto.MarketplaceId < 0) return BadRequest("Invalid marketplace Id");
            if(dto.MarketplaceLink.IsNullOrEmpty()) return BadRequest("Link must not be empty!");
            bool isUpdated = await _manager.UpdateIngredientMarketplace(dto);
            return isUpdated ? Ok() : NotFound("Ingredient-Marketplace doesn't exist!");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int ingredientId, int marketplaceId)
        {
            if(ingredientId < 0) return BadRequest("Invalid ingredient Id");
            if(marketplaceId < 0) return BadRequest("Invalid marketplace Id");
            bool isDeleted = await _manager.DeleteIngredientMarketplace(ingredientId, marketplaceId);
            return isDeleted ? Ok() : NotFound("Ingredient-Marketplace doesn't exist!");
        }
    }
}
