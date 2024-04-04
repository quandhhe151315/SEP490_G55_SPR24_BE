using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Managers;
using Business.Profiles;
using Data.Entity;
using Data.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

namespace Business.Test;

public class RatingManagerTest
{
    private Mock<IRatingRepository> _mockRatingRepository;
    private Mock<IRecipeRepository> _mockRecipeRepository;
    private IMapper _mapper;

    public RatingManagerTest() {
        _mockRatingRepository = new Mock<IRatingRepository>();
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<RecipeProfile>();
            }));
    }

    [Fact]
    public async void GetRecipeRatings_GetRatingsByRecipeId_RecipeExistInRepo() {
        List<RecipeRating> ratings = GetRatings();
        _mockRatingRepository.Setup(x => x.GetRatings(1)).ReturnsAsync(ratings.Where(x => x.RecipeId == 1).ToList());

        RatingManager _ratingManager = new RatingManager(_mockRatingRepository.Object, _mockRecipeRepository.Object, _mapper);
        var result = await _ratingManager.GetRecipeRatings(1);
        var actual = ratings.Where(x => x.RecipeId == 1).ToList();

        result.Should().BeOfType<List<RecipeRatingDTO>>();
        result.Count.Should().Be(actual.Count);
    }

    [Fact]
    public async void GetRecipeRatings_GetRatingsByRecipeId_BoundaryRecipeId() {
        List<RecipeRating> ratings = GetRatings();
        _mockRatingRepository.Setup(x => x.GetRatings(0)).ReturnsAsync(ratings.Where(x => x.RecipeId == 0).ToList());

        RatingManager _ratingManager = new RatingManager(_mockRatingRepository.Object, _mockRecipeRepository.Object, _mapper);
        var result = await _ratingManager.GetRecipeRatings(0);
        var actual = ratings.Where(x => x.RecipeId == 0).ToList();

        result.Should().BeOfType<List<RecipeRatingDTO>>();
        result.Count.Should().Be(actual.Count);
    }

    [Fact]
    public async void GetRecipeRatings_GetRatingsByRecipeId_AbnormalRecipeId() {
        List<RecipeRating> ratings = GetRatings();
        _mockRatingRepository.Setup(x => x.GetRatings(-1)).ReturnsAsync(ratings.Where(x => x.RecipeId == -1).ToList());

        RatingManager _ratingManager = new RatingManager(_mockRatingRepository.Object, _mockRecipeRepository.Object, _mapper);
        var result = await _ratingManager.GetRecipeRatings(-1);
        var actual = ratings.Where(x => x.RecipeId == -1).ToList();

        result.Should().BeOfType<List<RecipeRatingDTO>>();
        result.Count.Should().Be(actual.Count);
    }

    [Fact]
    public async void CreateRating_CreateNewRating_RecipeExistInRepo(){
        List<RecipeRating> ratings = GetRatings();
        RecipeRatingDTO toAdd = new() {
            RecipeId = 1,
            UserId = 4,
            RatingValue = 5,
            RatingContent = "content",
            CreateDate = DateTime.Now.AddHours(4)
        };
        List<Recipe> recipes = GetRecipes();
        _mockRecipeRepository.Setup(x => x.GetRecipe(1)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == 1));
        _mockRecipeRepository.Setup(x => x.UpdateRecipe(It.IsAny<Recipe>())).Callback<Recipe>(recipe => recipes[recipe.RecipeId - 1] = recipe);
        _mockRatingRepository.Setup(x => x.GetRatings(1)).ReturnsAsync(ratings.Where(x => x.RecipeId == 1).ToList());
        _mockRatingRepository.Setup(x => x.CreateRating(It.IsAny<RecipeRating>())).Callback(ratings.Add);

        RatingManager _ratingManager = new(_mockRatingRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _ratingManager.CreateRating(toAdd);
        var actual = recipes.FirstOrDefault(x => x.RecipeId == 1);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.RecipeRating.Should().Be((5m + 1m + 3m + toAdd.RatingValue) / 4m);
    }

    [Fact]
    public async void CreateRating_NotCreateNewRating_RecipeNotExistInRepo(){
        List<RecipeRating> ratings = GetRatings();
        RecipeRatingDTO toAdd = new() {
            RecipeId = -1,
            UserId = 4,
            RatingValue = 5,
            RatingContent = "content",
            CreateDate = DateTime.Now.AddHours(4)
        };
        List<Recipe> recipes = GetRecipes();
        _mockRecipeRepository.Setup(x => x.GetRecipe(-1)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == -1));
        _mockRecipeRepository.Setup(x => x.UpdateRecipe(It.IsAny<Recipe>())).Callback<Recipe>(recipe => recipes[recipe.RecipeId - 1] = recipe);
        _mockRatingRepository.Setup(x => x.GetRatings(-1)).ReturnsAsync(ratings.Where(x => x.RecipeId == -1).ToList());
        _mockRatingRepository.Setup(x => x.CreateRating(It.IsAny<RecipeRating>())).Callback(ratings.Add);

        RatingManager _ratingManager = new(_mockRatingRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _ratingManager.CreateRating(toAdd);
        var actual = recipes.FirstOrDefault(x => x.RecipeId == -1);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void UpdateRating_UpdateExistingRating_LowerRatingValue() {
        List<RecipeRating> ratings = GetRatings();
        RecipeRatingDTO toUpdate = new() {
            RatingId = 1,
            RecipeId =1,
            RatingValue = 3,
            RatingContent = "content"
        };
        List<Recipe> recipes = GetRecipes();
        _mockRecipeRepository.Setup(x => x.GetRecipe(1)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == 1));
        _mockRecipeRepository.Setup(x => x.UpdateRecipe(It.IsAny<Recipe>())).Callback<Recipe>(recipe => recipes[recipe.RecipeId - 1] = recipe);
        _mockRatingRepository.Setup(x => x.GetRatings(1)).ReturnsAsync(ratings.Where(x => x.RecipeId == 1).ToList());
        _mockRatingRepository.Setup(x => x.GetRating(1)).ReturnsAsync(ratings.FirstOrDefault(x => x.RatingId == 1));
        _mockRatingRepository.Setup(x => x.UpdateRating(It.IsAny<RecipeRating>())).Callback<RecipeRating>(rating => ratings[rating.RatingId - 1] = rating);

        RatingManager _ratingManager = new(_mockRatingRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _ratingManager.UpdateRating(toUpdate);
        var actual = recipes.FirstOrDefault(x => x.RecipeId == 1);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.RecipeRating.Should().BeLessThan((5m + 1m + 3m) / 3m);
    }

    [Fact]
    public async void UpdateRating_UpdateExistingRating_HigherRatingValue() {
        List<RecipeRating> ratings = GetRatings();
        RecipeRatingDTO toUpdate = new() {
            RatingId = 2,
            RecipeId =1,
            RatingValue = 3,
            RatingContent = "content"
        };
        List<Recipe> recipes = GetRecipes();
        _mockRecipeRepository.Setup(x => x.GetRecipe(1)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == 1));
        _mockRecipeRepository.Setup(x => x.UpdateRecipe(It.IsAny<Recipe>())).Callback<Recipe>(recipe => recipes[recipe.RecipeId - 1] = recipe);
        _mockRatingRepository.Setup(x => x.GetRatings(1)).ReturnsAsync(ratings.Where(x => x.RecipeId == 1).ToList());
        _mockRatingRepository.Setup(x => x.GetRating(2)).ReturnsAsync(ratings.FirstOrDefault(x => x.RatingId == 2));
        _mockRatingRepository.Setup(x => x.UpdateRating(It.IsAny<RecipeRating>())).Callback<RecipeRating>(rating => ratings[rating.RatingId - 1] = rating);

        RatingManager _ratingManager = new(_mockRatingRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _ratingManager.UpdateRating(toUpdate);
        var actual = recipes.FirstOrDefault(x => x.RecipeId == 1);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.RecipeRating.Should().BeGreaterThan((5m + 1m + 3m) / 3m);
    }

    [Fact]
    public async void UpdateRating_NotUpdateExistingRating_RecipeNotExistInRepo() {
        List<RecipeRating> ratings = GetRatings();
        RecipeRatingDTO toUpdate = new() {
            RatingId = 1,
            RecipeId = -1,
            RatingValue = 3,
            RatingContent = "content"
        };
        List<Recipe> recipes = GetRecipes();
        _mockRecipeRepository.Setup(x => x.GetRecipe(-1)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == -1));
        _mockRecipeRepository.Setup(x => x.UpdateRecipe(It.IsAny<Recipe>())).Callback<Recipe>(recipe => recipes[recipe.RecipeId - 1] = recipe);
        _mockRatingRepository.Setup(x => x.GetRatings(1)).ReturnsAsync(ratings.Where(x => x.RecipeId == 1).ToList());
        _mockRatingRepository.Setup(x => x.GetRating(1)).ReturnsAsync(ratings.FirstOrDefault(x => x.RatingId == 1));
        _mockRatingRepository.Setup(x => x.UpdateRating(It.IsAny<RecipeRating>())).Callback<RecipeRating>(rating => ratings[rating.RatingId - 1] = rating);

        RatingManager _ratingManager = new(_mockRatingRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _ratingManager.UpdateRating(toUpdate);
        var actual = recipes.FirstOrDefault(x => x.RecipeId == -1);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void UpdateRating_NotUpdateExistingRating_RatingNotExistInRepo() {
        List<RecipeRating> ratings = GetRatings();
        RecipeRatingDTO toUpdate = new() {
            RatingId = -1,
            RecipeId = 1,
            RatingValue = 3,
            RatingContent = "content"
        };
        List<Recipe> recipes = GetRecipes();
        _mockRecipeRepository.Setup(x => x.GetRecipe(1)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == 1));
        _mockRecipeRepository.Setup(x => x.UpdateRecipe(It.IsAny<Recipe>())).Callback<Recipe>(recipe => recipes[recipe.RecipeId - 1] = recipe);
        _mockRatingRepository.Setup(x => x.GetRatings(1)).ReturnsAsync(ratings.Where(x => x.RecipeId == 1).ToList());
        _mockRatingRepository.Setup(x => x.GetRating(-1)).ReturnsAsync(ratings.FirstOrDefault(x => x.RatingId == -1));
        _mockRatingRepository.Setup(x => x.UpdateRating(It.IsAny<RecipeRating>())).Callback<RecipeRating>(rating => ratings[rating.RatingId - 1] = rating);

        RatingManager _ratingManager = new(_mockRatingRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _ratingManager.UpdateRating(toUpdate);
        var actual = recipes.FirstOrDefault(x => x.RecipeId == 1);

        boolResult.Should().BeFalse();
        actual.Should().NotBeNull();
        actual!.RecipeRating.Should().Be((5m + 1m + 3m) / 3m);
    }

    private List<RecipeRating> GetRatings() {
        List<RecipeRating> output = [
            new RecipeRating() {
                RatingId = 1,
                RecipeId = 1,
                UserId = 1,
                User = new() {
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
            new RecipeRating() {
                RatingId = 2,
                RecipeId = 1,
                UserId = 2,
                User = new() {
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
            new RecipeRating() {
                RatingId = 3,
                RecipeId = 1,
                UserId = 3,
                User = new() {
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
        ];
        return output;
    }
    private List<Recipe> GetRecipes()
    {
        List<Recipe> output = [
            new Recipe() {
                RecipeId = 1,
                IsFree = false,
                RecipePrice = 50000,
                RecipeRating = 3,
                RecipeRatings = [
            new RecipeRating() {
                RatingId = 1,
                RecipeId = 1,
                UserId = 1,
                User = new() {
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
            new RecipeRating() {
                RatingId = 2,
                RecipeId = 1,
                UserId = 2,
                User = new() {
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
            new RecipeRating() {
                RatingId = 3,
                RecipeId = 1,
                UserId = 3,
                User = new() {
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
        ]
            },
            new Recipe() {
                RecipeId = 2,
                IsFree = true,
                RecipeRating = 0
            },
            new Recipe() {
                RecipeId = 3,
                IsFree = true,
                RecipeRating = 0
            },
            new Recipe() {
                RecipeId = 4,
                IsFree = true,
                RecipeRating = 0
            }
        ];
        return output;
    }
}
