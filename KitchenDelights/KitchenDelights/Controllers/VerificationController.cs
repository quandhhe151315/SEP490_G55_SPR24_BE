using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VerificationController : Controller
    {
        private readonly IVerificationManager _verificationManager;
        private readonly IConfiguration _configuration;

        public VerificationController(IVerificationManager verificationManager, IConfiguration configuration)
        {
            _verificationManager = verificationManager;
            _configuration = configuration;
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpGet]
        public async Task<IActionResult> Get(int? id)
        {
            if(id == null) return Ok(await _verificationManager.GetVerifications());
            if (id < 0) return BadRequest();
            var result = await _verificationManager.GetVerification(id.Value);
            return result is not null ? Ok(result) : NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(VerificationDTO verification)
        {
            verification.VerificationStatus = 0; //Default to not yet verified
            verification.VerificationDate = DateTime.Now;

            bool isCreated = await _verificationManager.CreateVerification(verification);
            return isCreated ? Ok() : BadRequest("Create Verification entry failed!");
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPatch]
        public async Task<IActionResult> Update(VerificationDTO verification)
        {
            verification.VerificationDate = DateTime.Now;
            if (verification.VerificationStatus is not 1 and not 2) return BadRequest("Wrong Verification status!");

            bool isUpdated = await _verificationManager.UpdateVerification(verification);
            return isUpdated ? Ok() : BadRequest("Update Verification entry failed!");
        }
    }
}
