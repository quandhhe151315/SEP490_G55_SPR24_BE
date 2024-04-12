using Business.DTO;
using Business.Interfaces;
using Data.Entity;
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
    public class RecipeControllerTest
    {
        private Mock<IRecipeManager> _mockRecipeManager;
        private IConfiguration _configuration;

        public RecipeControllerTest()
        {
            _mockRecipeManager = new Mock<IRecipeManager>();
            _configuration = new ConfigurationBuilder().Build();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetAllRecipe()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe(null, null, null, null, null, null,null)).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                },
                new RecipeDTO()
                {
                    RecipeId = 2,
                },
                new RecipeDTO()
                {
                    RecipeId = 3,
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe(null, null, null, null, null, null, null);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetRecipeBySearchString()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe("Cơm", null, null, null, null, null, null)).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm rang"
                },
                new RecipeDTO()
                {
                    RecipeId = 2,
                    RecipeTitle = "Cơm thịt nguội"
                },
                new RecipeDTO()
                {
                    RecipeId = 3,
                    RecipeTitle = "Cơm gia đình"
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe("Cơm", null, null, null, null, null, null);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetRecipeByCategory()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe(null, 1, null, null, null, null, null)).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm rang",
                    Categories = [
                        new CategoryDTO()
                        {
                            CategoryId = 1
                        },
                        new CategoryDTO()
                        {
                            CategoryId = 2
                        },
                    ]
                },
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm thịt nguội",
                    Categories = [
                        new CategoryDTO()
                        {
                            CategoryId = 1
                        },
                        new CategoryDTO()
                        {
                            CategoryId = 2
                        },
                    ]
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe(null, 1, null, null, null, null, null);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetRecipeByCountry()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe(null, null, 1, null, null, null, null)).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm rang",
                    Categories = [
                        new CategoryDTO()
                        {
                            CategoryId = 1
                        },
                        new CategoryDTO()
                        {
                            CategoryId = 2
                        },
                    ],
                    Countries = [
                        new CountryDTO()
                        {
                            CountryId = 1
                        }
                    ]
                },
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm thịt nguội",
                    Categories = [
                        new CategoryDTO()
                        {
                            CategoryId = 1
                        },
                        new CategoryDTO()
                        {
                            CategoryId = 2
                        },
                    ],
                    Countries = [
                        new CountryDTO()
                        {
                            CountryId = 1
                        }
                    ]
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe(null, null, 1, null, null, null, null);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetRecipeByIngredient()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe(null, null, null, 1, null, null, null)).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm rang",
                    Categories = [
                        new CategoryDTO()
                        {
                            CategoryId = 1
                        },
                        new CategoryDTO()
                        {
                            CategoryId = 2
                        },
                    ],
                    Countries = [
                        new CountryDTO()
                        {
                            CountryId = 1
                        }
                    ],
                    RecipeIngredients = [
                        new RecipeIngredientDTO()
                        {
                            IngredientId = 1
                        },
                        new RecipeIngredientDTO()
                        {
                            IngredientId = 2
                        },
                    ]
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe(null, null, null, 1, null, null, null);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetRecipeFree()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe(null, null, null, null, 1, null, null)).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm rang",
                    IsFree = true
                },
                new RecipeDTO()
                {
                    RecipeId = 2,
                    RecipeTitle = "Cơm thịt nguội",
                    IsFree = true
                },
                new RecipeDTO()
                {
                    RecipeId = 3,
                    RecipeTitle = "Cơm gia đình",
                    IsFree = true
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe(null, null, null, null, 1, null, null);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetRecipePaid()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe(null, null, null, null, 2, null, null)).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm rang",
                    IsFree = false
                },
                new RecipeDTO()
                {
                    RecipeId = 2,
                    RecipeTitle = "Cơm thịt nguội",
                    IsFree = false
                },
                new RecipeDTO()
                {
                    RecipeId = 3,
                    RecipeTitle = "Cơm gia đình",
                    IsFree = false
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe(null, null, null, null, 2, null, null);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetRecipeOrderByCreateDate()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe(null, null, null, null, null, "CreateDate", null)).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm rang",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                },
                new RecipeDTO()
                {
                    RecipeId = 2,
                    RecipeTitle = "Cơm thịt nguội",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                },
                new RecipeDTO()
                {
                    RecipeId = 3,
                    RecipeTitle = "Cơm gia đình",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe(null, null, null, null, null, "CreateDate", null);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetRecipeOrderByTitle()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe(null, null, null, null, null, "Title", null)).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm rang",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                },
                new RecipeDTO()
                {
                    RecipeId = 2,
                    RecipeTitle = "Cơm thịt nguội",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                },
                new RecipeDTO()
                {
                    RecipeId = 3,
                    RecipeTitle = "Cơm gia đình",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe(null, null, null, null, null, "Title", null);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetRecipeOrderByPrice()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe(null, null, null, null, null, "Price", null)).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm rang",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                    RecipePrice = 10000
                },
                new RecipeDTO()
                {
                    RecipeId = 2,
                    RecipeTitle = "Cơm thịt nguội",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                    RecipePrice = 30000
                },
                new RecipeDTO()
                {
                    RecipeId = 3,
                    RecipeTitle = "Cơm gia đình",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                    RecipePrice = 15000
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe(null, null, null, null, null, "Price", null);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetRecipeOrderByRating()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe(null, null, null, null, null, "Rating", null)).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm rang",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                    RecipePrice = 10000,
                    RecipeRating = 5
                },
                new RecipeDTO()
                {
                    RecipeId = 2,
                    RecipeTitle = "Cơm thịt nguội",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                    RecipePrice = 30000,
                    RecipeRating = 3
                },
                new RecipeDTO()
                {
                    RecipeId = 3,
                    RecipeTitle = "Cơm gia đình",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                    RecipePrice = 15000,
                    RecipeRating = 4
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe(null, null, null, null, null, "Rating", null);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetRecipeDESC()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe(null, null, null, null, null, null, "DESC")).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm rang",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                    RecipePrice = 10000,
                    RecipeRating = 5
                },
                new RecipeDTO()
                {
                    RecipeId = 2,
                    RecipeTitle = "Cơm thịt nguội",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                    RecipePrice = 30000,
                    RecipeRating = 3
                },
                new RecipeDTO()
                {
                    RecipeId = 3,
                    RecipeTitle = "Cơm gia đình",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                    RecipePrice = 15000,
                    RecipeRating = 4
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe(null, null, null, null, null, null, "DESC");

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetRecipeASC()
        {
            _mockRecipeManager.Setup(x => x.FilterRecipe(null, null, null, null, null, null, "ASC")).ReturnsAsync(new List<RecipeDTO>(){
                new RecipeDTO() {
                    RecipeId = 1,
                    RecipeTitle = "Cơm rang",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                    RecipePrice = 10000,
                    RecipeRating = 5
                },
                new RecipeDTO()
                {
                    RecipeId = 2,
                    RecipeTitle = "Cơm thịt nguội",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                    RecipePrice = 30000,
                    RecipeRating = 3
                },
                new RecipeDTO()
                {
                    RecipeId = 3,
                    RecipeTitle = "Cơm gia đình",
                    IsFree = false,
                    CreateDate = DateTime.Now ,
                    RecipePrice = 15000,
                    RecipeRating = 4
                },
            });

            RecipeController _controller = new(_configuration, _mockRecipeManager.Object);
            var result = await _controller.GetAllRecipe(null, null, null, null, null, null, "ASC");

            result.Should().BeOkObjectResult();
        }
    }
}
