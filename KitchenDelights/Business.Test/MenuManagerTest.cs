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
    public class MenuManagerTest
    {
        private readonly Mock<IMenuRepository> _menuRepositoryMock;
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
        private readonly IMapper _mapper;

        public MenuManagerTest()
        {
            //Initial setup
            _menuRepositoryMock = new Mock<IMenuRepository>();
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<MenuProfile>();
                options.AddProfile<RecipeProfile>();
            }));
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetAllMenus_GetAllMenus_MenuExistInRepo()
        {
            //Arrange
            var menus = MenusSample();
            List<MenuDTO> menuDTOs = [];
            menuDTOs.AddRange(menus.Select(_mapper.Map<Menu, MenuDTO>));
            _menuRepositoryMock.Setup(x => x.GetAllMenus()).ReturnsAsync(menus.ToList()); //Mock Advertisement repository GetAdvertisementById(int id) method

            //Act
            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var result = await _menuManager.GetAllMenues();

            result.Should().BeOfType<List<MenuDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(2);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetMenuById_GetMenuById_MenuExistInRepo()
        {
            //Arrange
            var menus = MenusSample();
            List<MenuDTO> menuDTOs = [];
            menuDTOs.AddRange(menus.Select(_mapper.Map<Menu, MenuDTO>));
            _menuRepositoryMock.Setup(x => x.GetMenuById(1)).ReturnsAsync(menus.Find(x => x.MenuId == 1)); //Mock Advertisement repository GetAdvertisementById(int id) method

            //Act
            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var result = await _menuManager.GetMenuById(1);
            var actual = menuDTOs.Find(x => x.MenuId == 1);

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<MenuDTO>().And.BeEquivalentTo(actual!);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetMenuById_GetMenuById_MenuNotExistInRepo()
        {
            var menus = MenusSample();
            _menuRepositoryMock.Setup(x => x.GetMenuById(-1)).ReturnsAsync(menus.FirstOrDefault(x => x.MenuId == -1));

            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var result = await _menuManager.GetMenuById(-1);
            var actual = menus.FirstOrDefault(x => x.MenuId == -1);

            result.Should().BeNull();
            actual.Should().BeNull();
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetMenuByUserId_GetMenuByUserId_MenuExistInRepo()
        {
            //Arrange
            var menus = MenusSample();
            List<MenuDTO> menuDTOs = [];
            menuDTOs.AddRange(menus.Select(_mapper.Map<Menu, MenuDTO>));
            _menuRepositoryMock.Setup(x => x.GetMenuByUserId(2)).ReturnsAsync(menus.Where(x => x.UserId == 2).ToList()); //Mock Advertisement repository GetAdvertisementById(int id) method

            //Act
            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var result = await _menuManager.GetMenuByUserId(2);

            result.Should().BeOfType<List<MenuDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(1);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetMenuByUserIdAndCheckExistRecipe_GetMenuByUserIdAndCheckExistRecipe_RecipeExistInMenu()
        {
            //Arrange
            var menus = MenusSample();
            var recipes = RecipesSample();
            _menuRepositoryMock.Setup(x => x.GetMenuByUserId(1)).ReturnsAsync(menus.Where(x => x.UserId == 1).ToList);
            _recipeRepositoryMock.Setup(x => x.GetRecipe(2)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == 2));

            //Act
            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var result = await _menuManager.GetMenuByUserIdAndCheckExistRecipe(1, 2);

            result.Should().BeOfType<List<MenuDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(1);
        }


        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetMenuByUserId_GetMenuByUserId_MenuNotExistInRepo()
        {
            //Arrange
            var menus = MenusSample();
            List<MenuDTO> menuDTOs = [];
            menuDTOs.AddRange(menus.Select(_mapper.Map<Menu, MenuDTO>));
            _menuRepositoryMock.Setup(x => x.GetMenuByUserId(-1)).ReturnsAsync(menus.Where(x => x.UserId == -1).ToList()); //Mock Advertisement repository GetAdvertisementById(int id) method

            //Act
            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var result = await _menuManager.GetMenuByUserId(-1);

            result.Should().BeOfType<List<MenuDTO>>()
            .And.BeNullOrEmpty();
            result.Count.Should().Be(0);
        }

        [Fact]
        public async void CreateMenu_CreateWithMenuDTO_MenuNotExistInRepo()
        {
            var menus = MenusSample();
            MenuRequestDTO menuDTO = new()
            {
                MenuId = 3,
                FeaturedImage = "mock-image-link",
                MenuName = "mock-name",
                MenuDescription = "mock-description",
                UserId = 1,
            };
            _menuRepositoryMock.Setup(x => x.CreateMenu(It.IsAny<Menu>())).Callback<Menu>(menus.Add);
            _menuRepositoryMock.Setup(x => x.GetMenuById(3)).ReturnsAsync(menus.FirstOrDefault(x => x.MenuId == 3));

            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _menuManager.CreateMenu(menuDTO);
            var countResult = menus.Count;

            boolResult.Should().BeTrue();
            countResult.Should().Be(3);
        }

        [Fact]
        public async void UpdateMenu_UpdateMenu_MenuExistInRepo()
        {
            var menus = MenusSample();
            MenuRequestDTO menuDTO = new()
            {
                MenuId = 1,
                FeaturedImage = "mock-image-link-update",
                MenuName = "mock-name-update",
                MenuDescription = "mock-description-update",
            };
            _menuRepositoryMock.Setup(x => x.GetMenuById(1)).ReturnsAsync(menus.FirstOrDefault(x => x.MenuId == 1));
            _menuRepositoryMock.Setup(x => x.UpdateMenu(It.IsAny<Menu>())).Callback<Menu>((menu) => menus[0] = menu);

            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _menuManager.UpdateMenu(menuDTO);
            var updatedAdvertisement = menus.FirstOrDefault(x => x.MenuId == 1);

            boolResult.Should().BeTrue();
            updatedAdvertisement.Should().NotBeNull();
            updatedAdvertisement!.FeaturedImage.Should().BeSameAs(menuDTO.FeaturedImage);
            updatedAdvertisement!.MenuName.Should().BeSameAs(menuDTO.MenuName);
            updatedAdvertisement!.MenuDescription.Should().BeSameAs(menuDTO.MenuDescription);
        }

        [Fact]
        public async void UpdateMenu_UpdateMenu_NotExistInRepo()
        {
            var menus = MenusSample();
            MenuRequestDTO menuDTO = new()
            {
                MenuId = 3,
                FeaturedImage = "mock-image-link-update",
                MenuName = "mock-name-update",
                MenuDescription = "mock-description-update",
            };
            _menuRepositoryMock.Setup(x => x.GetMenuById(3)).ReturnsAsync(menus.FirstOrDefault(x => x.MenuId == 3));
            _menuRepositoryMock.Setup(x => x.UpdateMenu(It.IsAny<Menu>())).Callback<Menu>((menu) => menus[2] = menu);

            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _menuManager.UpdateMenu(menuDTO);
            var updatedMenu = menus.FirstOrDefault(x => x.MenuId == 3);

            boolResult.Should().BeFalse();
            updatedMenu.Should().BeNull();
        }

        [Fact]
        public async void DeleteAdvertisement_DeleteAdvertisement_AdvertisementExistInRepo()
        {
            var menus = MenusSample();
            _menuRepositoryMock.Setup(x => x.GetMenuById(1)).ReturnsAsync(menus.FirstOrDefault(x => x.MenuId == 1));
            _menuRepositoryMock.Setup(x => x.DeleteMenu(It.IsAny<Menu>())).Callback<Menu>(item => menus.Remove(item));

            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _menuManager.DeleteMenu(1);
            var actual = menus.ToList();

            boolResult.Should().BeTrue();
            actual.Count.Should().Be(1);
        }

        [Fact]
        public async void DeleteAdvertisement_DeleteAdvertisement_AdvertisementNotExistInRepo()
        {
            var menus = MenusSample();
            _menuRepositoryMock.Setup(x => x.GetMenuById(-1)).ReturnsAsync(menus.FirstOrDefault(x => x.MenuId == -1));
            _menuRepositoryMock.Setup(x => x.DeleteMenu(It.IsAny<Menu>())).Callback<Menu>(item => menus.Remove(item));

            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _menuManager.DeleteMenu(-1);
            var actual = menus.ToList();

            boolResult.Should().BeFalse();
            actual.Count.Should().Be(2);
        }

        [Fact]
        public async void AddRecipeToMenu_AddRecipeToMenu_MenuRecipeExistInRepo()
        {
            var menus = MenusSample();
            var recipes = RecipesSample();
            int menuId = 1;
            int recipeId = 1;

            _menuRepositoryMock.Setup(x => x.GetMenuById(menuId)).ReturnsAsync(menus.FirstOrDefault(x => x.MenuId == menuId));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeId)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeId));
            _menuRepositoryMock.Setup(x => x.UpdateMenu(It.IsAny<Menu>())).Callback<Menu>((menu) => menus[0] = menu);

            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _menuManager.AddRecipeToMenu(menuId, recipeId);
            var updatedMenu = menus.FirstOrDefault(x => x.MenuId == menuId);
            var countResult = updatedMenu.Recipes.Count();

            boolResult.Should().BeTrue();
            updatedMenu.Should().NotBeNull();
            countResult.Should().Be(3);
        }

        [Fact]
        public async void AddRecipeToBookmark_AddRecipeToBookmark_MenuNotExistInRepo()
        {
            var menus = MenusSample();
            var recipes = RecipesSample();
            int menuId = -1;
            int recipeId = 1;

            _menuRepositoryMock.Setup(x => x.GetMenuById(menuId)).ReturnsAsync(menus.FirstOrDefault(x => x.MenuId == menuId));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeId)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeId));
            _menuRepositoryMock.Setup(x => x.UpdateMenu(It.IsAny<Menu>())).Callback<Menu>((menu) => menus[0] = menu);

            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _menuManager.AddRecipeToMenu(menuId, recipeId);
            var updatedMenu = menus.FirstOrDefault(x => x.MenuId == menuId);

            var countResult = 0;
            if (updatedMenu != null)
                countResult = updatedMenu.Recipes.Count();

            boolResult.Should().BeFalse();
            updatedMenu.Should().BeNull();
            countResult.Should().Be(0);
        }

        [Fact]
        public async void AddRecipeToBookmark_AddRecipeToBookmark_RecipeNotExistInRepo()
        {
            var menus = MenusSample();
            var recipes = RecipesSample();
            int menuId = 1;
            int recipeId = -1;

            _menuRepositoryMock.Setup(x => x.GetMenuById(menuId)).ReturnsAsync(menus.FirstOrDefault(x => x.MenuId == menuId));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeId)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeId));
            _menuRepositoryMock.Setup(x => x.UpdateMenu(It.IsAny<Menu>())).Callback<Menu>((menu) => menus[0] = menu);

            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _menuManager.AddRecipeToMenu(menuId, recipeId);
            var updatedMenu = menus.FirstOrDefault(x => x.MenuId == menuId);

            var countResult = updatedMenu.Recipes.Count();

            boolResult.Should().BeFalse();
            updatedMenu.Should().NotBeNull();
            countResult.Should().Be(2);
        }

        [Fact]
        public async void RemoveRecipeFromBookmark_RemoveRecipeFromBookmark_UserRecipeExistInRepo()
        {
            var menus = MenusSample();
            var recipes = RecipesSample();
            int menuId = 1;
            int recipeId = 2;

            _menuRepositoryMock.Setup(x => x.GetMenuById(menuId)).ReturnsAsync(menus.FirstOrDefault(x => x.MenuId == menuId));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeId)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeId));
            _menuRepositoryMock.Setup(x => x.UpdateMenu(It.IsAny<Menu>())).Callback<Menu>((menu) => menus[0] = menu);

            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _menuManager.RemoveRecipeFromMenu(menuId, recipeId);
            var updatedMenu = menus.FirstOrDefault(x => x.MenuId == menuId);
            var countResult = updatedMenu.Recipes.Count() - 1;

            boolResult.Should().BeTrue();
            updatedMenu.Should().NotBeNull();
            countResult.Should().Be(1);
        }

        [Fact]
        public async void RemoveRecipeFromBookmark_RemoveRecipeFromBookmark_UserNotExistInRepo()
        {
            var menus = MenusSample();
            var recipes = RecipesSample();
            int menuId = -1;
            int recipeId = 1;

            _menuRepositoryMock.Setup(x => x.GetMenuById(menuId)).ReturnsAsync(menus.FirstOrDefault(x => x.MenuId == menuId));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeId)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeId));
            _menuRepositoryMock.Setup(x => x.UpdateMenu(It.IsAny<Menu>())).Callback<Menu>((menu) => menus[0] = menu);

            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _menuManager.RemoveRecipeFromMenu(menuId, recipeId);
            var updatedMenu = menus.FirstOrDefault(x => x.MenuId == menuId);

            var countResult = 0;
            if (updatedMenu != null)
                countResult = updatedMenu.Recipes.Count();

            boolResult.Should().BeFalse();
            updatedMenu.Should().BeNull();
            countResult.Should().Be(0);
        }

        [Fact]
        public async void RemoveRecipeFromBookmark_RemoveRecipeFromBookmark_RecipeNotExistInRepo()
        {
            var menus = MenusSample();
            var recipes = RecipesSample();
            int menuId = 1;
            int recipeId = -1;

            _menuRepositoryMock.Setup(x => x.GetMenuById(menuId)).ReturnsAsync(menus.FirstOrDefault(x => x.MenuId == menuId));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeId)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeId));
            _menuRepositoryMock.Setup(x => x.UpdateMenu(It.IsAny<Menu>())).Callback<Menu>((menu) => menus[0] = menu);

            IMenuManager _menuManager = new MenuManager(_menuRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _menuManager.RemoveRecipeFromMenu(menuId, recipeId);
            var updatedUser = menus.FirstOrDefault(x => x.MenuId == menuId);

            var countResult = updatedUser.Recipes.Count();

            boolResult.Should().BeFalse();
            updatedUser.Should().NotBeNull();
            countResult.Should().Be(2);
        }

        private static List<Menu> MenusSample()
        {
            List<Menu> output = [
                new Menu()
                {
                    MenuId = 1,
                    FeaturedImage = "mock-image-link",
                    MenuName = "mock-name",
                    MenuDescription = "mock-description",
                    UserId = 1,
                    Recipes = [
                        new Recipe()
                        {
                            RecipeId = 2,
                            UserId = 2,
                            FeaturedImage = "image_2",
                            RecipeTitle = "title_2",
                            RecipeDescription = "description_2",
                            VideoLink = "link_2",
                            RecipeStatus = 1,
                            IsFree = true,
                            RecipePrice = 10000
                        },
                        new Recipe()
                        {
                            RecipeId = 3,
                            UserId = 1,
                            FeaturedImage = "image_3",
                            RecipeTitle = "title_3",
                            RecipeDescription = "description_3",
                            VideoLink = "link_3",
                            RecipeStatus = 1,
                            IsFree = true,
                            RecipePrice = 10000
                        }
                    ],
                },
                new Menu()
                {
                    MenuId = 2,
                    FeaturedImage = "mock-image-link",
                    MenuName = "mock-name",
                    MenuDescription = "mock-description",
                    UserId = 2,
                    Recipes = [
                        new Recipe()
                        {
                            RecipeId = 1,
                            UserId = 1,
                            FeaturedImage = "image_1",
                            RecipeTitle = "title_1",
                            RecipeDescription = "description_1",
                            VideoLink = "link_1",
                            RecipeStatus = 1,
                            IsFree = true,
                            RecipePrice = 10000
                        },
                        new Recipe()
                        {
                            RecipeId = 2,
                            UserId = 2,
                            FeaturedImage = "image_2",
                            RecipeTitle = "title_2",
                            RecipeDescription = "description_2",
                            VideoLink = "link_2",
                            RecipeStatus = 1,
                            IsFree = true,
                            RecipePrice = 10000
                        },
                        new Recipe()
                        {
                            RecipeId = 3,
                            UserId = 1,
                            FeaturedImage = "image_3",
                            RecipeTitle = "title_3",
                            RecipeDescription = "description_3",
                            VideoLink = "link_3",
                            RecipeStatus = 1,
                            IsFree = true,
                            RecipePrice = 10000
                        }
                    ],
                },
            ];
            return output;
        }

        private static List<Recipe> RecipesSample()
        {
            List<Recipe> output = [
                new Recipe()
                {
                    RecipeId = 1,
                    UserId = 1,
                    FeaturedImage = "image_1",
                    RecipeTitle = "title_1",
                    RecipeDescription = "description_1",
                    VideoLink = "link_1",
                    RecipeStatus = 1,
                    IsFree = true,
                    RecipePrice = 10000
                },
                new Recipe()
                {
                    RecipeId = 2,
                    UserId = 2,
                    FeaturedImage = "image_2",
                    RecipeTitle = "title_2",
                    RecipeDescription = "description_2",
                    VideoLink = "link_2",
                    RecipeStatus = 1,
                    IsFree = true,
                    RecipePrice = 10000
                },
                new Recipe()
                {
                    RecipeId = 3,
                    UserId = 1,
                    FeaturedImage = "image_3",
                    RecipeTitle = "title_3",
                    RecipeDescription = "description_3",
                    VideoLink = "link_3",
                    RecipeStatus = 1,
                    IsFree = true,
                    RecipePrice = 10000
                }
            ];
            return output;
        }
    }
}
