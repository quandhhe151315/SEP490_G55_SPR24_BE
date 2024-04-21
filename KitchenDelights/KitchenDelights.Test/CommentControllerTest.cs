using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.DTO;
using Business.Interfaces;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using KitchenDelights.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace KitchenDelights.Test;

public class CommentControllerTest
{
    private IConfiguration _configuration;
    private Mock<ICommentManager> _mockCommentManager;

    public CommentControllerTest() {
        _configuration = new ConfigurationBuilder().Build();
        _mockCommentManager = new Mock<ICommentManager>();
    }

    [Fact]
    public async void Get_ReturnStatus200_NullBlogId() {
        _mockCommentManager.Setup(x => x.GetComments()).ReturnsAsync([]);

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Get(null);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus200_ValidBlogId() {
        _mockCommentManager.Setup(x => x.GetComments(1)).ReturnsAsync([]);

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Get(1);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus400_InvalidBlogId() {
        _mockCommentManager.Setup(x => x.GetComments(-1));

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Get(-1);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus200_AllValid() {
        BlogCommentDTO toAdd = new() {
            BlogId = 1,
            UserId = 1,
            CommentContent = "comment content"
        };
        _mockCommentManager.Setup(x => x.CreateComment(It.IsAny<BlogCommentDTO>())).ReturnsAsync(1);

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus500_BlogNotExist() {
        BlogCommentDTO toAdd = new() {
            BlogId = 0,
            UserId = 1,
            CommentContent = "comment content"
        };
        _mockCommentManager.Setup(x => x.CreateComment(It.IsAny<BlogCommentDTO>())).Throws(new Exception());

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Create_ReturnStatus500_UserNotExist() {
        BlogCommentDTO toAdd = new() {
            BlogId = 1,
            UserId = 0,
            CommentContent = "comment content"
        };
        _mockCommentManager.Setup(x => x.CreateComment(It.IsAny<BlogCommentDTO>())).Throws(new Exception());

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Create_ReturnStatus500_ParentCommentNotExist() {
        BlogCommentDTO toAdd = new() {
            BlogId = 0,
            ParentId = 0,
            UserId = 1,
            CommentContent = "comment content"
        };
        _mockCommentManager.Setup(x => x.CreateComment(It.IsAny<BlogCommentDTO>())).Throws(new Exception());

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Create_ReturnStatus500_InvalidBlogId() {
        BlogCommentDTO toAdd = new() {
            BlogId = -1,
            UserId = 1,
            CommentContent = "comment content"
        };
        _mockCommentManager.Setup(x => x.CreateComment(It.IsAny<BlogCommentDTO>())).Throws(new Exception());

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus500_InvalidUserId() {
        BlogCommentDTO toAdd = new() {
            BlogId = 1,
            UserId = -1,
            CommentContent = "comment content"
        };
        _mockCommentManager.Setup(x => x.CreateComment(It.IsAny<BlogCommentDTO>())).Throws(new Exception());

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus500_InvalidParentCommentId() {
        BlogCommentDTO toAdd = new() {
            BlogId = 1,
            ParentId = -1,
            UserId = 1,
            CommentContent = "comment content"
        };
        _mockCommentManager.Setup(x => x.CreateComment(It.IsAny<BlogCommentDTO>())).Throws(new Exception());

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus200_AllValid() {
        BlogCommentDTO toUpdate = new() {
            CommentId = 1,
            CommentContent = "new comment"
        };
        _mockCommentManager.Setup(x => x.UpdateComment(It.IsAny<BlogCommentDTO>())).ReturnsAsync(true);

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Update_ReturnStatus500_CommentNotExist() {
        BlogCommentDTO toUpdate = new() {
            CommentId = 0,
            CommentContent = "new comment"
        };
        _mockCommentManager.Setup(x => x.UpdateComment(It.IsAny<BlogCommentDTO>())).ReturnsAsync(false);

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Update_ReturnStatus400_InvalidCommentId() {
        BlogCommentDTO toUpdate = new() {
            CommentId = -1,
            CommentContent = "new comment"
        };
        _mockCommentManager.Setup(x => x.UpdateComment(It.IsAny<BlogCommentDTO>())).ReturnsAsync(false);

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Delete_ReturnStatus200_ValidCommentId() {
        _mockCommentManager.Setup(x => x.DeleteComment(1)).ReturnsAsync(true);

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Delete(1);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Delete_ReturnStatus500_CommentNotExist() {
        _mockCommentManager.Setup(x => x.DeleteComment(0)).ReturnsAsync(false);

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Delete(0);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Delete_ReturnStatus400_ValidCommentId() {
        _mockCommentManager.Setup(x => x.DeleteComment(-1)).ReturnsAsync(true);

        CommentController _controller = new(_configuration, _mockCommentManager.Object);
        var result = await _controller.Delete(-1);

        result.Should().BeBadRequestObjectResult();
    }
}
