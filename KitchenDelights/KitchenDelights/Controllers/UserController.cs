using Business.DTO;
using Business.Interfaces;
using KitchenDelights.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserManager _accountManager;

        public UserController(IConfiguration configuration, IUserManager accountManager)
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
        public async Task<IActionResult> Register(RegisterRequestDTO user)
        {
            if(user.Password.ToCharArray().Length < 6)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable,"Password should not be shorter than 6 characters!");
            }

            user.Password = PasswordHelper.Hash(user.Password);
            user.RoleId = 5; //Default "User" Role
            user.StatusId = 1; // Default "Active" status
            try
            {
                _accountManager.CreateUser(user);
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequest)
        {
            UserDTO? account = await _accountManager.GetUser(loginRequest.Email);
            if(account == null)
            {
                return NotFound("Account does not exist!");
            }
            bool isCorrectPassword = PasswordHelper.Verify(loginRequest.Password, account.PasswordHash);
            if(isCorrectPassword)
            {
                if (account.StatusName.Equals("Banned")) 
                {
                    return Unauthorized("User is being banned!"); 
                }
                return Ok(GenerateJwtToken(account));
            } else
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Wrong password!");
            }
        }

        private string GenerateJwtToken(UserDTO account)
        {
            string name;
            if (account.MiddleName.IsNullOrEmpty())
            {
                name = account.LastName + ' ' + account.FirstName;
            } else
            {
                name = account.LastName + ' ' + account.MiddleName + ' ' + account.FirstName;
            }

            if(name.Trim().IsNullOrEmpty())
            {
                name = "Người dùng";
            }

            var claims = new List<Claim>
            {
                new(type: "id", account.UserId.ToString()),
                new(ClaimTypes.Role, account.RoleName),
                new(type: "name", name.Trim()),
                new(ClaimTypes.Email, account.Email),
                new(type: "avatar", account.Avatar),
                new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"]),
                new(Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"])
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7), // Set token expiration time
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
