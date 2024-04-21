using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using KitchenDelights.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace KitchenDelights.Test;

public class ReviewControllerTest
{
    private Mock<IRatingManager> _mockManager;
    private IConfiguration _configuration;

    public ReviewControllerTest() {
        _mockManager = new Mock<IRatingManager>();
        _configuration = new ConfigurationBuilder().Build();
    }

    [Fact]
    public async void Get_ReturnStatus200_ValidRecipeId() {
        _mockManager.Setup(x => x.GetRecipeRatings(1)).ReturnsAsync([
            new RecipeRatingDTO() {
              RatingId = 1,
              RatingValue = 5,
              RatingContent = "content",
              RatingStatus = 1  
            },
            new RecipeRatingDTO() {
              RatingId = 2,
              RatingValue = 5,
              RatingContent = "content",
              RatingStatus = 1  
            },
            new RecipeRatingDTO() {
              RatingId = 3,
              RatingValue = 5,
              RatingContent = "content",
              RatingStatus = 1  
            },
        ]);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Get(1);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus200_RecipeNotHaveRating() {
        _mockManager.Setup(x => x.GetRecipeRatings(3)).ReturnsAsync([]);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Get(3);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus400_InvalidRecipeId() {
        _mockManager.Setup(x => x.GetRecipeRatings(-1)).ReturnsAsync([]);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Get(-1);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus200_AllValid() {
        RecipeRatingDTO toAdd = new() {
            RecipeId = 1,
            UserId = 1,
            RatingValue = 3,
            RatingContent = "content"
        };
        _mockManager.Setup(x => x.CreateRating(It.IsAny<RecipeRatingDTO>())).ReturnsAsync(true);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Create_ReturnStatus500_RecipeNotExist() {
        RecipeRatingDTO toAdd = new() {
            RecipeId = 5,
            UserId = 1,
            RatingValue = 3,
            RatingContent = "content"
        };
        _mockManager.Setup(x => x.CreateRating(It.IsAny<RecipeRatingDTO>())).ReturnsAsync(false);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Create_ReturnStatus500_UserNotExist() {
        RecipeRatingDTO toAdd = new() {
            RecipeId = 1,
            UserId = 11,
            RatingValue = 3,
            RatingContent = "content"
        };
        _mockManager.Setup(x => x.CreateRating(It.IsAny<RecipeRatingDTO>())).ReturnsAsync(false);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Create_ReturnStatus400_InvalidRecipeId() {
        RecipeRatingDTO toAdd = new() {
            RecipeId = -1,
            UserId = 1,
            RatingValue = 3,
            RatingContent = "content"
        };
        _mockManager.Setup(x => x.CreateRating(It.IsAny<RecipeRatingDTO>())).ReturnsAsync(true);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus400_InvalidUserId() {
        RecipeRatingDTO toAdd = new() {
            RecipeId = 1,
            UserId = -1,
            RatingValue = 3,
            RatingContent = "content"
        };
        _mockManager.Setup(x => x.CreateRating(It.IsAny<RecipeRatingDTO>())).ReturnsAsync(true);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus400_InvalidRatingLowerValue() {
        RecipeRatingDTO toAdd = new() {
            RecipeId = -1,
            UserId = 1,
            RatingValue = 3,
            RatingContent = "content"
        };
        _mockManager.Setup(x => x.CreateRating(It.IsAny<RecipeRatingDTO>())).ReturnsAsync(true);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus400_InvalidRatingUpperValue() {
        RecipeRatingDTO toAdd = new() {
            RecipeId = -1,
            UserId = 1,
            RatingValue = 6,
            RatingContent = "content"
        };
        _mockManager.Setup(x => x.CreateRating(It.IsAny<RecipeRatingDTO>())).ReturnsAsync(true);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus200_AllValid() {
        RecipeRatingDTO toUpdate = new() {
            RatingId = 1,
            RatingValue = 3,
            RatingContent = "new comment content"
        };
        _mockManager.Setup(x => x.UpdateRating(It.IsAny<RecipeRatingDTO>())).ReturnsAsync(true);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Update_ReturnStatus200_ReviewNotExist() {
        RecipeRatingDTO toUpdate = new() {
            RatingId = 6,
            RatingValue = 3,
            RatingContent = "new comment content"
        };
        _mockManager.Setup(x => x.UpdateRating(It.IsAny<RecipeRatingDTO>())).ReturnsAsync(false);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Update_ReturnStatus400_InvalidRatingId() {
        RecipeRatingDTO toUpdate = new() {
            RatingId = -1,
            RatingValue = 3,
            RatingContent = "new comment content"
        };
        _mockManager.Setup(x => x.UpdateRating(It.IsAny<RecipeRatingDTO>())).ReturnsAsync(false);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_InvalidRatingLowerValue() {
        RecipeRatingDTO toUpdate = new() {
            RatingId = 1,
            RatingValue = -1,
            RatingContent = "new comment content"
        };
        _mockManager.Setup(x => x.UpdateRating(It.IsAny<RecipeRatingDTO>())).ReturnsAsync(false);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_InvalidRatingUpperValue() {
        RecipeRatingDTO toUpdate = new() {
            RatingId = 1,
            RatingValue = 6,
            RatingContent = "new comment content"
        };
        _mockManager.Setup(x => x.UpdateRating(It.IsAny<RecipeRatingDTO>())).ReturnsAsync(false);

        ReviewController _controller = new ReviewController(_configuration, _mockManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }
}
