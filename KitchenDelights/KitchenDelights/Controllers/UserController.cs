using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using KitchenDelights.Helper;
using Microsoft.AspNetCore.Authorization;
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
    public class UserController : Controller
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
            if (!StringHelper.IsEmail(emailAddress.Email)) return BadRequest("Please input a valid email address!");
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
            if (user.Email.IsNullOrEmpty() || !StringHelper.IsEmail(user.Email!)) return BadRequest("Please input a valid email address!");

            if(user.Password.IsNullOrEmpty() || user.Password!.Length < 6)
            {
                return StatusCode(406,"Password should not be shorter than 6 characters!");
            }

            user.Password = PasswordHelper.Hash(user.Password);
            user.RoleId = 5; //Default "User" Role
            user.StatusId = 1; // Default "Active" status
            
            bool isCreated = await _userManager.CreateUser(user);
            return isCreated ? Ok() : StatusCode(500, "Register failed!");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequest)
        {
            if (!StringHelper.IsEmail(loginRequest.Email)) return BadRequest("Please input a valid email address!");
            if(loginRequest.Password.IsNullOrEmpty()) return StatusCode(406, "Password should not be empty!");
            UserDTO? account = await _userManager.GetUser(loginRequest.Email);
            if(account == null || account.Status.StatusId == 0) return NotFound("Account does not exist!");
            bool isCorrectPassword = PasswordHelper.Verify(loginRequest.Password, account.PasswordHash);
            if(isCorrectPassword)
            {
                if (account.Status.StatusName.Equals("Banned")) return Unauthorized("User is being banned!"); 
                return Ok(GenerateJwtToken(account));
            } else
            {
                return StatusCode(400, "Wrong password!");
            }
        }

        [HttpPatch]
        public async Task<IActionResult> ResetToken(EmailEncapsulation mail)
        {
            if (!StringHelper.IsEmail(mail.Email)) return BadRequest("Please input a valid email address!");
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

        [Authorize]
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

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpGet]
        public async Task<IActionResult> List(int id)
        {
            List<UserDTO> users = await _userManager.GetUsers(id);
            return Ok(users);
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> UpdateProfile(UserDTO userDTO)
        {
            bool isUpdated = await _userManager.UpdateProfile(userDTO);
            return isUpdated ? Ok() : StatusCode(StatusCodes.Status500InternalServerError, "Update profile failed!");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDTO userDTO)
        {
            if(userDTO.Password.Length < 6) return StatusCode(StatusCodes.Status406NotAcceptable, "Password should not be shorter than 6 characters!");

            userDTO.Password = PasswordHelper.Hash(userDTO.Password);

            bool isCreated = await _userManager.CreateUser(userDTO);
            return isCreated ? Ok() : StatusCode(500, "Create new user failed!");
        }

        [Authorize(Roles = "Administrator")]
        [HttpPatch]
        public async Task<IActionResult> Role(ChangeRoleDTO changeRole)
        {
            bool isUpdated = await _userManager.UpdateRole(changeRole.UserId, changeRole.RoleId);
            return isUpdated ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> Delete(int id)
        {
            if(id < 0) return BadRequest();
            bool isUpdated = await _userManager.UpdateStatus(id, 3);
            return isUpdated ? Ok() : BadRequest();
        }

        [Authorize(Roles = "Administrator,Moderator")]
        [HttpPatch]
        public async Task<IActionResult> Ban(int id)
        {
            if(id < 0) return BadRequest();
            UserDTO? userDTO = await _userManager.GetUser(id);
            if(userDTO == null) return NotFound();
            bool isUpdated = userDTO.Status!.StatusId switch
            {
                1 => await _userManager.UpdateStatus(id, 2),
                2 => await _userManager.UpdateStatus(id, 1),
                _ => false,
            };
            return isUpdated ? Ok() : BadRequest();
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> Interact(int id, string type)
        {
            if(id < 0) return BadRequest();
            int interactResult = await _userManager.Interact(id, type);
            return interactResult switch
            {
                0 => NotFound("User doesn't exist!"),
                1 => BadRequest("Wrong interaction type!"),
                2 => Ok(false),//Interact successfully, not enough point for voucher
                3 => Ok(true),//Interact successfully, enough point for voucher
                _ => StatusCode(500, "Something wrong happened with the server!"),
            };
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
                account.Avatar = $"{Request.Scheme}://{Request.Host.Value}/images/avatar/default-avatar.png";
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
