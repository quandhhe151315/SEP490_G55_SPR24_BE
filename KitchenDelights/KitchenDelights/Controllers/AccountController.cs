using Business.DTO;
using Business.Interfaces;
using KitchenDelights.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountManager _accountManager;

        public AccountController(IConfiguration configuration, IAccountManager accountManager)
        {
            _configuration = configuration;
            _accountManager = accountManager;
        }

        [HttpPost]
        public async Task<IActionResult> EmailVerify(EmailEncapsulation emailAddress)
        {
            string verificationCode = StringHelper.GenerateRandomString(10);
            EmailHelper email = new(_configuration);
            EmailDetails details = new()
            {
                Subject = "Email Verification",
                Body = $"Your verification code is: {verificationCode}\n\nPlease do not share this code with anyone else!"
            };
            bool mailStatus = await email.SendEmail(emailAddress, details);
            if(mailStatus)
            {
                return Ok(verificationCode);
            } else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Verification code has not been sent.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDTO account)
        {
            account.Password = PasswordHelper.Hash(account.Password);
            account.RoleId = 5; //Default "User" Role
            account.StatusId = 1; // Default "Active" status
            try
            {
                _accountManager.CreateAccount(account);
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
