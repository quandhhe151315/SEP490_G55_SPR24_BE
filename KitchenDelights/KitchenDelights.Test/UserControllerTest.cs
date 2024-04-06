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

        [Fact]
        public async void EmailVerify_ReturnStatus200_ValidEmail() {
            EmailEncapsulation email = new() {
                Email = "stunghy@gmail.com"
            };

            UserController _controller = new(_configuration, _mockUserManager.Object);
            var result = await _controller.EmailVerify(email);

            result.Should().BeOkObjectResult();
        }

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
    }
}