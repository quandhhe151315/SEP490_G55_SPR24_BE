using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleManager _roleManager;
        private readonly IConfiguration _configuration;

        public RoleController(IRoleManager roleManager, IConfiguration configuration)
        {
            _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _roleManager.GetRoles());
        }
    }
}
