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
            bool isUpdated = await _marketplaceManager.UpdateMarketplace(marketplace);
            return isUpdated? Ok() : NotFound("Marketplace doesn't exist!");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await _marketplaceManager.DeleteMarketplace(id);
            return isDeleted ? Ok() : NotFound("Marketplace doesn't exist!");
        }
    }
}
