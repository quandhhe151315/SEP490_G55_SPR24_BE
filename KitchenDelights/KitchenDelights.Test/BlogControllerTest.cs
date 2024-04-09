using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using KitchenDelights.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace KitchenDelights.Test;

public class BlogControllerTest
{
    private Mock<IBlogManager> _mockBlogManager;
    private IConfiguration _configuration;

    public BlogControllerTest() {
        _mockBlogManager = new Mock<IBlogManager>();
        _configuration = new ConfigurationBuilder().Build();

    }

    [Fact]
    public async void Get_ReturnStatus200_ValidBlogId() {
        _mockBlogManager.Setup(x => x.GetBlog(1)).ReturnsAsync(new BlogDTO(){
            BlogId = 1,
            BlogTitle = "title",
            BlogContent = "content"
        });

        BlogController _controller = new BlogController(_configuration, _mockBlogManager.Object);
        var result = await _controller.Get(1, null, null, null, null);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus404_BoundaryBlogId() {
        _mockBlogManager.Setup(x => x.GetBlog(0));

        BlogController _controller = new BlogController(_configuration, _mockBlogManager.Object);
        var result = await _controller.Get(0, null, null, null, null);

        result.Should().BeNotFoundObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus400_InvalidBlogId() {
        _mockBlogManager.Setup(x => x.GetBlog(-1)).ReturnsAsync(new BlogDTO(){
            BlogId = 1,
            BlogTitle = "title",
            BlogContent = "content"
        });

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Get(-1, null, null, null, null);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus200_AllValid() {
        _mockBlogManager.Setup(x => x.GetBlogs("search", 1, "desc")).ReturnsAsync([
            new BlogDTO() {
                BlogId = 1,
                BlogStatus = 1
            },
            new BlogDTO() {
                BlogId = 2,
                BlogStatus = 1
            },
            new BlogDTO() {
                BlogId = 3,
                BlogStatus = 2
            }
        ]);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Role, "User"),
        }, "mock"));

        BlogController _controller = new(_configuration, _mockBlogManager.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            }
        };
        var result = await _controller.Get(null, 1, "desc", "search", null);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus400_InvalidCategoryId() {
        _mockBlogManager.Setup(x => x.GetBlogs("search", -1, "desc")).ReturnsAsync([
            new BlogDTO() {
                BlogId = 1,
                BlogStatus = 1
            },
            new BlogDTO() {
                BlogId = 2,
                BlogStatus = 1
            },
            new BlogDTO() {
                BlogId = 3,
                BlogStatus = 2
            }
        ]);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Role, "User"),
        }, "mock"));

        BlogController _controller = new(_configuration, _mockBlogManager.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            }
        };
        var result = await _controller.Get(null, -1, "desc", "search", null);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus404_NoBlogExist() {
        _mockBlogManager.Setup(x => x.GetBlogs("search", 1, "desc")).ReturnsAsync([]);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Role, "User"),
        }, "mock"));

        BlogController _controller = new(_configuration, _mockBlogManager.Object)
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            }
        };
        var result = await _controller.Get(null, 1, "desc", "search", null);

        result.Should().BeNotFoundObjectResult();
    }

    [Fact]
    public async void Lastest_ReturnStatus200_ValidCount() {
        _mockBlogManager.Setup(x => x.GetBlogsLastest(2)).ReturnsAsync([
            new BlogDTO() {
                BlogId = 1,
                BlogStatus = 1
            },
            new BlogDTO() {
                BlogId = 2,
                BlogStatus = 1
            },
            new BlogDTO() {
                BlogId = 3,
                BlogStatus = 2
            }
        ]);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Lastest(2);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Lastest_ReturnStatus200_BoundaryCount() {
        _mockBlogManager.Setup(x => x.GetBlogsLastest(0)).ReturnsAsync([]);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Lastest(0);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Lastest_ReturnStatus400_InvalidCount() {
        _mockBlogManager.Setup(x => x.GetBlogsLastest(-1));

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Lastest(-1);

        result.Should().BeBadRequestResult();
    }

    [Fact]
    public async void Create_ReturnStatus200_AllValid() {
        BlogDTO toAdd = new() {
            UserId = 1,
            CategoryId = 1,
            BlogTitle = "title",
            BlogContent = "content",
            BlogImage = "image-link"           
        };
        _mockBlogManager.Setup(x => x.CreateBlog(It.IsAny<BlogDTO>()));
        
        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Create_ReturnStatus500_InvalidUserId() {
        BlogDTO toAdd = new() {
            UserId = -1,
            CategoryId = 1,
            BlogTitle = "title",
            BlogContent = "content",
            BlogImage = "image-link"           
        };
        _mockBlogManager.Setup(x => x.CreateBlog(It.IsAny<BlogDTO>())).Throws(new Exception());
        
        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Create_ReturnStatus500_InvalidCategoryId() {
        BlogDTO toAdd = new() {
            UserId = 1,
            CategoryId = -1,
            BlogTitle = "title",
            BlogContent = "content",
            BlogImage = "image-link"           
        };
        _mockBlogManager.Setup(x => x.CreateBlog(It.IsAny<BlogDTO>())).Throws(new Exception());
        
        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Create_ReturnStatus406_EmptyTitle() {
        BlogDTO toAdd = new() {
            UserId = 1,
            CategoryId = 1,
            BlogTitle = string.Empty,
            BlogContent = "content",
            BlogImage = "image-link"           
        };
        _mockBlogManager.Setup(x => x.CreateBlog(It.IsAny<BlogDTO>()));
        
        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(406);
    }

    [Fact]
    public async void Create_ReturnStatus406_EmptyImage() {
        BlogDTO toAdd = new() {
            UserId = 1,
            CategoryId = 1,
            BlogTitle = "title",
            BlogContent = "content",
            BlogImage = string.Empty          
        };
        _mockBlogManager.Setup(x => x.CreateBlog(It.IsAny<BlogDTO>()));
        
        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Create(toAdd);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(406);
    }

    [Fact]
    public async void Update_ReturnStatus200_AllValid() {
        BlogDTO toUpdate = new() {
            BlogId = 1,
            UserId = 1,
            CategoryId = 2,
            BlogTitle = "new title",
            BlogContent = "new content",
            BlogImage = "new-image"     
        };
        _mockBlogManager.Setup(x => x.UpdateBlog(It.IsAny<BlogDTO>())).ReturnsAsync(true);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Update_ReturnStatus500_BlogNotExist() {
        BlogDTO toUpdate = new() {
            BlogId = 0,
            UserId = 1,
            CategoryId = 2,
            BlogTitle = "new title",
            BlogContent = "new content",
            BlogImage = "new-image"     
        };
        _mockBlogManager.Setup(x => x.UpdateBlog(It.IsAny<BlogDTO>())).ReturnsAsync(false);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Update_ReturnStatus400_InvalidBlogId() {
        BlogDTO toUpdate = new() {
            BlogId = -1,
            UserId = 1,
            CategoryId = 2,
            BlogTitle = "new title",
            BlogContent = "new content",
            BlogImage = "new-image"     
        };
        _mockBlogManager.Setup(x => x.UpdateBlog(It.IsAny<BlogDTO>())).ReturnsAsync(false);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus406_EmtpyTitle() {
        BlogDTO toUpdate = new() {
            BlogId = 1,
            UserId = 1,
            CategoryId = 2,
            BlogTitle = string.Empty,
            BlogContent = "new content",
            BlogImage = "new-image"     
        };
        _mockBlogManager.Setup(x => x.UpdateBlog(It.IsAny<BlogDTO>())).ReturnsAsync(false);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(406);
    }

    [Fact]
    public async void Update_ReturnStatus406_EmtpyImage() {
        BlogDTO toUpdate = new() {
            BlogId = 1,
            UserId = 1,
            CategoryId = 2,
            BlogTitle = "new title",
            BlogContent = "new content",
            BlogImage = string.Empty    
        };
        _mockBlogManager.Setup(x => x.UpdateBlog(It.IsAny<BlogDTO>())).ReturnsAsync(false);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Update(toUpdate);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(406);
    }

    [Fact]
    public async void Status_ReturnStatus200_AllValid() {
        _mockBlogManager.Setup(x => x.BlogStatus(1, 1)).ReturnsAsync(true);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Status(1, 1);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Status_ReturnStatus500_BlogNotExist() {
        _mockBlogManager.Setup(x => x.BlogStatus(0, 1)).ReturnsAsync(false);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Status(0, 1);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Status_ReturnStatus400_InvalidBlogId() {
        _mockBlogManager.Setup(x => x.BlogStatus(-1, 1)).ReturnsAsync(false);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Status(-1, 1);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Status_ReturnStatus400_InvalidStatus() {
        _mockBlogManager.Setup(x => x.BlogStatus(1, 3)).ReturnsAsync(true);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Status(1, 3);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Delete_ReturnStatus200_BlogExist() {
        _mockBlogManager.Setup(x => x.DeleteBlog(1)).ReturnsAsync(true);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Delete(1);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Delete_ReturnStatus500_BlogNotExist() {
        _mockBlogManager.Setup(x => x.DeleteBlog(0)).ReturnsAsync(false);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Delete(0);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Delete_ReturnStatus400_InvalidBlogId() {
        _mockBlogManager.Setup(x => x.DeleteBlog(-1)).ReturnsAsync(false);

        BlogController _controller = new(_configuration, _mockBlogManager.Object);
        var result = await _controller.Delete(-1);

        result.Should().BeBadRequestObjectResult();
    }
}
