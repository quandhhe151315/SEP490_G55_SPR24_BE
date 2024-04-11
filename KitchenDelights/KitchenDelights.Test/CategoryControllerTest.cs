using Business.DTO;
using Business.Interfaces;
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
    public class CategoryControllerTest
    {
        private Mock<ICategoryManager> _mockCategoryManager;
        private IConfiguration _configuration;

        public CategoryControllerTest()
        {
            _mockCategoryManager = new Mock<ICategoryManager>();
            _configuration = new ConfigurationBuilder().Build();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetCategory()
        {
            _mockCategoryManager.Setup(x => x.GetAllCategories(1)).ReturnsAsync(new List<CategoryDTO>(){
                new CategoryDTO() {
                    CategoryId = 1,
                    ParentId = null,
                    ParentName = null,
                    CategoryName = "Thịt",
                    CategoryType = true
                },
                new CategoryDTO() {
                    CategoryId = 2,
                    ParentId = 1,
                    ParentName = "Thịt",
                    CategoryName = "Thịt bò",
                    CategoryType = true
                },
                new CategoryDTO() {
                    CategoryId = 3,
                    ParentId = null,
                    ParentName = null,
                    CategoryName = "Hải Sản",
                    CategoryType = false
                },
                new CategoryDTO() {
                    CategoryId = 4,
                    ParentId = 3,
                    ParentName = "Hải Sản",
                    CategoryName = "Cá",
                    CategoryType = false
                },
            });

            CategoryController _controller = new(_configuration, _mockCategoryManager.Object);
            var result = await _controller.GetAllCategoy(1);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetCategoryById()
        {
            _mockCategoryManager.Setup(x => x.GetCategoryById(1)).ReturnsAsync(new CategoryDTO()
            {
                CategoryId = 1,
                ParentId = null,
                ParentName = null,
                CategoryName = "Thịt",
                CategoryType = true
            });

            CategoryController _controller = new (_configuration, _mockCategoryManager.Object);
            var result = await _controller.GetCategoryById(1);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus404_GetCategoryById()
        {
            _mockCategoryManager.Setup(x => x.GetCategoryById(0));

            CategoryController _controller = new (_configuration, _mockCategoryManager.Object);
            var result = await _controller.GetCategoryById(0);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetCategoryByParentId()
        {
            _mockCategoryManager.Setup(x => x.GetCategoryByParentId(1, true)).ReturnsAsync(new List<CategoryDTO>(){
                new CategoryDTO() {
                    CategoryId = 1,
                    ParentId = null,
                    ParentName = null,
                    CategoryName = "Thịt",
                    CategoryType = true
                },
                new CategoryDTO() {
                    CategoryId = 2,
                    ParentId = 1,
                    ParentName = "Thịt",
                    CategoryName = "Thịt bò",
                    CategoryType = true
                },
                new CategoryDTO() {
                    CategoryId = 3,
                    ParentId = null,
                    ParentName = null,
                    CategoryName = "Hải Sản",
                    CategoryType = false
                },
                new CategoryDTO() {
                    CategoryId = 4,
                    ParentId = 3,
                    ParentName = "Hải Sản",
                    CategoryName = "Cá",
                    CategoryType = false
                },
            });

            CategoryController _controller = new(_configuration, _mockCategoryManager.Object);
            var result = await _controller.GetCategoryByParentId(1, true);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetAllParentCategory()
        {
            _mockCategoryManager.Setup(x => x.GetCategoryByParentId(null, true)).ReturnsAsync(new List<CategoryDTO>(){
                new CategoryDTO() {
                    CategoryId = 1,
                    ParentId = null,
                    ParentName = null,
                    CategoryName = "Thịt",
                    CategoryType = true
                },
                new CategoryDTO() {
                    CategoryId = 2,
                    ParentId = 1,
                    ParentName = "Thịt",
                    CategoryName = "Thịt bò",
                    CategoryType = true
                },
                new CategoryDTO() {
                    CategoryId = 3,
                    ParentId = null,
                    ParentName = null,
                    CategoryName = "Hải Sản",
                    CategoryType = false
                },
                new CategoryDTO() {
                    CategoryId = 4,
                    ParentId = 3,
                    ParentName = "Hải Sản",
                    CategoryName = "Cá",
                    CategoryType = false
                },
            });

            CategoryController _controller = new(_configuration, _mockCategoryManager.Object);
            var result = await _controller.GetCategoryByParentId(null, true);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void Create_ReturnStatus200_AllValid()
        {
            CategoryDTO toAdd = new CategoryDTO()
            {
                CategoryId = 1,
                ParentId = null,
                ParentName = null,
                CategoryName = "category-name-update",
                CategoryType = true
            };
            _mockCategoryManager.Setup(x => x.CreateCategory(It.IsAny<CategoryDTO>())).ReturnsAsync(true);

            CategoryController _controller = new(_configuration, _mockCategoryManager.Object);
            var result = await _controller.CreateCategory(toAdd);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void Update_ReturnStatus200_AllValid()
        {
            CategoryDTO toUpdate = new CategoryDTO()
            {
                CategoryId = 1,
                ParentId = null,
                ParentName = null,
                CategoryName = "category-name-update",
                CategoryType = true
            };
            _mockCategoryManager.Setup(x => x.GetCategoryById(1)).ReturnsAsync(new CategoryDTO());
            _mockCategoryManager.Setup(x => x.UpdateCategory(It.IsAny<CategoryDTO>())).ReturnsAsync(true);

            CategoryController _controller = new(_configuration, _mockCategoryManager.Object);
            var result = await _controller.UpdateCategory(toUpdate);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void Update_ReturnStatus404_NotFound()
        {
            CategoryDTO toUpdate = new CategoryDTO()
            {
                CategoryId = -1,
                ParentId = null,
                ParentName = null,
                CategoryName = "category-name-update",
                CategoryType = true
            };
            _mockCategoryManager.Setup(x => x.UpdateCategory(It.IsAny<CategoryDTO>())).ReturnsAsync(false);

            CategoryController _controller = new(_configuration, _mockCategoryManager.Object);
            var result = await _controller.UpdateCategory(toUpdate);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Delete_ReturnStatus200_ExistCategory()
        {
            CategoryDTO toDelete = new CategoryDTO()
            {
                CategoryId = 1,
                ParentId = null,
                ParentName = null,
                CategoryName = "category-name-update",
                CategoryType = true
            };
            _mockCategoryManager.Setup(x => x.GetCategoryById(1)).ReturnsAsync(new CategoryDTO());
            _mockCategoryManager.Setup(x => x.UpdateCategory(It.IsAny<CategoryDTO>())).ReturnsAsync(true);

            CategoryController _controller = new(_configuration, _mockCategoryManager.Object);
            var result = await _controller.DeleteCategory(toDelete.CategoryId.Value);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void Delete_ReturnStatus404_NotExistCategory()
        {
            CategoryDTO toDelete = new CategoryDTO()
            {
                CategoryId = -1,
                ParentId = null,
                ParentName = null,
                CategoryName = "category-name-update",
                CategoryType = true
            };
            _mockCategoryManager.Setup(x => x.DeleteCategory(toDelete.CategoryId.Value)).ReturnsAsync(true);

            CategoryController _controller = new(_configuration, _mockCategoryManager.Object);
            var result = await _controller.DeleteCategory(toDelete.CategoryId.Value);

            result.Should().BeNotFoundObjectResult();
        }
    }
}
