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

public class NewsControllerTest
{
    private Mock<INewsManager> _mockNewsManager;
    private IConfiguration _configuration;

    public NewsControllerTest() {
        _mockNewsManager = new Mock<INewsManager>();
        _configuration = new ConfigurationBuilder().Build();
    }

    [Fact]
    public async void Get_ReturnStatus200_NullNewsId() {
        _mockNewsManager.Setup(x => x.GetNews()).ReturnsAsync(new List<NewsDTO>(){
            new NewsDTO() {
                NewsId = 1,
                NewsStatus = 1
            },
            new NewsDTO() {
                NewsId = 2,
                NewsStatus = 1
            },
            new NewsDTO() {
                NewsId = 3,
                NewsStatus = 1
            },
        });

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Get(null);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus200_ValidNewsId() {
        _mockNewsManager.Setup(x => x.GetNews(1)).ReturnsAsync(new NewsDTO() {
            NewsId = 1,
            NewsStatus = 1
        });

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Get(1);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus404_NewsNotExist() {
        _mockNewsManager.Setup(x => x.GetNews(0));

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Get(0);

        result.Should().BeNotFoundObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus400_InvalidNewsId() {
        _mockNewsManager.Setup(x => x.GetNews(-1));

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Get(-1);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Search_ReturnStatus200_ValidSearchString() {
        _mockNewsManager.Setup(x => x.SearchNews("search string")).ReturnsAsync(new List<NewsDTO>(){
            new NewsDTO() {
                NewsId = 1,
                NewsStatus = 1
            },
            new NewsDTO() {
                NewsId = 2,
                NewsStatus = 1
            },
            new NewsDTO() {
                NewsId = 3,
                NewsStatus = 1
            },
        });

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Search("search string");

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Search_ReturnStatus200_EmptyString() {
        _mockNewsManager.Setup(x => x.SearchNews(string.Empty)).ReturnsAsync(new List<NewsDTO>(){
            new NewsDTO() {
                NewsId = 1,
                NewsStatus = 1
            },
            new NewsDTO() {
                NewsId = 2,
                NewsStatus = 1
            },
            new NewsDTO() {
                NewsId = 3,
                NewsStatus = 1
            },
        });

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Search(string.Empty);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Lastest_ReturnStatus200_ValidCount() {
        _mockNewsManager.Setup(x => x.GetNewsLastest(3)).ReturnsAsync(new List<NewsDTO>(){
            new NewsDTO() {
                NewsId = 1,
                NewsStatus = 1
            },
            new NewsDTO() {
                NewsId = 2,
                NewsStatus = 1
            },
            new NewsDTO() {
                NewsId = 3,
                NewsStatus = 1
            },
        });

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Lastest(3);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Lastest_ReturnStatus200_BoundaryCount() {
        _mockNewsManager.Setup(x => x.GetNewsLastest(0)).ReturnsAsync(new List<NewsDTO>());

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Lastest(0);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Lastest_ReturnStatus400_InvalidCount() {
        _mockNewsManager.Setup(x => x.GetNewsLastest(-1));

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Lastest(-1);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus200_AllValid() {
        NewsDTO toAdd = new() {
            UserId = 1,
            FeaturedImage = "image-link",
            NewsTitle = "title",
            NewsContent = "content"
        };
        _mockNewsManager.Setup(x => x.CreateNews(It.IsAny<NewsDTO>()));

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Create_ReturnStatus500_InvalidUserId() {
        NewsDTO toAdd = new() {
            UserId = -1,
            FeaturedImage = "image-link",
            NewsTitle = "title",
            NewsContent = "content"
        };
        _mockNewsManager.Setup(x => x.CreateNews(It.IsAny<NewsDTO>())).Throws(new Exception());

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Create_ReturnStatus400_EmptyTitle() {
        NewsDTO toAdd = new() {
            UserId = 1,
            FeaturedImage = "image-link",
            NewsTitle = string.Empty,
            NewsContent = "content"
        };
        _mockNewsManager.Setup(x => x.CreateNews(It.IsAny<NewsDTO>()));

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus400_EmptyImage() {
        NewsDTO toAdd = new() {
            UserId = 1,
            FeaturedImage = string.Empty,
            NewsTitle = "title",
            NewsContent = "content"
        };
        _mockNewsManager.Setup(x => x.CreateNews(It.IsAny<NewsDTO>()));

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus200_AllValid() {
        NewsDTO toUpdate = new() {
            NewsId = 1,
            FeaturedImage = "new image-link",
            NewsTitle = "new title",
            NewsContent = "new content"
        };
        _mockNewsManager.Setup(x => x.GetNews(toUpdate.NewsId.Value)).ReturnsAsync(new NewsDTO());
        _mockNewsManager.Setup(x => x.UpdateNews(It.IsAny<NewsDTO>())).ReturnsAsync(true);

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Update_ReturnStatus404_NewsNotExist() {
        NewsDTO toUpdate = new() {
            NewsId = 0,
            FeaturedImage = "new image-link",
            NewsTitle = "new title",
            NewsContent = "new content"
        };
        _mockNewsManager.Setup(x => x.GetNews(toUpdate.NewsId.Value));
        _mockNewsManager.Setup(x => x.UpdateNews(It.IsAny<NewsDTO>())).ReturnsAsync(false);

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeNotFoundObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_InvalidNewsId() {
        NewsDTO toUpdate = new() {
            NewsId = -1,
            FeaturedImage = "new image-link",
            NewsTitle = "new title",
            NewsContent = "new content"
        };
        _mockNewsManager.Setup(x => x.GetNews(toUpdate.NewsId.Value));
        _mockNewsManager.Setup(x => x.UpdateNews(It.IsAny<NewsDTO>())).ReturnsAsync(true);

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_EmptyImage() {
        NewsDTO toUpdate = new() {
            NewsId = 1,
            FeaturedImage = string.Empty,
            NewsTitle = "new title",
            NewsContent = "new content"
        };
        _mockNewsManager.Setup(x => x.GetNews(toUpdate.NewsId.Value)).ReturnsAsync(new NewsDTO());
        _mockNewsManager.Setup(x => x.UpdateNews(It.IsAny<NewsDTO>())).ReturnsAsync(true);

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_EmptyTitle() {
        NewsDTO toUpdate = new() {
            NewsId = 1,
            FeaturedImage = "new image-link",
            NewsTitle = string.Empty,
            NewsContent = "new content"
        };
        _mockNewsManager.Setup(x => x.GetNews(toUpdate.NewsId.Value)).ReturnsAsync(new NewsDTO());
        _mockNewsManager.Setup(x => x.UpdateNews(It.IsAny<NewsDTO>())).ReturnsAsync(true);

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Delete_ReturnStatus200_ValidNewsId() {
        _mockNewsManager.Setup(x => x.GetNews(1)).ReturnsAsync(new NewsDTO());
        _mockNewsManager.Setup(x => x.DeleteNews(1)).ReturnsAsync(true);

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Delete(1);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Delete_ReturnStatus404_NewsNotExist() {
        _mockNewsManager.Setup(x => x.GetNews(0));
        _mockNewsManager.Setup(x => x.DeleteNews(0)).ReturnsAsync(false);

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Delete(0);

        result.Should().BeNotFoundObjectResult();
    }

    [Fact]
    public async void Delete_ReturnStatus400_InvalidNewsId() {
        _mockNewsManager.Setup(x => x.GetNews(-1)).ReturnsAsync(new NewsDTO());
        _mockNewsManager.Setup(x => x.DeleteNews(-1)).ReturnsAsync(true);

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Delete(-1);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Accept_ReturnStatus200_ValidNewsId() {
        _mockNewsManager.Setup(x => x.Accept(1)).ReturnsAsync(true);

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Accept(1);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Accept_ReturnStatus500_NewsNotExist() {
        _mockNewsManager.Setup(x => x.Accept(0)).ReturnsAsync(false);

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Accept(0);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Accept_ReturnStatus400_InvalidNewsId() {
        _mockNewsManager.Setup(x => x.Accept(-1)).ReturnsAsync(false);

        NewsController _controller = new(_configuration, _mockNewsManager.Object);
        var result = await _controller.Accept(-1);

        result.Should().BeBadRequestObjectResult();
    }
}
