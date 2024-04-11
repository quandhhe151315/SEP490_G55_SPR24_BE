using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using KitchenDelights.Controllers;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenDelights.Test
{
    public class BookmarkControllerTest
    {
        private Mock<IBookmarkManager> _mockBookmarkManager;
        private Mock<IUserManager> _mockUserManager;
        private IConfiguration _configuration;

        public BookmarkControllerTest()
        {
            _mockBookmarkManager = new Mock<IBookmarkManager>();
            _mockUserManager = new Mock<IUserManager>();
            _configuration = new ConfigurationBuilder().Build();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetBookmarkOfUserExist()
        {
            _mockBookmarkManager.Setup(x => x.GetBookmarkOfUser(1)).ReturnsAsync(new BookmarkDTO()
            {
                UserId = 1,
                Recipes = [
                    new RecipeDTO()
                    {
                        RecipeId = 1
                    },
                    new RecipeDTO()
                    {
                        RecipeId = 2
                    }
                ]
            });

            BookmarkController _controller = new BookmarkController(_configuration, _mockBookmarkManager.Object);
            var result = await _controller.GetBookmarkOfUser(1);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetBookmarkOfUserNotExist()
        {
            _mockBookmarkManager.Setup(x => x.GetBookmarkOfUser(0));

            BookmarkController _controller = new BookmarkController(_configuration, _mockBookmarkManager.Object);
            var result = await _controller.GetBookmarkOfUser(0);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void AddRecipeToBookmark_ReturnStatus200_AllValid()
        {
            BookmarkDTO toUpdate = new BookmarkDTO()
            {
                UserId = 1,
                Recipes = [
                    new RecipeDTO()
                    {
                        RecipeId = 2
                    }
                ]
            };
            _mockBookmarkManager.Setup(x => x.GetBookmarkOfUser(1)).ReturnsAsync(new BookmarkDTO());
            _mockUserManager.Setup(x => x.UpdateProfile(It.IsAny<UserDTO>())).ReturnsAsync(true);

            BookmarkController _controller = new(_configuration, _mockBookmarkManager.Object);
            var result = await _controller.ModifyRecipeInBookMark(1, 1, 1);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void AddRecipeToBookmark_ReturnStatus200_NotFoundUser()
        {
            _mockBookmarkManager.Setup(x => x.GetBookmarkOfUser(0)).ReturnsAsync(new BookmarkDTO());
            _mockUserManager.Setup(x => x.UpdateProfile(It.IsAny<UserDTO>())).ReturnsAsync(false);

            BookmarkController _controller = new(_configuration, _mockBookmarkManager.Object);
            var result = await _controller.ModifyRecipeInBookMark(0, 1, 1);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void AddRecipeToBookmark_ReturnStatus200_NotFoundRecipe()
        {
            _mockBookmarkManager.Setup(x => x.GetBookmarkOfUser(0)).ReturnsAsync(new BookmarkDTO());
            _mockUserManager.Setup(x => x.UpdateProfile(It.IsAny<UserDTO>())).ReturnsAsync(false);

            BookmarkController _controller = new(_configuration, _mockBookmarkManager.Object);
            var result = await _controller.ModifyRecipeInBookMark(1, 0, 1);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void RemoveRecipeToBookmark_ReturnStatus200_AllValid()
        {
            BookmarkDTO toUpdate = new BookmarkDTO()
            {
                UserId = 1,
                Recipes = [
                    new RecipeDTO()
                    {
                        RecipeId = 2
                    }
                ]
            };
            _mockBookmarkManager.Setup(x => x.GetBookmarkOfUser(1)).ReturnsAsync(new BookmarkDTO());
            _mockUserManager.Setup(x => x.UpdateProfile(It.IsAny<UserDTO>())).ReturnsAsync(true);

            BookmarkController _controller = new(_configuration, _mockBookmarkManager.Object);
            var result = await _controller.ModifyRecipeInBookMark(1, 2, 2);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void RemoveRecipeToBookmark_ReturnStatus200_NotFoundUser()
        {
            _mockBookmarkManager.Setup(x => x.GetBookmarkOfUser(0)).ReturnsAsync(new BookmarkDTO());
            _mockUserManager.Setup(x => x.UpdateProfile(It.IsAny<UserDTO>())).ReturnsAsync(false);

            BookmarkController _controller = new(_configuration, _mockBookmarkManager.Object);
            var result = await _controller.ModifyRecipeInBookMark(0, 1, 2);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void RemoveRecipeToBookmark_ReturnStatus200_NotFoundRecipe()
        {
            _mockBookmarkManager.Setup(x => x.GetBookmarkOfUser(1)).ReturnsAsync(new BookmarkDTO());
            _mockUserManager.Setup(x => x.UpdateProfile(It.IsAny<UserDTO>())).ReturnsAsync(false);

            BookmarkController _controller = new(_configuration, _mockBookmarkManager.Object);
            var result = await _controller.ModifyRecipeInBookMark(1, 0, 2);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void ModifyRecipeToBookmark_ReturnStatus200_WrongType()
        {
            _mockBookmarkManager.Setup(x => x.GetBookmarkOfUser(1)).ReturnsAsync(new BookmarkDTO());
            _mockUserManager.Setup(x => x.UpdateProfile(It.IsAny<UserDTO>())).ReturnsAsync(false);

            BookmarkController _controller = new(_configuration, _mockBookmarkManager.Object);
            var result = await _controller.ModifyRecipeInBookMark(1, 0, 0);

            result.Should().BeObjectResult();
        }
    }
}
