using Business.DTO;
using Business.Interfaces;
using FluentAssertions.AspNetCore.Mvc;
using KitchenDelights.Controllers;
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
            _configuration = new ConfigurationBuilder().Build();
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

            UserController controller = new(_configuration, _mockUserManager.Object);
            var result = await controller.Register(toRegister);

            result.Should().BeOkResult();
        }
    }
}