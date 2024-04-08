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
    public class MarketplaceController : Controller
    {
        private readonly IMarketplaceManager _marketplaceManager;
        private readonly IConfiguration _configuration;

        public MarketplaceController(IMarketplaceManager marketplaceManager, IConfiguration configuration)
        {
            _marketplaceManager = marketplaceManager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _marketplaceManager.GetMarketplaces());
        }

        [HttpPost]
        public async Task<IActionResult> Create(MarketplaceDTO marketplace)
        {
            if(marketplace.MarketplaceName.IsNullOrEmpty()) return BadRequest("Marketplace name should not be empty");
            if(marketplace.MarketplaceLogo.IsNullOrEmpty()) return BadRequest("Marketplace logo should not be empty");
            try
            {
                marketplace.MarketplaceStatus = 1;
                _marketplaceManager.CreateMarketplace(marketplace);
                return Ok();
            }
            catch
            {
                return StatusCode(500, "Create Marketplace failed!");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(MarketplaceDTO marketplace)
        {
            if (marketplace.MarketplaceId is null) return StatusCode(406, "Id should not be null");
            if (marketplace.MarketplaceId != null && marketplace.MarketplaceId < 0) return BadRequest("Invalid marketplace Id");
            if(marketplace.MarketplaceName.IsNullOrEmpty()) return BadRequest("Marketplace name should not be empty");
            if(marketplace.MarketplaceLogo.IsNullOrEmpty()) return BadRequest("Marketplace logo should not be empty");
            bool isUpdated = await _marketplaceManager.UpdateMarketplace(marketplace);
            return isUpdated? Ok() : NotFound("Marketplace doesn't exist!");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            if(id < 0) return BadRequest("Invalid Marketplace Id");
            bool isDeleted = await _marketplaceManager.DeleteMarketplace(id);
            return isDeleted ? Ok() : NotFound("Marketplace doesn't exist!");
        }

        [HttpPatch]
        public async Task<IActionResult> Status(int id)
        {
            if(id < 0) return BadRequest("Invalid Marketplace Id");
            bool isUpdated = await _marketplaceManager.UpdateStatus(id);
            return isUpdated ? Ok() : NotFound("Marketplace doesn't exist!");
        }
    }
}
