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
    public class BookmarkManagerTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IRecipeRepository> _recipeRepositoryMock;
        private readonly IMapper _mapper;

        public BookmarkManagerTest()
        {
            //Initial setup
            _userRepositoryMock = new Mock<IUserRepository>();
            _recipeRepositoryMock = new Mock<IRecipeRepository>();
            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<UserProfile>();
                options.AddProfile<RecipeProfile>();
            }));
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetBookmarkOfUser_GetBookmarkOfUser_BookmarkExistInRepo()
        {
            //Arrange
            var users = UsersSample();
            List<BookmarkDTO> bookmarkDTOs = [];
            bookmarkDTOs.AddRange(users.Select(_mapper.Map<User, BookmarkDTO>));
            _userRepositoryMock.Setup(x => x.GetBookmarkOfUser(1)).ReturnsAsync(users.Find(x => x.UserId == 1)); //Mock Advertisement repository GetAdvertisementById(int id) method

            //Act
            IBookmarkManager _bookmarkManager = new BookmarkManager(_userRepositoryMock.Object,_recipeRepositoryMock.Object , _mapper);
            var result = await _bookmarkManager.GetBookmarkOfUser(1);
            var actual = bookmarkDTOs.Find(x => x.UserId == 1);

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<BookmarkDTO>().And.BeEquivalentTo(actual!);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetBookmarkOfUser_GetBookmarkOfUser_BookmarkNotExistInRepo()
        {
            var users = UsersSample();
            _userRepositoryMock.Setup(x => x.GetBookmarkOfUser(-1)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == -1));

            IBookmarkManager _bookmarkManager = new BookmarkManager(_userRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var result = await _bookmarkManager.GetBookmarkOfUser(-1);
            var actual = users.FirstOrDefault(x => x.UserId == -1);

            result.Should().BeNull();
            actual.Should().BeNull();
        }

        [Fact]
        public async void AddRecipeToBookmark_AddRecipeToBookmark_UserRecipeExistInRepo()
        {
            var users = UsersSample();
            var recipes = RecipesSample();
            int userId = 1;
            int recipeId = 1;

            _userRepositoryMock.Setup(x => x.GetUser(userId)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == userId));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeId)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeId));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IBookmarkManager _bookmarkManager = new BookmarkManager(_userRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _bookmarkManager.AddRecipeToBookmark(userId, recipeId);
            var updatedUser = users.FirstOrDefault(x => x.UserId == userId);
            var countResult = updatedUser.Recipes.Count();

            boolResult.Should().BeTrue();
            updatedUser.Should().NotBeNull();
            countResult.Should().Be(3);
        }

        [Fact]
        public async void AddRecipeToBookmark_AddRecipeToBookmark_UserNotExistInRepo()
        {
            var users = UsersSample();
            var recipes = RecipesSample();
            int userId = -1;
            int recipeId = 1;

            _userRepositoryMock.Setup(x => x.GetUser(userId)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == userId));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeId)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeId));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IBookmarkManager _bookmarkManager = new BookmarkManager(_userRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _bookmarkManager.AddRecipeToBookmark(userId, recipeId);
            var updatedUser = users.FirstOrDefault(x => x.UserId == userId);

            var countResult = 0;
            if (updatedUser != null)
                countResult = updatedUser.Recipes.Count();

            boolResult.Should().BeFalse();
            updatedUser.Should().BeNull();
            countResult.Should().Be(0);
        }

        [Fact]
        public async void AddRecipeToBookmark_AddRecipeToBookmark_RecipeNotExistInRepo()
        {
            var users = UsersSample();
            var recipes = RecipesSample();
            int userId = 1;
            int recipeId = -1;

            _userRepositoryMock.Setup(x => x.GetUser(userId)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == userId));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeId)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeId));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IBookmarkManager _bookmarkManager = new BookmarkManager(_userRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _bookmarkManager.AddRecipeToBookmark(userId, recipeId);
            var updatedUser = users.FirstOrDefault(x => x.UserId == userId);

            var countResult = updatedUser.Recipes.Count();

            boolResult.Should().BeFalse();
            updatedUser.Should().NotBeNull();
            countResult.Should().Be(2);
        }

        [Fact]
        public async void RemoveRecipeFromBookmark_RemoveRecipeFromBookmark_UserRecipeExistInRepo()
        {
            var users = UsersSample();
            var recipes = RecipesSample();
            int userId = 1;
            int recipeId = 2;

            _userRepositoryMock.Setup(x => x.GetUser(userId)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == userId));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeId)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeId));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IBookmarkManager _bookmarkManager = new BookmarkManager(_userRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _bookmarkManager.RemoveRecipeFromBookmark(userId, recipeId);
            var updatedUser = users.FirstOrDefault(x => x.UserId == userId);
            var countResult = updatedUser.Recipes.Count() - 1;

            boolResult.Should().BeTrue();
            updatedUser.Should().NotBeNull();
            countResult.Should().Be(1);
        }

        [Fact]
        public async void RemoveRecipeFromBookmark_RemoveRecipeFromBookmark_UserNotExistInRepo()
        {
            var users = UsersSample();
            var recipes = RecipesSample();
            int userId = -1;
            int recipeId = 1;

            _userRepositoryMock.Setup(x => x.GetUser(userId)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == userId));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeId)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeId));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IBookmarkManager _bookmarkManager = new BookmarkManager(_userRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _bookmarkManager.RemoveRecipeFromBookmark(userId, recipeId);
            var updatedUser = users.FirstOrDefault(x => x.UserId == userId);

            var countResult = 0;
            if (updatedUser != null)
                countResult = updatedUser.Recipes.Count();

            boolResult.Should().BeFalse();
            updatedUser.Should().BeNull();
            countResult.Should().Be(0);
        }

        [Fact]
        public async void RemoveRecipeFromBookmark_RemoveRecipeFromBookmark_RecipeNotExistInRepo()
        {
            var users = UsersSample();
            var recipes = RecipesSample();
            int userId = 1;
            int recipeId = -1;

            _userRepositoryMock.Setup(x => x.GetUser(userId)).ReturnsAsync(users.FirstOrDefault(x => x.UserId == userId));
            _recipeRepositoryMock.Setup(x => x.GetRecipe(recipeId)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == recipeId));
            _userRepositoryMock.Setup(x => x.UpdateUser(It.IsAny<User>())).Callback<User>((user) => users[0] = user);

            IBookmarkManager _bookmarkManager = new BookmarkManager(_userRepositoryMock.Object, _recipeRepositoryMock.Object, _mapper);
            var boolResult = await _bookmarkManager.RemoveRecipeFromBookmark(userId, recipeId);
            var updatedUser = users.FirstOrDefault(x => x.UserId == userId);

            var countResult = updatedUser.Recipes.Count();

            boolResult.Should().BeFalse();
            updatedUser.Should().NotBeNull();
            countResult.Should().Be(2);
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
                    Interaction = 0,
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
                    Interaction = 0,
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
