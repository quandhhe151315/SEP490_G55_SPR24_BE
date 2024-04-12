using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using KitchenDelights.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenDelights.Test
{
    public class MenuControllerTest
    {
        private Mock<IMenuManager> _mockMenuManager;
        private IConfiguration _configuration;

        public MenuControllerTest()
        {
            _mockMenuManager = new Mock<IMenuManager>();
            _configuration = new ConfigurationBuilder().Build();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetAllMenus()
        {
            _mockMenuManager.Setup(x => x.GetAllMenues()).ReturnsAsync(new List<MenuDTO>(){
                new MenuDTO() {
                    MenuId = 1,
                    FeaturedImage = "mock-image-link",
                    MenuName = "mock-name",
                    MenuDescription = "mock-description",
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
                    ],
                },
                new MenuDTO() {
                    MenuId = 2,
                    FeaturedImage = "mock-image-link",
                    MenuName = "mock-name",
                    MenuDescription = "mock-description",
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
                    ],
                },
            });

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.GetAllMenu();

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetMenuByUserId()
        {
            _mockMenuManager.Setup(x => x.GetMenuByUserId(1)).ReturnsAsync(new List<MenuDTO>(){
                new MenuDTO() {
                    MenuId = 1,
                    FeaturedImage = "mock-image-link",
                    MenuName = "mock-name",
                    MenuDescription = "mock-description",
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
                    ],
                },
                new MenuDTO() {
                    MenuId = 2,
                    FeaturedImage = "mock-image-link",
                    MenuName = "mock-name",
                    MenuDescription = "mock-description",
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
                    ],
                },
            });

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.GetMenuByUserId(1);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus404_GetMenuByUserId()
        {
            _mockMenuManager.Setup(x => x.GetMenuByUserId(-1));

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.GetMenuByUserId(-1);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetMenuById()
        {
            _mockMenuManager.Setup(x => x.GetMenuById(1)).ReturnsAsync(new MenuDTO()
            {
                MenuId = 1,
                FeaturedImage = "mock-image-link",
                MenuName = "mock-name",
                MenuDescription = "mock-description",
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
                ],
            });

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.GetMenuById(1);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus404_GetMenuById()
        {
            _mockMenuManager.Setup(x => x.GetMenuById(-1));

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.GetMenuById(-1);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetMenuByUserIdAndCheckExistRecipe()
        {
            _mockMenuManager.Setup(x => x.GetMenuByUserIdAndCheckExistRecipe(1, 1)).ReturnsAsync(new List<MenuDTO>(){
                new MenuDTO() {
                    MenuId = 1,
                    FeaturedImage = "mock-image-link",
                    MenuName = "mock-name",
                    MenuDescription = "mock-description",
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
                    ],
                    isExistRecipe = true
                },
                new MenuDTO() {
                    MenuId = 2,
                    FeaturedImage = "mock-image-link",
                    MenuName = "mock-name",
                    MenuDescription = "mock-description",
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
                    ],
                    isExistRecipe = true
                },
            });

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.GetMenuByUserIdAndCheckExistRecipe(1, 1);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus404_GetMenuByUserIdAndCheckExistRecipe_UserNotExist()
        {
            _mockMenuManager.Setup(x => x.GetMenuByUserIdAndCheckExistRecipe(-1, 1));

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.GetMenuByUserIdAndCheckExistRecipe(-1, 1);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus404_GetMenuByUserIdAndCheckExistRecipe_RecipeNotExist()
        {
            _mockMenuManager.Setup(x => x.GetMenuByUserIdAndCheckExistRecipe(1, -1));

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.GetMenuByUserIdAndCheckExistRecipe(1, -1);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Create_ReturnStatus200_AllValid()
        {
            MenuRequestDTO toAdd = new MenuRequestDTO()
            {
                MenuId = 1,
                FeaturedImage = "mock-image-link",
                MenuName = "mock-name",
                MenuDescription = "mock-description",
                UserId = 1,
            };

            _mockMenuManager.Setup(x => x.CreateMenu(It.IsAny<MenuRequestDTO>()));

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.CreateMenu(toAdd);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void Update_ReturnStatus200_AllValid()
        {
            MenuRequestDTO toUpdate = new MenuRequestDTO()
            {
                MenuId = 1,
                FeaturedImage = "mock-image-link-update",
                MenuName = "mock-name-update",
                MenuDescription = "mock-description-update",
                UserId = 1,
            };
            _mockMenuManager.Setup(x => x.GetMenuById(1)).ReturnsAsync(new MenuDTO());
            _mockMenuManager.Setup(x => x.UpdateMenu(It.IsAny<MenuRequestDTO>())).ReturnsAsync(true);

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.UpdateMenu(toUpdate);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void Update_ReturnStatus404_NotFound()
        {
            MenuRequestDTO toUpdate = new MenuRequestDTO()
            {
                MenuId = -1,
                FeaturedImage = "mock-image-link-update",
                MenuName = "mock-name-update",
                MenuDescription = "mock-description-update",
                UserId = 1,
            };
            _mockMenuManager.Setup(x => x.UpdateMenu(It.IsAny<MenuRequestDTO>())).ReturnsAsync(true);

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.UpdateMenu(toUpdate);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Delete_ReturnStatus200_ExistMenu()
        {
            MenuRequestDTO toDelete = new MenuRequestDTO()
            {
                MenuId = 1,
                FeaturedImage = "mock-image-link-update",
                MenuName = "mock-name-update",
                MenuDescription = "mock-description-update",
                UserId = 1,
            };
            _mockMenuManager.Setup(x => x.GetMenuById(1)).ReturnsAsync(new MenuDTO());
            _mockMenuManager.Setup(x => x.DeleteMenu(toDelete.MenuId.Value)).ReturnsAsync(true);

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.DeleteMenu(toDelete.MenuId.Value);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void Delete_ReturnStatus404_NotExistAdvertisement()
        {
            _mockMenuManager.Setup(x => x.GetMenuById(-1)).ReturnsAsync(new MenuDTO());
            _mockMenuManager.Setup(x => x.DeleteMenu(-1)).ReturnsAsync(true);

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.DeleteMenu(-1);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Update_ReturnStatus200_AddRecipeToMenu() 
        { 
            _mockMenuManager.Setup(x => x.GetMenuById(1)).ReturnsAsync(new MenuDTO());
            _mockMenuManager.Setup(x => x.AddRecipeToMenu(1, 1)).ReturnsAsync(true);

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.AddRecipeToMenu(1, 1);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void Update_ReturnStatus404_AddRecipeToMenu_MenuNotExist()
        {
            _mockMenuManager.Setup(x => x.GetMenuById(-1)).ReturnsAsync(new MenuDTO());
            _mockMenuManager.Setup(x => x.AddRecipeToMenu(-1, 1)).ReturnsAsync(true);

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.AddRecipeToMenu(-1, 1);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Update_ReturnStatus404_AddRecipeToMenu_RecipeNotExist()
        {
            _mockMenuManager.Setup(x => x.GetMenuById(1)).ReturnsAsync(new MenuDTO());
            _mockMenuManager.Setup(x => x.AddRecipeToMenu(1, -1)).ReturnsAsync(true);

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.AddRecipeToMenu(1, -1);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Update_ReturnStatus200_RemoveRecipeFromMenu()
        {
            _mockMenuManager.Setup(x => x.GetMenuById(1)).ReturnsAsync(new MenuDTO());
            _mockMenuManager.Setup(x => x.RemoveRecipeFromMenu(1, 1)).ReturnsAsync(true);

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.RemoveRecipeFromMenu(1, 1);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void Update_ReturnStatus404_RemoveRecipeFromMenu_MenuNotExist()
        {
            _mockMenuManager.Setup(x => x.GetMenuById(-1)).ReturnsAsync(new MenuDTO());
            _mockMenuManager.Setup(x => x.RemoveRecipeFromMenu(-1, 1)).ReturnsAsync(true);

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.RemoveRecipeFromMenu(-1, 1);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Update_ReturnStatus404_RemoveRecipeFromMenu_RecipeNotExist()
        {
            _mockMenuManager.Setup(x => x.GetMenuById(1)).ReturnsAsync(new MenuDTO());
            _mockMenuManager.Setup(x => x.RemoveRecipeFromMenu(1, -1)).ReturnsAsync(true);

            MenuController _controller = new(_configuration, _mockMenuManager.Object);
            var result = await _controller.RemoveRecipeFromMenu(1, -1);

            result.Should().BeNotFoundObjectResult();
        }
    }
}
