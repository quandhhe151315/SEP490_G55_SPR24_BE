using Business.DTO;
using Business.Interfaces;
using KitchenDelights.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUserManager _userManager;

        public UserController(IConfiguration configuration, IUserManager accountManager)
        {
            _configuration = configuration;
            _userManager = accountManager;
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
                _userManager.CreateUser(user);
                return Ok();
            } catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequest)
        {
            UserDTO? account = await _userManager.GetUser(loginRequest.Email);
            if(account == null) return NotFound("Account does not exist!");
            bool isCorrectPassword = PasswordHelper.Verify(loginRequest.Password, account.PasswordHash);
            if(isCorrectPassword)
            {
                if (account.Status.StatusName.Equals("Banned")) return Unauthorized("User is being banned!"); 
                return Ok(GenerateJwtToken(account));
            } else
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Wrong password!");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> ResetToken(EmailEncapsulation mail)
        {
            string token = StringHelper.GenerateRandomString(10);

            ForgotPasswordDTO forgotPasswordDTO = new()
            {
                Email = mail.Email,
                ResetToken = token,
            };

            bool isSuccess = await _userManager.CreateResetToken(forgotPasswordDTO);

            if(!isSuccess) return NotFound("User with this email doesn't exist!");

            EmailHelper email = new(_configuration);
            EmailDetails details = new()
            {
                Subject = "Forget Password",
                Body = $"Your code to reset password is: {token}\n\nThis code will expire in 1 hour.\n\nPlease do not share this code with anyone else!\n\nIf you did not send this request, please ignore this email!"
            };

            bool mailStatus = await email.SendEmail(mail, details);

            if (mailStatus)
            {
                return Ok();
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Forget password code has not been sent.");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> ForgetPassword(ForgotPasswordDTO forgotPasswordDTO)
        {
            forgotPasswordDTO.Password = PasswordHelper.Hash(forgotPasswordDTO.Password);
            int status = await _userManager.ForgetPassword(forgotPasswordDTO);
            return status switch
            {
                0 => NotFound("User with this email doesn't exist!"),
                1 => StatusCode(StatusCodes.Status406NotAcceptable, "Wrong forget password code!"),
                2 => StatusCode(StatusCodes.Status406NotAcceptable, "Forget password code expired!"),
                3 => Ok(),
                _ => StatusCode(StatusCodes.Status500InternalServerError, "Something wrong happened, please contact administrator!"),
            };
        }

        [HttpPatch]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changePasswordDTO)
        {
            UserDTO? account = await _userManager.GetUser(changePasswordDTO.UserId);
            if (account == null) return NotFound("Account does not exist!");
            bool isCorrectPassword = PasswordHelper.Verify(changePasswordDTO.OldPassword, account.PasswordHash);
            if (!isCorrectPassword) return StatusCode(StatusCodes.Status406NotAcceptable, "Wrong password!");

            changePasswordDTO.Password = PasswordHelper.Hash(changePasswordDTO.Password);
            bool isSuccess = await _userManager.ChangePassword(changePasswordDTO);
            if (!isSuccess) return NotFound("Account does not exist!");
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            UserDTO? profile = await _userManager.GetUser(id);
            return profile == null ? NotFound("User profile doesn't exist!") : Ok(profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UserDTO userDTO)
        {
            bool isUpdated = await _userManager.UpdateProfile(userDTO);
            return isUpdated ? Ok() : StatusCode(StatusCodes.Status500InternalServerError, "Update profile failed!");
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

            if(account.Avatar.IsNullOrEmpty())
            {
                account.Avatar = "http://localhost:4200/images/avatar/default-avatar.png";
            }

            var claims = new List<Claim>
            {
                new(type: "id", account.UserId.ToString()),
                new(ClaimTypes.Role, account.Role.RoleName),
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
