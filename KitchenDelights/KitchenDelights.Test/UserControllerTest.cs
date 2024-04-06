using Business.DTO;
using Business.Interfaces;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using KitchenDelights.Controllers;
using KitchenDelights.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace KitchenDelights.Test
{
    public class UserControllerTest
    {
        private Mock<IUserManager> _mockUserManager;
        private IConfiguration _configuration;

        public UserControllerTest()
        {
            _mockUserManager = new Mock<IUserManager>();
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build();
        }
        
        // Temporarily commented out for performance
        // [Fact]
        // public async void EmailVerify_ReturnStatus200_ValidEmail() {
        //     EmailEncapsulation email = new() {
        //         Email = "stunghy@gmail.com"
        //     };

        //     UserController _controller = new(_configuration, _mockUserManager.Object);
        //     var result = await _controller.EmailVerify(email);

        //     result.Should().BeOkObjectResult();
        // }

        [Fact]
        public async void EmailVerify_ReturnStatus400_InvalidEmail() {
            EmailEncapsulation email = new() {
                Email = "justastring"
            };

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.EmailVerify(email);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void EmailVerify_ReturnStatus400_EmptyString() {
            EmailEncapsulation email = new() {
                Email = string.Empty
            };

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.EmailVerify(email);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void Register_ReturnStatus200_AllValid()
        {
            RegisterRequestDTO toRegister = new()
            {
                Username = "Test",
                Email = "test@mail.com",
                Password = "123456"
            };
            _mockUserManager.Setup(x => x.CreateUser(It.IsAny<RegisterRequestDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Register(toRegister);

            result.Should().BeOkResult();
        }

        [Fact]
        public async void Register_ReturnStatus400_InvalidEmail()
        {
            RegisterRequestDTO toRegister = new()
            {
                Username = "Test",
                Email = "justastring",
                Password = "123456"
            };
            _mockUserManager.Setup(x => x.CreateUser(It.IsAny<RegisterRequestDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Register(toRegister);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void Register_ReturnStatus406_InvalidPassword()
        {
            RegisterRequestDTO toRegister = new()
            {
                Username = "Test",
                Email = "test@mail.com",
                Password = string.Empty
            };
            _mockUserManager.Setup(x => x.CreateUser(It.IsAny<RegisterRequestDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Register(toRegister);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(406);
        }

        [Fact]
        public async void Register_ReturnStatus500_ExistedUser()
        {
            RegisterRequestDTO toRegister = new()
            {
                Username = "Test",
                Email = "test@mail.com",
                Password = "123456"
            };
            _mockUserManager.Setup(x => x.CreateUser(It.IsAny<RegisterRequestDTO>())).ReturnsAsync(false);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Register(toRegister);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(500);
        }

        [Fact]
        public async void Login_ReturnStatus200_AllValid() {
            LoginRequestDTO login = new() {
                Email = "valid@mail.com",
                Password = "123456"
            };
            _mockUserManager.Setup(x => x.GetUser(login.Email)).ReturnsAsync(new UserDTO() {
                UserId = 1,
                Email = "valid@mail.com",
                Avatar = "avatar-link",
                FirstName = "firstname",
                MiddleName = "middlename",
                LastName = "lastname",
                Role = new() {
                    RoleId = 1,
                    RoleName = "Administrator"
                },
                Status = new() {
                    StatusId = 1,
                    StatusName = "Active"
                },
                PasswordHash = PasswordHelper.Hash(login.Password)
            });

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Login(login);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Login_ReturnStatus400_InvalidEmail() {
            LoginRequestDTO login = new() {
                Email = "justastring",
                Password = "123456"
            };
            _mockUserManager.Setup(x => x.GetUser(login.Email)).ReturnsAsync(new UserDTO() {
                UserId = 1,
                Email = "valid@mail.com",
                Avatar = "avatar-link",
                FirstName = "firstname",
                MiddleName = "middlename",
                LastName = "lastname",
                Role = new() {
                    RoleId = 1,
                    RoleName = "Administrator"
                },
                Status = new() {
                    StatusId = 1,
                    StatusName = "Active"
                },
                PasswordHash = PasswordHelper.Hash(login.Password)
            });

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Login(login);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void Login_ReturnStatus406_InvalidPassword() {
            LoginRequestDTO login = new() {
                Email = "valid@mail.com",
                Password = string.Empty
            };
            _mockUserManager.Setup(x => x.GetUser(login.Email)).ReturnsAsync(new UserDTO() {
                UserId = 1,
                Email = "valid@mail.com",
                Avatar = "avatar-link",
                FirstName = "firstname",
                MiddleName = "middlename",
                LastName = "lastname",
                Role = new() {
                    RoleId = 1,
                    RoleName = "Administrator"
                },
                Status = new() {
                    StatusId = 1,
                    StatusName = "Active"
                },
                PasswordHash = PasswordHelper.Hash(login.Password)
            });

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Login(login);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(406);
        }

        [Fact]
        public async void Login_ReturnStatus404_UserNotExist() {
            LoginRequestDTO login = new() {
                Email = "valid@mail.com",
                Password = "123456"
            };
            _mockUserManager.Setup(x => x.GetUser(login.Email));

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Login(login);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Login_ReturnStatus400_WrongPassword() {
            LoginRequestDTO login = new() {
                Email = "valid@mail.com",
                Password = "wrongpassword"
            };
            _mockUserManager.Setup(x => x.GetUser(login.Email)).ReturnsAsync(new UserDTO() {
                UserId = 1,
                Email = "valid@mail.com",
                Avatar = "avatar-link",
                FirstName = "firstname",
                MiddleName = "middlename",
                LastName = "lastname",
                Role = new() {
                    RoleId = 1,
                    RoleName = "Administrator"
                },
                Status = new() {
                    StatusId = 1,
                    StatusName = "Active"
                },
                PasswordHash = PasswordHelper.Hash("notsamepassword")
            });

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Login(login);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async void Login_ReturnStatus401_BannedUser() {
            LoginRequestDTO login = new() {
                Email = "valid@mail.com",
                Password = "123456"
            };
            _mockUserManager.Setup(x => x.GetUser(login.Email)).ReturnsAsync(new UserDTO() {
                UserId = 1,
                Email = "valid@mail.com",
                Avatar = "avatar-link",
                FirstName = "firstname",
                MiddleName = "middlename",
                LastName = "lastname",
                Role = new() {
                    RoleId = 5,
                    RoleName = "User"
                },
                Status = new() {
                    StatusId = 2,
                    StatusName = "Banned"
                },
                PasswordHash = PasswordHelper.Hash(login.Password)
            });

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Login(login);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(401);
        }

        // Temporarily commented out for performance
        // [Fact]
        // public async void ResetToken_ReturnStatus200_UserExist() {
        //     EmailEncapsulation email = new() {
        //         Email = "stunghy@gmail.com"
        //     };
        //     _mockUserManager.Setup(x => x.CreateResetToken(It.IsAny<ForgotPasswordDTO>())).ReturnsAsync(true);

        //     UserController _controller = new(_configuration, _mockUserManager.Object);
        //     var result = await _controller.ResetToken(email);

        //     result.Should().BeOkResult();
        // }

        [Fact]
        public async void ResetToken_ReturnStatus400_InvalidEmail() {
            EmailEncapsulation email = new() {
                Email = "justastring"
            };
            _mockUserManager.Setup(x => x.CreateResetToken(It.IsAny<ForgotPasswordDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ResetToken(email);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async void ResetToken_ReturnStatus404_UserNotExist() {
            EmailEncapsulation email = new() {
                Email = "stunghy@gmail.com"
            };
            _mockUserManager.Setup(x => x.CreateResetToken(It.IsAny<ForgotPasswordDTO>())).ReturnsAsync(false);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ResetToken(email);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void ForgetPassword_ReturnStatus200_AllValid() {
            ForgotPasswordDTO forget = new() {
                Email = "valid@mail.com",
                Password = "123456",
                ResetToken = "validtoken"
            };
            _mockUserManager.Setup(x => x.ForgetPassword(It.IsAny<ForgotPasswordDTO>())).ReturnsAsync(3);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ForgetPassword(forget);

            result.Should().BeOkResult();
        }

        [Fact]
        public async void ForgetPassword_ReturnStatus400_InvalidEmail() {
            ForgotPasswordDTO forget = new() {
                Email = "justastring",
                Password = "123456",
                ResetToken = "invalidtoken"
            };
            _mockUserManager.Setup(x => x.ForgetPassword(It.IsAny<ForgotPasswordDTO>())).ReturnsAsync(3);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ForgetPassword(forget);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async void ForgetPassword_ReturnStatus400_InvalidPassword() {
            ForgotPasswordDTO forget = new() {
                Email = "justastring",
                Password = string.Empty,
                ResetToken = "invalidtoken"
            };
            _mockUserManager.Setup(x => x.ForgetPassword(It.IsAny<ForgotPasswordDTO>())).ReturnsAsync(3);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ForgetPassword(forget);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async void ForgetPassword_ReturnStatus400_InvalidResetToken() {
            ForgotPasswordDTO forget = new() {
                Email = "justastring",
                Password = "123456",
                ResetToken = string.Empty
            };
            _mockUserManager.Setup(x => x.ForgetPassword(It.IsAny<ForgotPasswordDTO>())).ReturnsAsync(3);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ForgetPassword(forget);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(400);
        }

        [Fact]
        public async void ForgetPassword_ReturnStatus404_UserNotExist() {
            ForgotPasswordDTO forget = new() {
                Email = "valid@mail.com",
                Password = "123456",
                ResetToken = "validtoken"
            };
            _mockUserManager.Setup(x => x.ForgetPassword(It.IsAny<ForgotPasswordDTO>())).ReturnsAsync(0);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ForgetPassword(forget);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void ForgetPassword_ReturnStatus406_WrongToken() {
            ForgotPasswordDTO forget = new() {
                Email = "valid@mail.com",
                Password = "123456",
                ResetToken = "wrongtoken"
            };
            _mockUserManager.Setup(x => x.ForgetPassword(It.IsAny<ForgotPasswordDTO>())).ReturnsAsync(1);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ForgetPassword(forget);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(406);
        }

        [Fact]
        public async void ForgetPassword_ReturnStatus406_TokenExpired() {
            ForgotPasswordDTO forget = new() {
                Email = "valid@mail.com",
                Password = "123456",
                ResetToken = "expiredtoken"
            };
            _mockUserManager.Setup(x => x.ForgetPassword(It.IsAny<ForgotPasswordDTO>())).ReturnsAsync(2);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ForgetPassword(forget);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(406);
        }

        [Fact]
        public async void ChangePassword_ReturnStatus200_AllValid() {
            ChangePasswordDTO change = new() {
                UserId = 1,
                OldPassword = "123456",
                Password = "1234567"
            };
            _mockUserManager.Setup(x => x.GetUser(change.UserId)).ReturnsAsync(new UserDTO() {
                UserId = 1,
                PasswordHash = PasswordHelper.Hash(change.OldPassword)
            });
            _mockUserManager.Setup(x => x.ChangePassword(It.IsAny<ChangePasswordDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ChangePassword(change);

            result.Should().BeOkResult();
        }

        [Fact]
        public async void ChangePassword_ReturnStatus404_UserNotExist() {
            ChangePasswordDTO change = new() {
                UserId = -1,
                OldPassword = "123456",
                Password = "1234567"
            };
            _mockUserManager.Setup(x => x.GetUser(change.UserId));
            _mockUserManager.Setup(x => x.ChangePassword(It.IsAny<ChangePasswordDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ChangePassword(change);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void ChangePassword_ReturnStatus400_InvalidOldPassword() {
            ChangePasswordDTO change = new() {
                UserId = 1,
                OldPassword = string.Empty,
                Password = "1234567"
            };
            _mockUserManager.Setup(x => x.GetUser(change.UserId)).ReturnsAsync(new UserDTO() {
                UserId = 1,
                PasswordHash = PasswordHelper.Hash(change.OldPassword)
            });
            _mockUserManager.Setup(x => x.ChangePassword(It.IsAny<ChangePasswordDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ChangePassword(change);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void ChangePassword_ReturnStatus400_InvalidPassword() {
            ChangePasswordDTO change = new() {
                UserId = 1,
                OldPassword = "123456",
                Password = string.Empty
            };
            _mockUserManager.Setup(x => x.GetUser(change.UserId)).ReturnsAsync(new UserDTO() {
                UserId = 1,
                PasswordHash = PasswordHelper.Hash(change.OldPassword)
            });
            _mockUserManager.Setup(x => x.ChangePassword(It.IsAny<ChangePasswordDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ChangePassword(change);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void ChangePassword_ReturnStatus406_WrongPassword() {
            ChangePasswordDTO change = new() {
                UserId = 1,
                OldPassword = "wrongpassword",
                Password = "1234567"
            };
            _mockUserManager.Setup(x => x.GetUser(change.UserId)).ReturnsAsync(new UserDTO() {
                UserId = 1,
                PasswordHash = PasswordHelper.Hash("123456")
            });
            _mockUserManager.Setup(x => x.ChangePassword(It.IsAny<ChangePasswordDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.ChangePassword(change);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(406);
        }

        [Fact]
        public async void Profile_ReturnStatus200_ValidUserId() {
            var users = GetUserDTOs();
            _mockUserManager.Setup(x => x.GetUser(1)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == 1));

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Profile(1);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Profile_ReturnStatus404_BoundaryUserId() {
            var users = GetUserDTOs();
            _mockUserManager.Setup(x => x.GetUser(0)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == 0));

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Profile(0);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Profile_ReturnStatus400_InvalidUserId() {
            var users = GetUserDTOs();
            _mockUserManager.Setup(x => x.GetUser(-1)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == -1));

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Profile(-1);

            result.Should().BeBadRequestResult();
        }

        [Fact]
        public async void List_ReturnStatus200_ValidUserId() {
            var users = GetUserDTOs();
            _mockUserManager.Setup(x => x.GetUsers(1)).ReturnsAsync(users.Where(x => x.UserId != 1).ToList());

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.List(1);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void List_ReturnStatus200_BoundaryUserId() {
            var users = GetUserDTOs();
            _mockUserManager.Setup(x => x.GetUsers(0)).ReturnsAsync(users.Where(x => x.UserId != 0).ToList());

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.List(0);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void List_ReturnStatus400_InvalidUserId() {
            var users = GetUserDTOs();
            _mockUserManager.Setup(x => x.GetUsers(-1)).ReturnsAsync(users.Where(x => x.UserId != -1).ToList());

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.List(-1);

            result.Should().BeBadRequestResult();
        }

        [Fact]
        public async void UpdateProfile_ReturnStatus200_UserExist() {
            UserDTO toUpdate = new() {
                UserId = 1,
                FirstName = "firstname",
                MiddleName = "middlename",
                LastName = "lastname",
                Email = "valid@mail.com",
                Phone = "0904285035",
                Avatar = "avatar"
            };
            _mockUserManager.Setup(x => x.UpdateProfile(It.IsAny<UserDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.UpdateProfile(toUpdate);

            result.Should().BeOkResult();
        }

        [Fact]
        public async void UpdateProfile_ReturnStatus500_UserNotExist() {
            UserDTO toUpdate = new() {
                UserId = -1,
                FirstName = "firstname",
                MiddleName = "middlename",
                LastName = "lastname",
                Email = "valid@mail.com",
                Phone = "0904285035",
                Avatar = "avatar"
            };
            _mockUserManager.Setup(x => x.UpdateProfile(It.IsAny<UserDTO>())).ReturnsAsync(false);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.UpdateProfile(toUpdate);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(500);
        }

        [Fact]
        public async void UpdateProfile_ReturnStatus400_InvalidEmail() {
            UserDTO toUpdate = new() {
                UserId = 1,
                FirstName = "firstname",
                MiddleName = "middlename",
                LastName = "lastname",
                Email = string.Empty,
                Phone = "0904285035",
                Avatar = "avatar"
            };
            _mockUserManager.Setup(x => x.UpdateProfile(It.IsAny<UserDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.UpdateProfile(toUpdate);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void Create_ReturnStatus200_AllValid() {
            CreateUserDTO toAdd = new() {
                Email = "valid@mail.com",
                Password = "123456",
                RoleId = 5,
                StatusId = 1
            };
            _mockUserManager.Setup(x => x.CreateUser(It.IsAny<CreateUserDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Create(toAdd);

            result.Should().BeOkResult();
        }

        [Fact]
        public async void Create_ReturnStatus400_InvalidEmail() {
            CreateUserDTO toAdd = new() {
                Email = "justastring",
                Password = "123456",
                RoleId = 5,
                StatusId = 1
            };
            _mockUserManager.Setup(x => x.CreateUser(It.IsAny<CreateUserDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Create(toAdd);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void Create_ReturnStatus406_InvalidPassword() {
            CreateUserDTO toAdd = new() {
                Email = "valid@mail.com",
                Password = string.Empty,
                RoleId = 5,
                StatusId = 1
            };
            _mockUserManager.Setup(x => x.CreateUser(It.IsAny<CreateUserDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Create(toAdd);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(406);
        }

        [Fact]
        public async void Create_ReturnStatus400_InvalidRoleId() {
            CreateUserDTO toAdd = new() {
                Email = "valid@mail.com",
                Password = "123456",
                RoleId = 0,
                StatusId = 1
            };
            _mockUserManager.Setup(x => x.CreateUser(It.IsAny<CreateUserDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Create(toAdd);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void Create_ReturnStatus400_InvalidStatusId() {
            CreateUserDTO toAdd = new() {
                Email = "valid@mail.com",
                Password = "123456",
                RoleId = 5,
                StatusId = -1
            };
            _mockUserManager.Setup(x => x.CreateUser(It.IsAny<CreateUserDTO>())).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Create(toAdd);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void Create_ReturnStatus500_DuplicateUser() {
            CreateUserDTO toAdd = new() {
                Email = "valid@mail.com",
                Password = "123456",
                RoleId = 5,
                StatusId = 1
            };
            _mockUserManager.Setup(x => x.CreateUser(It.IsAny<CreateUserDTO>())).ReturnsAsync(false);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Create(toAdd);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(500);
        }

        [Fact]
        public async void Role_ReturnStatus200_AllValid() {
            ChangeRoleDTO change = new() {
                UserId = 1,
                RoleId = 2
            };
            _mockUserManager.Setup(x => x.UpdateRole(change.UserId, change.RoleId)).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Role(change);

            result.Should().BeOkResult();
        }

        [Fact]
        public async void Role_ReturnStatus400_InvalidUserId() {
            ChangeRoleDTO change = new() {
                UserId = -1,
                RoleId = 2
            };
            _mockUserManager.Setup(x => x.UpdateRole(change.UserId, change.RoleId)).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Role(change);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void Role_ReturnStatus400_InvalidRoleId() {
            ChangeRoleDTO change = new() {
                UserId = 1,
                RoleId = 6
            };
            _mockUserManager.Setup(x => x.UpdateRole(change.UserId, change.RoleId)).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Role(change);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void Role_ReturnStatus400_AllInvalid() {
            ChangeRoleDTO change = new() {
                UserId = -1,
                RoleId = 6
            };
            _mockUserManager.Setup(x => x.UpdateRole(change.UserId, change.RoleId)).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Role(change);

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void Role_ReturnStatus400_UserNotExist() {
            ChangeRoleDTO change = new() {
                UserId = 0,
                RoleId = 2
            };
            _mockUserManager.Setup(x => x.UpdateRole(change.UserId, change.RoleId)).ReturnsAsync(false);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Role(change);

            result.Should().BeBadRequestResult();
        }

        [Fact]
        public async void Delete_ReturnStatus200_UserExist() {
            _mockUserManager.Setup(x => x.UpdateStatus(1, 3)).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Delete(1);

            result.Should().BeOkResult();
        }

        [Fact]
        public async void Delete_ReturnStatus400_UserNotExist() {
            _mockUserManager.Setup(x => x.UpdateStatus(0, 3)).ReturnsAsync(false);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Delete(0);

            result.Should().BeBadRequestResult();
        }

        [Fact]
        public async void Delete_ReturnStatus400_InvalidUserId() {
            _mockUserManager.Setup(x => x.UpdateStatus(-1, 3)).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Delete(-1);

            result.Should().BeBadRequestResult();
        }

        [Fact]
        public async void Ban_ReturnStatus200_UserExist() {
            _mockUserManager.Setup(x => x.GetUser(1)).ReturnsAsync(new UserDTO(){
                UserId = 1,
                Status = new StatusDTO {
                    StatusId = 1,
                    StatusName = "Active"
                }
            });
            _mockUserManager.Setup(x => x.UpdateStatus(1, 2)).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Ban(1);

            result.Should().BeOkResult();
        }

        [Fact]
        public async void Ban_ReturnStatus404_UserNotExist() {
            _mockUserManager.Setup(x => x.GetUser(0));
            _mockUserManager.Setup(x => x.UpdateStatus(0, 2)).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Ban(0);

            result.Should().BeNotFoundResult();
        }

        [Fact]
        public async void Ban_ReturnStatus400_UserDeleted() {
            _mockUserManager.Setup(x => x.GetUser(1)).ReturnsAsync(new UserDTO(){
                UserId = 1,
                Status = new StatusDTO {
                    StatusId = 3,
                    StatusName = "Deleted"
                }
            });
            _mockUserManager.Setup(x => x.UpdateStatus(1, 2)).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Ban(1);

            result.Should().BeBadRequestResult();
        }

        [Fact]
        public async void Ban_ReturnStatus400_InvalidUserId() {
            _mockUserManager.Setup(x => x.GetUser(-1));
            _mockUserManager.Setup(x => x.UpdateStatus(-1, 2)).ReturnsAsync(true);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Ban(-1);

            result.Should().BeBadRequestResult();
        }

        [Fact]
        public async void Interact_ReturnStatus200_AllValid() {
            _mockUserManager.Setup(x => x.Interact(1, "recipe")).ReturnsAsync(2);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Interact(1, "recipe");

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Interact_ReturnStatus404_UserNotExist() {
            _mockUserManager.Setup(x => x.Interact(0, "recipe")).ReturnsAsync(0);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Interact(0, "recipe");

            result.Should().BeNotFoundObjectResult();
        }
        
        [Fact]
        public async void Interact_ReturnStatus400_WrongInteractionType() {
            _mockUserManager.Setup(x => x.Interact(1, "wrongtype")).ReturnsAsync(1);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Interact(1, "wrongtype");

            result.Should().BeBadRequestObjectResult();
        }

        [Fact]
        public async void Interact_ReturnStatus400_InvalidUserId() {
            _mockUserManager.Setup(x => x.Interact(-1, "recipe")).ReturnsAsync(0);

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.Interact(-1, "recipe");

            result.Should().BeBadRequestResult();
        }

        private List<UserDTO> GetUserDTOs() {
            List<UserDTO> output = [
                new UserDTO() {
                    UserId = 1
                },
                new UserDTO() {
                    UserId = 2
                },
                new UserDTO() {
                    UserId = 3
                },
                new UserDTO() {
                    UserId = 4
                },
                new UserDTO() {
                    UserId = 5
                },
            ];
            return output;
        }
    }
}