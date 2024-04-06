using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Business.Profiles;
using Data.Entity;
using Data.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Test
{
    public class CategoryManagerTest
    {
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly IMapper _mapper;

        public CategoryManagerTest()
        {
            //Initial setup
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<CategoryProfile>();
            }));
        }

        [Fact]
        public async void GetAllCategories_GetAllCategories()
        {
            var categories = CategoriesSample();
            _categoryRepositoryMock.Setup(x => x.GetAllCategories()).ReturnsAsync(categories.ToList());

            ICategoryManager _categoryManager = new CategoryManager(_categoryRepositoryMock.Object, _mapper);
            var result = await _categoryManager.GetAllCategories(1);

            result.Should().BeOfType<List<CategoryDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(4);
        }

        [Fact]
        public async void GetCategory_GetCategoryByParentId_ExistInRepo()
        {
            //Arrange
            var categories = CategoriesSample();
            List<CategoryDTO> categoryDTOs = [];
            categoryDTOs.AddRange(categories.Select(_mapper.Map<Category, CategoryDTO>));
            _categoryRepositoryMock.Setup(x => x.GetCategoryByParentId(1)).ReturnsAsync(categories.Where(x => x.ParentId == 1).ToList()); //Mock Advertisement repository GetAdvertisementById(int id) method

            ICategoryManager _categoryManager = new CategoryManager(_categoryRepositoryMock.Object, _mapper);
            var result = await _categoryManager.GetCategoryByParentId(1, true);

            result.Should().BeOfType<List<CategoryDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(1);
        }

        [Fact]
        public async void GetCategory_GetCategoryByCategoryId_ExistInRepo()
        {
            //Arrange
            var categories = CategoriesSample();
            List<CategoryDTO> categoryDTOs = [];
            categoryDTOs.AddRange(categories.Select(_mapper.Map<Category, CategoryDTO>));
            _categoryRepositoryMock.Setup(x => x.GetCategoryById(1)).ReturnsAsync(categories.Find(x => x.CategoryId == 1)); //Mock Advertisement repository GetAdvertisementById(int id) method

            //Act
            ICategoryManager _categoryManager = new CategoryManager(_categoryRepositoryMock.Object, _mapper);
            var result = await _categoryManager.GetCategoryById(1);
            var actual = categoryDTOs.Find(x => x.CategoryId == 1);

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<CategoryDTO>().And.BeEquivalentTo(actual!);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetCategory_GetCategoryByCategoryId_NotExistInRepo()
        {
            var categories = CategoriesSample();
            _categoryRepositoryMock.Setup(x => x.GetCategoryById(-1)).ReturnsAsync(categories.FirstOrDefault(x => x.CategoryId == -1));

            ICategoryManager _categoryManager = new CategoryManager(_categoryRepositoryMock.Object, _mapper);
            var result = await _categoryManager.GetCategoryById(-1);
            var actual = categories.FirstOrDefault(x => x.CategoryId == -1);

            result.Should().BeNull();
            actual.Should().BeNull();
        }

        [Fact]
        public async void CreateCategory_CreateWithCategoryDTO_CategoryNotExistInRepo()
        {
            var categories = CategoriesSample();
            CategoryDTO categoryDTO = new()
            {
                CategoryId = 5,
                ParentId  = 0,
                ParentName = "string",
                CategoryName = "Hải Sản",
                CategoryType = true,
                isExistRecipe = true
            };
            _categoryRepositoryMock.Setup(x => x.CreateCategory(It.IsAny<Category>())).Callback<Category>(categories.Add);
            _categoryRepositoryMock.Setup(x => x.GetCategoryById(5)).ReturnsAsync(categories.FirstOrDefault(x => x.CategoryId == 5));

            ICategoryManager _categoryManager = new CategoryManager(_categoryRepositoryMock.Object, _mapper);
            var boolResult = await _categoryManager.CreateCategory(categoryDTO);
            var countResult = categories.Count;

            boolResult.Should().BeTrue();
            countResult.Should().Be(5);
        }

        [Fact]
        public async void UpdateCategory_UpdateCategory_CategoryExistInRepo()
        {
            var categories = CategoriesSample();
            CategoryDTO categoryDTO = new()
            {
                CategoryId = 1,
                ParentId = 2,
                ParentName = "string",
                CategoryName = "Hải Sản",
                CategoryType = false,
                isExistRecipe = true
            };
            _categoryRepositoryMock.Setup(x => x.GetCategoryById(1)).ReturnsAsync(categories.FirstOrDefault(x => x.CategoryId == 1));
            _categoryRepositoryMock.Setup(x => x.UpdateCategory(It.IsAny<Category>())).Callback<Category>((category) => categories[0] = category);

            ICategoryManager _categoryManager = new CategoryManager(_categoryRepositoryMock.Object, _mapper);
            var boolResult = await _categoryManager.UpdateCategory(categoryDTO);
            var updatedCategory = categories.FirstOrDefault(x => x.CategoryId == 1);

            boolResult.Should().BeTrue();
            updatedCategory.Should().NotBeNull();
            updatedCategory!.ParentId.Should().Be(categoryDTO.ParentId);
            updatedCategory!.CategoryName.Should().BeSameAs(categoryDTO.CategoryName);
            updatedCategory!.CategoryType.Should().Be(categoryDTO.CategoryType);
        }

        [Fact]
        public async void UpdateCategory_UpdateCategory_CategoryNotExistInRepo()
        {
            var categories = CategoriesSample();
            CategoryDTO categoryDTO = new()
            {
                CategoryId = 5,
                ParentId = 2,
                ParentName = "string",
                CategoryName = "Hải Sản",
                CategoryType = false,
                isExistRecipe = true
            };
            _categoryRepositoryMock.Setup(x => x.GetCategoryById(5)).ReturnsAsync(categories.FirstOrDefault(x => x.CategoryId == 5));
            _categoryRepositoryMock.Setup(x => x.UpdateCategory(It.IsAny<Category>())).Callback<Category>((category) => categories[3] = category);

            ICategoryManager _categoryManager = new CategoryManager(_categoryRepositoryMock.Object, _mapper);
            var boolResult = await _categoryManager.UpdateCategory(categoryDTO);
            var updatedCategory = categories.FirstOrDefault(x => x.CategoryId == 5);

            boolResult.Should().BeFalse();
            updatedCategory.Should().BeNull();
        }

        [Fact]
        public async void DeleteACategory_DeleteCategory_CategoryExistInRepo()
        {
            var categories = CategoriesSample();
            CategoryDTO categoryDTO = new()
            {
                CategoryId = 2,
            };
            _categoryRepositoryMock.Setup(x => x.GetCategoryById(2)).ReturnsAsync(categories.FirstOrDefault(x => x.CategoryId == 2));
            _categoryRepositoryMock.Setup(x => x.DeleteCategory(It.IsAny<Category>())).Callback<Category>(item => categories.Remove(item));

            ICategoryManager _categoryManager = new CategoryManager(_categoryRepositoryMock.Object, _mapper);
            var boolResult = await _categoryManager.DeleteCategory(categoryDTO.CategoryId.Value);
            var actual = categories.ToList();

            boolResult.Should().BeTrue();
            actual.Count.Should().Be(3);
        }

        [Fact]
        public async void DeleteACategory_DeleteCategory_CategoryNotExistInRepo()
        {
            var categories = CategoriesSample();
            CategoryDTO categoryDTO = new()
            {
                CategoryId = 5,
            };
            _categoryRepositoryMock.Setup(x => x.GetCategoryById(5)).ReturnsAsync(categories.FirstOrDefault(x => x.CategoryId == 5));
            _categoryRepositoryMock.Setup(x => x.DeleteCategory(It.IsAny<Category>())).Callback<Category>(item => categories.Remove(item));

            ICategoryManager _categoryManager = new CategoryManager(_categoryRepositoryMock.Object, _mapper);
            var boolResult = await _categoryManager.DeleteCategory(categoryDTO.CategoryId.Value);
            var actual = categories.ToList();

            boolResult.Should().BeFalse();
            actual.Count.Should().Be(4);
        }

        private static List<Category> CategoriesSample()
        {
            List<Category> output = [
                new Category()
                {
                    CategoryId = 1,
                    ParentId = null,
                    CategoryName = "Thịt",
                    CategoryType = true,
                    CategoryStatus = 1,
                    Parent = null
                },
                new Category()
                {
                    CategoryId = 2,
                    ParentId = null,
                    CategoryName = "Rau",
                    CategoryType = true,
                    CategoryStatus = 1,
                    Parent = null
                },
                new Category()
                {
                    CategoryId = 3,
                    ParentId = 1,
                    CategoryName = "Thịt bò",
                    CategoryType = true,
                    CategoryStatus = 1,
                    Parent = new Category()
                    {
                        CategoryId = 1,
                        ParentId = null,
                        CategoryName = "Thịt",
                        CategoryType = true,
                        CategoryStatus = 1,
                        Parent = null
                    }
                },
                new Category()
                {
                    CategoryId = 4,
                    ParentId = 2,
                    CategoryName = "Rau muống",
                    CategoryType = true,
                    CategoryStatus = 1,
                    Parent = new Category()
                    {
                        CategoryId = 2,
                        ParentId = null,
                        CategoryName = "Rau",
                        CategoryType = true,
                        CategoryStatus = 1,
                        Parent = null
                    }
                },
            ];
            return output;
        }
    }
}
