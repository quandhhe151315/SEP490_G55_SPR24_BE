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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Business.Test
{
    public class RecipeManagerTest
    {
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<ICountryRepository> _countryRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRecipeIngredientRepository> _recipeIngredientRepositoryMock;
        private readonly Mock<IIngredientRepository> _ingredientRepositoryMock;
        private readonly Mock<IHistoryRepository> _historyRepositoryMock;

        private readonly IMapper _mapper;

        public RecipeManagerTest()
        {
            //Initial setup
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _countryRepositoryMock = new Mock<ICountryRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _recipeIngredientRepositoryMock = new Mock<IRecipeIngredientRepository>();
            _ingredientRepositoryMock = new Mock<IIngredientRepository>();
            _historyRepositoryMock = new Mock<IHistoryRepository>();

            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<RecipeProfile>();
                options.AddProfile<IngredientProfile>();
                options.AddProfile<CategoryProfile>();
                options.AddProfile<CountryProfile>();
            }));
        }

        [Fact]
        public async void GetRecipe_GetRecipeList_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper
                                                            );
            var result = await _recipeManager.GetRecipes();

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(2);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetRecipe_GetRecipeById_RecipeExistInRepo()
        {
            //Arrange
            var recipes = RecipesSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(1)).ReturnsAsync(recipes.Find(x => x.RecipeId == 1));

            //Act
            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.GetRecipe(1);
            var actual = recipeDTOs.Find(x => x.RecipeId == 1);

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<RecipeDTO>().And.BeEquivalentTo(actual!);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetRecipe_GetRecipeById_RecipeNotExistInRepo()
        {
            var recipes = RecipesSample();
            _recipeRepositoryMock.Setup(x => x.GetRecipe(-1)).ReturnsAsync(recipes.Find(x => x.RecipeId == -1));

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.GetRecipe(-1);
            var actual = recipes.FirstOrDefault(x => x.RecipeId == -1);

            result.Should().BeNull();
            actual.Should().BeNull();
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetRecipe_GetRecipeByUserId_RecipeExistInRepo()
        {
            //Arrange
            var recipes = RecipesSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipeByUserId(1)).ReturnsAsync(recipes.Where(x => x.UserId == 1).ToList());

            //Act
            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.GetRecipeByUserId(1);
            var actual = recipeDTOs.Where(x => x.UserId == 1).ToList();

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<List<RecipeDTO>>().And.BeEquivalentTo(actual!);
            result.Count.Should().Be(2);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetRecipe_GetRecipeByUserId_RecipeNotExistInRepo()
        {
            var recipes = RecipesSample();
            _recipeRepositoryMock.Setup(x => x.GetRecipeByUserId(-1)).ReturnsAsync(recipes.Where(x => x.UserId == -1).ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.GetRecipeByUserId(-1);
            var actual = recipes.Where(x => x.UserId == -1).ToList();

            result.Should().BeNullOrEmpty();
            actual.Should().BeNullOrEmpty();
            result.Count.Should().Be(0);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetRecipe_GetRecipeByCategory_RecipeExistInRepo()
        {
            //Arrange
            var recipes = RecipesSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipeByCategory(1)).ReturnsAsync(recipes.Where(x => x.Categories.Any(x => x.CategoryId == 1)).ToList());

            //Act
            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.GetRecipeByCategory(1);
            var actual = recipeDTOs.Where(x => x.Categories.Any(x => x.CategoryId == 1)).ToList();

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<List<RecipeDTO>>().And.BeEquivalentTo(actual!);
            result.Count.Should().Be(1);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetRecipe_GetRecipeByCategory_RecipeNotExistInRepo()
        {
            var recipes = RecipesSample();
            _recipeRepositoryMock.Setup(x => x.GetRecipeByCategory(-1)).ReturnsAsync(recipes.Where(x => x.Categories.Any(x => x.CategoryId == -1)).ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.GetRecipeByCategory(-1);
            var actual = recipes.Where(x => x.Categories.Any(x => x.CategoryId == -1)).ToList();

            result.Should().BeNullOrEmpty();
            actual.Should().BeNullOrEmpty();
            result.Count.Should().Be(0);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetRecipe_GetRecipeByCountry_RecipeExistInRepo()
        {
            //Arrange
            var recipes = RecipesSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipeByCountry(1)).ReturnsAsync(recipes.Where(x => x.Countries.Any(x => x.CountryId == 1)).ToList());

            //Act
            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.GetRecipeByCountry(1);
            var actual = recipeDTOs.Where(x => x.Countries.Any(x => x.CountryId == 1)).ToList();

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<List<RecipeDTO>>().And.BeEquivalentTo(actual!);
            result.Count.Should().Be(1);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetRecipe_GetRecipeByCountry_RecipeNotExistInRepo()
        {
            var recipes = RecipesSample();
            _recipeRepositoryMock.Setup(x => x.GetRecipeByCountry(-1)).ReturnsAsync(recipes.Where(x => x.Countries.Any(x => x.CountryId == -1)).ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.GetRecipeByCountry(-1);
            var actual = recipes.Where(x => x.Countries.Any(x => x.CountryId == -1)).ToList();

            result.Should().BeNullOrEmpty();
            actual.Should().BeNullOrEmpty();
            result.Count.Should().Be(0);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetRecipe_GetRecipeByTitle_RecipeExistInRepo()
        {
            //Arrange
            var recipes = RecipesSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipeByTitle("title")).ReturnsAsync(recipes.Where(x => x.RecipeTitle!.Contains("title")).ToList());

            //Act
            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.GetRecipeByTitle("title");
            var actual = recipeDTOs.Where(x => x.RecipeTitle!.Contains("title")).ToList();

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<List<RecipeDTO>>().And.BeEquivalentTo(actual!);
            result.Count.Should().Be(2);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetRecipe_GetRecipeByTitle_RecipeNotExistInRepo()
        {
            var recipes = RecipesSample();
            _recipeRepositoryMock.Setup(x => x.GetRecipeByTitle("abcde")).ReturnsAsync(recipes.Where(x => x.RecipeTitle!.Contains("abcde")).ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.GetRecipeByTitle("abcde");
            var actual = recipes.Where(x => x.RecipeTitle!.Contains("abcde")).ToList();

            result.Should().BeNullOrEmpty();
            actual.Should().BeNullOrEmpty();
            result.Count.Should().Be(0);
        }

        [Fact]
        public async void GetRecipe_FilterRecipe_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe(null,null,null,null,null,null,null);

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(2);
        }

        [Fact]
        public async void GetRecipe_FilterRecipeSearchString_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe("title", null, null, null, null, null, null);

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(2);
        }

        [Fact]
        public async void GetRecipe_FilterRecipeSearchStringCategory_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe("title", 1, null, null, null, null, null);

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(1);
        }

        [Fact]
        public async void GetRecipe_FilterRecipeCategoryCountry_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe(null, 1, 1, null, null, null, null);

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(1);
        }

        [Fact]
        public async void GetRecipe_FilterRecipeIngredient_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe(null, null, null, 1, 1, null, null);

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(1);
        }

        [Fact]
        public async void GetRecipe_FilterRecipeFree_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe(null, null, null, null, 1, null, null);

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(1);
        }

        [Fact]
        public async void GetRecipe_FilterRecipePaid_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe(null, null, null, null, 2, null, null);

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(1);
        }

        [Fact]
        public async void GetRecipe_FilterRecipeOrderByCreateDate_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe(null, null, null, null, null, "CreateDate", null);

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(2);
        }

        public async void GetRecipe_FilterRecipeOrderByTitle_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe(null, null, null, null, null, "Title", null);

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(2);
        }

        [Fact]
        public async void GetRecipe_FilterRecipeOrderByPrice_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe(null, null, null, null, null, "Price", null);

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(2);
        }

        [Fact]
        public async void GetRecipe_FilterRecipeOrderByRating_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe(null, null, null, null, null, "Rating", null);

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(2);
        }

        [Fact]
        public async void GetRecipe_FilterRecipedDESC_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe(null, null, null, null, null, null, "DESC");

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(2);
        }

        [Fact]
        public async void GetRecipe_FilterRecipedASC_ExistInRepo()
        {
            var recipes = RecipesSample();
            var countries = CountriesSample();
            var ingredients = IngredientsSample();
            List<RecipeDTO> recipeDTOs = [];
            recipeDTOs.AddRange(recipes.Select(_mapper.Map<Recipe, RecipeDTO>));
            _recipeRepositoryMock.Setup(x => x.GetRecipes()).ReturnsAsync(recipes.ToList());

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var result = await _recipeManager.FilterRecipe(null, null, null, null, null, null, "ASC");

            result.Should().BeOfType<List<RecipeDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(2);
        }

        [Fact]
        public async void CreateAdvertisement_CreateWithAdvertisementDTO_AdvertisementNotExistInRepo()
        {
            var recipes = RecipesSample();
            var ingredients = IngredientsSample();
            var countries = CountriesSample();
            RecipeRequestDTO recipeRequestDTO = new()
            {
                RecipeId = 3,
                UserId = 1,
                FeaturedImage = "mock-image-link-update",
                RecipeDescription = "mock-description-update",
                VideoLink = "mock-video-link-update",
                RecipeTitle = "mock-title-update",
                PreparationTime = 10,
                CookTime = 10,
                RecipeServe = 4,
                RecipeContent = "mock-content-update",
                RecipeStatus = 1,
                IsFree = true,
                RecipePrice = 0,
                CreateDate = DateTime.Now,
                CountryId = 1,
                RecipeIngredients = [
                    new RecipeIngredientRequestDTO()
                    {
                        IngredientId = 1,
                        UnitValue = 10
                    },
                    new RecipeIngredientRequestDTO()
                    {
                        IngredientId = 2,
                        UnitValue = 15
                    },
                ]
            };
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeRequestDTO.RecipeId.Value)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeRequestDTO.RecipeId.Value));
            _recipeRepositoryMock.Setup(x => x.CreateRecipe(It.IsAny<Recipe>())).Callback<Recipe>(recipes.Add);
            _countryRepositoryMock.Setup(x => x.GetCountry(1)).ReturnsAsync(countries.FirstOrDefault(x => x.CountryId == 1));
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(1)).ReturnsAsync(ingredients.FirstOrDefault(x => x.IngredientId == 1));
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(2)).ReturnsAsync(ingredients.FirstOrDefault(x => x.IngredientId == 2));

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            bool boolResult = false;
            try
            {
                await _recipeManager.CreateRecipe(recipeRequestDTO);
                boolResult = true;
            }
            catch (Exception ex)
            {
                boolResult = false;
            }
            var countResult = recipes.Count;

            boolResult.Should().BeTrue();
            countResult.Should().Be(3);
        }

        [Fact]
        public async void UpdateRecipe_UpdateRecipe_RecipeExistInRepo()
        {
            var recipes = RecipesSample();
            var ingredients = IngredientsSample();
            var countries = CountriesSample();
            RecipeRequestDTO recipeRequestDTO = new()
            {
                RecipeId = 1,
                UserId = 1,
                FeaturedImage = "mock-image-link-update",
                RecipeDescription = "mock-description-update",
                VideoLink = "mock-video-link-update",
                RecipeTitle = "mock-title-update",
                PreparationTime = 10,
                CookTime = 10,
                RecipeServe = 4,
                RecipeContent = "mock-content-update",
                RecipeStatus = 1,
                IsFree = true,
                RecipePrice = 0,
                CreateDate = DateTime.Now,
                CountryId = 1,
                RecipeIngredients = [
                    new RecipeIngredientRequestDTO()
                    {
                        IngredientId = 1,
                        UnitValue = 10
                    },
                    new RecipeIngredientRequestDTO()
                    {
                        IngredientId = 2,
                        UnitValue = 15
                    },
                ]
            };
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeRequestDTO.RecipeId.Value)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeRequestDTO.RecipeId.Value));
            _recipeRepositoryMock.Setup(x => x.UpdateRecipe(It.IsAny<Recipe>())).Callback<Recipe>((recipe) => recipes[0] = recipe);
            _countryRepositoryMock.Setup(x => x.GetCountry(1)).ReturnsAsync(countries.FirstOrDefault(x => x.CountryId == 1));
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(1)).ReturnsAsync(ingredients.FirstOrDefault(x => x.IngredientId == 1));
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(2)).ReturnsAsync(ingredients.FirstOrDefault(x => x.IngredientId == 2));

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var boolResult = await _recipeManager.UpdateRecipe(recipeRequestDTO);
            var updatedRecipe = recipes.FirstOrDefault(x => x.RecipeId == 1);

            boolResult.Should().BeTrue();
            updatedRecipe.Should().NotBeNull();
            updatedRecipe!.FeaturedImage.Should().BeSameAs(recipeRequestDTO.FeaturedImage);
            updatedRecipe!.RecipeDescription.Should().BeSameAs(recipeRequestDTO.RecipeDescription);
            updatedRecipe!.VideoLink.Should().BeSameAs(recipeRequestDTO.VideoLink);
            updatedRecipe!.RecipeTitle.Should().BeSameAs(recipeRequestDTO.RecipeTitle);
            updatedRecipe!.PreparationTime.Should().Be(recipeRequestDTO.PreparationTime);
            updatedRecipe!.CookTime.Should().Be(recipeRequestDTO.CookTime);
            updatedRecipe!.RecipeServe.Should().Be(recipeRequestDTO.RecipeServe);
            updatedRecipe!.RecipeContent.Should().BeSameAs(recipeRequestDTO.RecipeContent);
            updatedRecipe!.RecipeStatus.Should().Be(recipeRequestDTO.RecipeStatus);
            updatedRecipe!.IsFree.Should().Be(recipeRequestDTO.IsFree);
            updatedRecipe!.RecipePrice.Should().Be(recipeRequestDTO.RecipePrice);
            updatedRecipe!.Countries.Any(x => x.CountryId == recipeRequestDTO.CountryId).Should().Be(true);
            updatedRecipe!.RecipeIngredients.Count().Should().Be(4);
        }

        [Fact]
        public async void UpdateRecipe_UpdateRecipe_RecipeNotExistInRepo()
        {
            var recipes = RecipesSample();
            var ingredients = IngredientsSample();
            var countries = CountriesSample();
            RecipeRequestDTO recipeRequestDTO = new()
            {
                RecipeId = 4,
                UserId = 1,
                FeaturedImage = "mock-image-link-update",
                RecipeDescription = "mock-description-update",
                VideoLink = "mock-video-link-update",
                RecipeTitle = "mock-title-update",
                PreparationTime = 10,
                CookTime = 10,
                RecipeServe = 4,
                RecipeContent = "mock-content-update",
                RecipeStatus = 1,
                IsFree = true,
                RecipePrice = 0,
                CreateDate = DateTime.Now,
                CountryId = 1,
                RecipeIngredients = [
                    new RecipeIngredientRequestDTO()
                    {
                        IngredientId = 1,
                        UnitValue = 10
                    },
                    new RecipeIngredientRequestDTO()
                    {
                        IngredientId = 2,
                        UnitValue = 15
                    },
                ]
            };
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeRequestDTO.RecipeId.Value)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeRequestDTO.RecipeId.Value));
            _recipeRepositoryMock.Setup(x => x.UpdateRecipe(It.IsAny<Recipe>())).Callback<Recipe>((recipe) => recipes[0] = recipe);
            _countryRepositoryMock.Setup(x => x.GetCountry(1)).ReturnsAsync(countries.FirstOrDefault(x => x.CountryId == 1));
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(1)).ReturnsAsync(ingredients.FirstOrDefault(x => x.IngredientId == 1));
            _ingredientRepositoryMock.Setup(x => x.GetIngredientById(2)).ReturnsAsync(ingredients.FirstOrDefault(x => x.IngredientId == 2));

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var boolResult = await _recipeManager.UpdateRecipe(recipeRequestDTO);
            var updatedRecipe = recipes.FirstOrDefault(x => x.RecipeId == 4);

            boolResult.Should().BeFalse();
            updatedRecipe.Should().BeNull();
        }

        [Fact]
        public async void DeleteRecipe_DeleteRecipeStatus_RecipeExistInRepo()
        {
            var recipes = RecipesSample();

            _recipeRepositoryMock.Setup(x => x.GetRecipe(1)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == 1));
            _recipeRepositoryMock.Setup(x => x.DeleteRecipe(It.IsAny<Recipe>())).Callback<Recipe>((recipe) => recipes[0] = recipe);

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var boolResult = await _recipeManager.DeleteRecipe(1);
            var updatedRecipe = recipes.FirstOrDefault(x => x.RecipeId == 1);

            boolResult.Should().BeTrue();
            updatedRecipe.Should().NotBeNull();
            updatedRecipe!.RecipeStatus.Should().Be(0);
        }

        [Fact]
        public async void DeleteRecipe_DeleteRecipeStatus_RecipeNotExistInRepo()
        {
            var recipes = RecipesSample();

            _recipeRepositoryMock.Setup(x => x.GetRecipe(-1)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == -1));
            _recipeRepositoryMock.Setup(x => x.DeleteRecipe(It.IsAny<Recipe>())).Callback<Recipe>((recipe) => recipes[0] = recipe);

            IRecipeManager _recipeManager = new RecipeManager(_recipeRepositoryMock.Object, _categoryRepositoryMock.Object,
                                                            _countryRepositoryMock.Object, _userRepositoryMock.Object,
                                                            _recipeIngredientRepositoryMock.Object, _historyRepositoryMock.Object,
                                                            _mapper);
            var boolResult = await _recipeManager.DeleteRecipe(1);
            var updatedRecipe = recipes.FirstOrDefault(x => x.RecipeId == -1);

            boolResult.Should().BeFalse();
            updatedRecipe.Should().BeNull();
        }

        private static List<Recipe> RecipesSample()
        {
            List<Recipe> output = [
                new Recipe()
                {
                    RecipeId = 1,
                    UserId = 1,
                    FeaturedImage = "mock-image-link",
                    RecipeTitle = "mock-title",
                    RecipeDescription = "mock-description",
                    VideoLink = "mock-video-link",
                    PreparationTime = 10,
                    CookTime = 10,
                    RecipeServe = 4,
                    RecipeContent = "mock-content",
                    RecipeRating = 5,
                    RecipeRatings = [
                        new RecipeRating()
                        {
                            RatingId = 1,
                            RecipeId = 1,
                            UserId = 1,
                            User = new()
                        {
                            UserId = 1,
                            FirstName = "firstname",
                            MiddleName = "middlename",
                            LastName = "lastname",
                            Avatar = "link"
                        },
                            RatingValue = 5,
                            RatingStatus = 1,
                            RatingContent = "Rating 1",
                            CreateDate = DateTime.Now
                        },
                        new RecipeRating()
                        {
                            RatingId = 2,
                            RecipeId = 1,
                            UserId = 2,
                            User = new()
                            {
                                UserId = 2,
                                FirstName = "firstname",
                                MiddleName = "middlename",
                                LastName = "lastname",
                                Avatar = "link"
                            },
                            RatingValue = 1,
                            RatingStatus = 1,
                            RatingContent = "Rating 2",
                            CreateDate = DateTime.Now.AddHours(1)
                        },
                        new RecipeRating()
                        {
                            RatingId = 3,
                            RecipeId = 1,
                            UserId = 3,
                            User = new()
                            {
                                UserId = 3,
                                FirstName = "firstname",
                                MiddleName = "middlename",
                                LastName = "lastname",
                                Avatar = "link"
                            },
                            RatingValue = 3,
                            RatingStatus = 1,
                            RatingContent = "Rating 3",
                            CreateDate = DateTime.Now.AddHours(3)
                        },
                    ],
                    RecipeStatus = 1,
                    IsFree = true,
                    RecipePrice = 0,
                    CreateDate = DateTime.Now,
                    RecipeIngredients = [
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 1,
                            UnitValue = 10,
                            Ingredient = new Ingredient()
                            {
                                IngredientId = 1,
                                IngredientName = "Thịt bò",
                                IngredientUnit = "g",
                                IngredientStatus = 1
                            }
                        },
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 2,
                            UnitValue = 15,
                            Ingredient = new Ingredient()
                            {
                                IngredientId = 1,
                                IngredientName = "Thịt gà",
                                IngredientUnit = "g",
                                IngredientStatus = 1
                            },
                        }
                    ],
                    Categories = [
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
                    ],
                    Countries = [
                        new Country()
                        {
                            CountryId = 1,
                            CountryName = "Việt Nam",
                            CountryStatus = 1
                        },
                    ]
                },
                new Recipe()
                {
                    RecipeId = 2,
                    UserId = 1,
                    FeaturedImage = "mock-image-link-2",
                    RecipeTitle = "mock-title-2",
                    RecipeDescription = "mock-description-2",
                    VideoLink = "mock-video-link-2",
                    PreparationTime = 10,
                    CookTime = 10,
                    RecipeServe = 4,
                    RecipeContent = "mock-content-2",
                    RecipeRating = 5,
                    RecipeStatus = 1,
                    IsFree = false,
                    RecipePrice = 10000,
                    CreateDate = DateTime.Now,
                    RecipeIngredients = [
                        new RecipeIngredient()
                        {
                            RecipeId = 1,
                            IngredientId = 1,
                            UnitValue = 10,
                            Ingredient = new Ingredient()
                            {
                                IngredientId = 1,
                                IngredientName = "Thịt bò",
                                IngredientUnit = "g",
                                IngredientStatus = 1
                            }
                        },
                    ]
                },
            ];
            return output;
        }

        private static List<Ingredient> IngredientsSample()
        {
            List<Ingredient> output = [
                new Ingredient()
                {
                    IngredientId = 1,
                    IngredientName = "Thịt bò",
                    IngredientUnit = "g",
                    IngredientStatus = 1
                },
                new Ingredient()
                {
                    IngredientId = 1,
                    IngredientName = "Thịt gà",
                    IngredientUnit = "g",
                    IngredientStatus = 1
                },
                new Ingredient()
                {
                    IngredientId = 1,
                    IngredientName = "Thịt lợn",
                    IngredientUnit = "g",
                    IngredientStatus = 1
                },
            ];
            return output;
        }

        private static List<Country> CountriesSample()
        {
            List<Country> output = [
                new Country()
                {
                    CountryId = 1,
                    CountryName = "Việt Nam",
                    CountryStatus = 1
                },
                new Country()
                {
                    CountryId = 2,
                    CountryName = "Mỹ",
                    CountryStatus = 1
                },
                new Country()
                {
                    CountryId = 1,
                    CountryName = "Anh",
                    CountryStatus = 1
                },
            ];
            return output;
        }

        private static List<User> UsersSample()
        {
            List<User> output = [
                new User()
                {
                    UserId = 1,
                    Username = "mock_1",
                    FirstName = "firstname_1",
                    MiddleName = "middlename_1",
                    LastName = "lastname_1",
                    Email = "mock1@mail.com",
                    Phone = "0904285035",
                    Avatar = "mock-avatar-link",
                    PasswordHash = "3A7A4EE1E7AE7D0C9F95193571200B157A6BE0FFA59EDC8F84D39E4AD2D4D9FE:C433EF5F9226A6E7DE52883236AC00EF:301200:SHA256",
                    StatusId = 1,
                    Status = new Status()
                    {
                        StatusId = 1,
                        StatusName = "Active"
                    },
                    RoleId = 1,
                    Role = new Role()
                    {
                        RoleId = 1,
                        RoleName = "Admin"
                    },
                    Interaction = 0
                },
                new User()
                {
                    UserId = 2,
                    Username = "mock_2",
                    FirstName = "firstname_2",
                    MiddleName = "middlename_2",
                    LastName = "lastname_2",
                    Email = "mock2@mail.com",
                    Phone = "0904285035",
                    Avatar = "mock-avatar-link",
                    PasswordHash = "3A7A4EE1E7AE7D0C9F95193571200B157A6BE0FFA59EDC8F84D39E4AD2D4D9FE:C433EF5F9226A6E7DE52883236AC00EF:301200:SHA256",
                    StatusId = 1,
                    Status = new Status()
                    {
                        StatusId = 1,
                        StatusName = "Active"
                    },
                    RoleId = 1,
                    Role = new Role()
                    {
                        RoleId = 1,
                        RoleName = "Admin"
                    },
                    Interaction = 0
                },
                new User()
                {
                    UserId = 3,
                    Username = "mock_3",
                    FirstName = "firstname_3",
                    MiddleName = "middlename_3",
                    LastName = "lastname_3",
                    Email = "mock3@mail.com",
                    Phone = "0904285035",
                    Avatar = "mock-avatar-link",
                    PasswordHash = "3A7A4EE1E7AE7D0C9F95193571200B157A6BE0FFA59EDC8F84D39E4AD2D4D9FE:C433EF5F9226A6E7DE52883236AC00EF:301200:SHA256",
                    StatusId = 1,
                    Status = new Status()
                    {
                        StatusId = 1,
                        StatusName = "Active"
                    },
                    RoleId = 1,
                    Role = new Role()
                    {
                        RoleId = 1,
                        RoleName = "Admin"
                    },
                    Interaction = 0
                },
            ];
            return output;
        }
    }
}
