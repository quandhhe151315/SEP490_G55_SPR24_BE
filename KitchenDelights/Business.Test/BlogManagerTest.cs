using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Business.Profiles;
using Data.Entity;
using Data.Interfaces;
using FluentAssertions;
using Microsoft.Data.SqlClient;
using Moq;
using Xunit;

namespace Business.Test;

public class BlogManagerTest
{
    private readonly Mock<IBlogRepository> _blogRepositoryMock;
    private readonly IMapper _mapper;

    public BlogManagerTest() {
        _blogRepositoryMock = new Mock<IBlogRepository>();
        _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<BlogProfile>();
            }));
    }

    [Fact]
    public void CreateBlog_CreateNewBlog_AllValid()
    {
        var blogs = GetBlogs();
        BlogDTO toAdd = new() {
            UserId = 1,
            CategoryId = 1,
            BlogTitle = "Test Add Title",
            BlogContent = "Test Add Content",
            BlogImage = "Test Add Image",
            CreateDate = DateTime.Now
        };
        _blogRepositoryMock.Setup(x => x.CreateBlog(It.IsAny<Blog>())).Callback<Blog>(blogs.Add);

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        _blogManager.CreateBlog(toAdd);
        var result = blogs.Count;

        result.Should().Be(6);
        _blogRepositoryMock.Verify(x => x.CreateBlog(It.IsAny<Blog>()), Times.Once);
    }

    [Fact]
    public void CreateBlog_NotCreateNewBlog_InvalidUser()
    {
        var blogs = GetBlogs();
        BlogDTO toAdd = new() {
            UserId = -1,
            CategoryId = 1,
            BlogTitle = "Test Add Title",
            BlogContent = "Test Add Content",
            BlogImage = "Test Add Image",
            CreateDate = DateTime.Now
        };
        _blogRepositoryMock.Setup(x => x.CreateBlog(It.IsAny<Blog>()));

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        _blogManager.CreateBlog(toAdd);
        var result = blogs.Count;

        result.Should().Be(5);
        _blogRepositoryMock.Verify(x => x.CreateBlog(It.IsAny<Blog>()), Times.Once);
    }

    [Fact]
    public void CreateBlog_NotCreateNewBlog_InvalidCategory()
    {
        var blogs = GetBlogs();
        BlogDTO toAdd = new() {
            UserId = 1,
            CategoryId = -1,
            BlogTitle = "Test Add Title",
            BlogContent = "Test Add Content",
            BlogImage = "Test Add Image",
            CreateDate = DateTime.Now
        };
        _blogRepositoryMock.Setup(x => x.CreateBlog(It.IsAny<Blog>()));

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        _blogManager.CreateBlog(toAdd);
        var result = blogs.Count;

        result.Should().Be(5);
        _blogRepositoryMock.Verify(x => x.CreateBlog(It.IsAny<Blog>()), Times.Once);
    }

    [Fact]
    public async void UpdateBlog_UpdateExistingBlog_BlogExistInRepo()
    {
        var blogs = GetBlogs();
        BlogDTO toUpdate = new() {
            BlogId = 2,
            UserId = 1,
            CategoryId = 2,
            BlogTitle = "Test Update Title",
            BlogContent = "Test Update Content",
            BlogImage = "Test Update Image",
            CreateDate = DateTime.Now
        };
        _blogRepositoryMock.Setup(x => x.GetBlog(2)).ReturnsAsync(blogs.FirstOrDefault(x => x.BlogId == 2));
        _blogRepositoryMock.Setup(x => x.UpdateBlog(It.IsAny<Blog>())).Callback<Blog>((blog) => blogs[1] = blog);

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var boolResult = await _blogManager.UpdateBlog(toUpdate);
        var actual = blogs.FirstOrDefault(x => x.BlogId == toUpdate.BlogId);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.CategoryId.Should().Be(toUpdate.CategoryId);
        actual!.BlogTitle.Should().BeSameAs(toUpdate.BlogTitle);
        actual!.BlogContent.Should().BeSameAs(toUpdate.BlogContent);
        actual!.BlogImage.Should().BeSameAs(toUpdate.BlogImage);
    }

    [Fact]
    public async void UpdateBlog_NotUpdateBlog_BlogNotExistInRepo()
    {
        var blogs = GetBlogs();
        BlogDTO toUpdate = new() {
            BlogId = -1,
            UserId = 1,
            CategoryId = 2,
            BlogTitle = "Test Update Title",
            BlogContent = "Test Update Content",
            BlogImage = "Test Update Image",
            CreateDate = DateTime.Now
        };
        _blogRepositoryMock.Setup(x => x.GetBlog(-1)).ReturnsAsync(blogs.FirstOrDefault(x => x.BlogId == -1));
        _blogRepositoryMock.Setup(x => x.UpdateBlog(It.IsAny<Blog>())).Callback<Blog>((blog) => blogs[-1] = blog);

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var boolResult = await _blogManager.UpdateBlog(toUpdate);
        var actual = blogs.FirstOrDefault(x => x.BlogId == toUpdate.BlogId);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void DeleteBlog_DeleteExistingBlog_BlogExistInRepo()
    {
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlog(1)).ReturnsAsync(blogs.FirstOrDefault(x => x.BlogId == 1));
        _blogRepositoryMock.Setup(x => x.UpdateBlog(It.IsAny<Blog>())).Callback<Blog>((blog) => blogs[0] = blog);

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var boolResult = await _blogManager.DeleteBlog(1);
        var actual = blogs.FirstOrDefault(x => x.BlogId == 1);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.BlogStatus.Should().Be(0);
    }

    [Fact]
    public async void DeleteBlog_NotDeleteBlog_BlogNotInRepo()
    {
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlog(-1)).ReturnsAsync(blogs.FirstOrDefault(x => x.BlogId == -1));
        _blogRepositoryMock.Setup(x => x.UpdateBlog(It.IsAny<Blog>())).Callback<Blog>((blog) => blogs[-1] = blog);

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var boolResult = await _blogManager.DeleteBlog(-1);
        var actual = blogs.FirstOrDefault(x => x.BlogId == -1);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void GetBlog_GetBlogById_BlogExistInRepo()
    {
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlog(1)).ReturnsAsync(blogs.FirstOrDefault(x => x.BlogId == 1));

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlog(1);
        var actual = blogs.FirstOrDefault(x => x.BlogId == 1);

        actual.Should().NotBeNull();
        result.Should().NotBeNull()
        .And.BeOfType<BlogDTO>()
        .And.BeEquivalentTo(_mapper.Map<Blog, BlogDTO>(actual!));
    }

    [Fact]
    public async void GetBlog_GetBlogById_BlogNotExistInRepo()
    {
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlog(-1)).ReturnsAsync(blogs.FirstOrDefault(x => x.BlogId == -1));

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlog(-1);
        var actual = blogs.FirstOrDefault(x => x.BlogId == -1);

        result.Should().BeNull();
        actual.Should().BeNull();
    }

    [Fact]
    public async void GetBlog_GetBlogList_AllParameterAvailable(){
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlogs(4)).ReturnsAsync(blogs.Where(x => x.CategoryId == 4).ToList());

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlogs("content 4", 4, "asc");
        
        result.Should().BeOfType<List<BlogDTO>>()
        .And.Contain(x => x.CategoryId == 4)
        .And.Contain(x => x.BlogContent.Contains("content 4", StringComparison.InvariantCultureIgnoreCase))
        .And.BeInAscendingOrder(x => x.CreateDate);
    }

    [Fact]
    public async void GetBlog_GetBlogList_SearchParameterNull(){
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlogs(1)).ReturnsAsync(blogs.Where(x => x.CategoryId == 1).ToList());

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlogs(string.Empty, 1, "asc");
        
        result.Should().BeOfType<List<BlogDTO>>()
        .And.Contain(x => x.CategoryId == 1)
        .And.BeInAscendingOrder(x => x.CreateDate);
    }

    [Fact]
    public async void GetBlog_GetBlogList_CategoryParameterNull(){
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlogs()).ReturnsAsync(blogs.ToList());

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlogs("search", null, "asc");
        
        result.Should().BeOfType<List<BlogDTO>>()
        .And.Contain(x => x.BlogContent.Contains("search", StringComparison.InvariantCultureIgnoreCase))
        .And.BeInAscendingOrder(x => x.CreateDate);
    }

    [Fact]
    public async void GetBlog_GetBlogList_SortParameterNull(){
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlogs(1)).ReturnsAsync(blogs.Where(x => x.CategoryId == 1).ToList());

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlogs("content", 1, string.Empty);
        
        result.Should().BeOfType<List<BlogDTO>>()
        .And.Contain(x => x.CategoryId == 1)
        .And.Contain(x => x.BlogContent.Contains("content", StringComparison.InvariantCultureIgnoreCase))
        .And.BeInDescendingOrder(x => x.CreateDate);
    }

    [Fact]
    public async void GetBlog_GetBlogList_OnlyHaveSearch(){
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlogs()).ReturnsAsync(blogs.ToList());

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlogs("search", null, string.Empty);
        
        result.Should().BeOfType<List<BlogDTO>>()
        .And.Contain(x => x.BlogContent.Contains("search", StringComparison.InvariantCultureIgnoreCase))
        .And.BeInDescendingOrder(x => x.CreateDate);
    }

    [Fact]
    public async void GetBlog_GetBlogList_OnlyHaveCategory(){
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlogs(1)).ReturnsAsync(blogs.Where(x => x.CategoryId == 1).ToList());

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlogs(string.Empty, 1, string.Empty);
        
        result.Should().BeOfType<List<BlogDTO>>()
        .And.Contain(x => x.CategoryId == 1)
        .And.BeInDescendingOrder(x => x.CreateDate);
    }

    [Fact]
    public async void GetBlog_GetBlogList_OnlyHaveSort(){
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlogs()).ReturnsAsync(blogs.ToList());

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlogs(string.Empty, null, "asc");
        
        result.Should().BeOfType<List<BlogDTO>>()
        .And.BeInAscendingOrder(x => x.CreateDate);
    }

    [Fact]
    public async void GetBlog_GetBlogList_AllParameterNull(){
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlogs()).ReturnsAsync(blogs.ToList());

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlogs(string.Empty, null, string.Empty);
        
        result.Should().BeOfType<List<BlogDTO>>()
        .And.BeInDescendingOrder(x => x.CreateDate);
        result.Count.Should().Be(5);
    }

    [Fact]
    public async void GetBlog_GetBlogList_CategoryNotExist(){
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlogs(-1)).ReturnsAsync(blogs.Where(x => x.CategoryId == -1).ToList());

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlogs(string.Empty, -1, string.Empty);
        
        result.Should().BeOfType<List<BlogDTO>>();
        result.Count.Should().Be(0);
    }

    [Fact]
    public async void GetBlogsLastest_GetBlogList_ValidCount() {
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlogs()).ReturnsAsync(blogs.ToList());

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlogsLastest(3);

        result.Should().BeOfType<List<BlogDTO>>()
            .And.BeInDescendingOrder(x => x.CreateDate);
        result.Count.Should().Be(3);
    }

    [Fact]
    public async void GetBlogsLastest_GetBlogList_BoundaryCount() {
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlogs()).ReturnsAsync(blogs.ToList());

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlogsLastest(0);

        result.Should().BeOfType<List<BlogDTO>>()
            .And.BeInDescendingOrder(x => x.CreateDate);
        result.Count.Should().Be(0);
    }

    [Fact]
    public async void GetBlogsLastest_GetBlogList_InvalidCount() {
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlogs()).ReturnsAsync(blogs.ToList());

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var result = await _blogManager.GetBlogsLastest(-1);

        result.Should().BeOfType<List<BlogDTO>>()
            .And.BeInDescendingOrder(x => x.CreateDate);
        result.Count.Should().Be(0);
    }

    [Fact]
    public async void BlogStatus_UpdateBlogStatus_BlogExistInRepo() {
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlog(1)).ReturnsAsync(blogs.FirstOrDefault(x => x.BlogId == 1));
        _blogRepositoryMock.Setup(x => x.UpdateBlog(It.IsAny<Blog>())).Callback<Blog>((blog) => blogs[0] = blog);

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var boolResult = await _blogManager.BlogStatus(1, 2);
        var actual = blogs.FirstOrDefault(x => x.BlogId == 1);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.BlogStatus.Should().Be(2);
    }

    [Fact]
    public async void BlogStatus_NotUpdateBlogStatus_BlogNotExistInRepo() {
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlog(-1)).ReturnsAsync(blogs.FirstOrDefault(x => x.BlogId == -1));
        _blogRepositoryMock.Setup(x => x.UpdateBlog(It.IsAny<Blog>())).Callback<Blog>((blog) => blogs[-1] = blog);

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var boolResult = await _blogManager.BlogStatus(-1, 2);
        var actual = blogs.FirstOrDefault(x => x.BlogId == -1);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void BlogStatus_NotUpdateBlogStatus_InvalidStatus() {
        var blogs = GetBlogs();
        _blogRepositoryMock.Setup(x => x.GetBlog(1)).ReturnsAsync(blogs.FirstOrDefault(x => x.BlogId == 1));
        _blogRepositoryMock.Setup(x => x.UpdateBlog(It.IsAny<Blog>())).Callback<Blog>((blog) => blogs[0] = blog);

        IBlogManager _blogManager = new BlogManager(_blogRepositoryMock.Object, _mapper);
        var boolResult = await _blogManager.BlogStatus(1, -1);
        var actual = blogs.FirstOrDefault(x => x.BlogId == 1);

        boolResult.Should().BeFalse();
        actual.Should().NotBeNull();
        actual!.BlogStatus.Should().Be(1);
    }

    private static List<Blog> GetBlogs() {
        List<Blog> output = [
            new Blog() {
                BlogId = 1,
                UserId = 1,
                User = new User() {
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
                    Status = new Status() {
                        StatusId = 1,
                        StatusName = "Active"
                    },
                    RoleId = 1,
                    Role = new Role() {
                        RoleId = 1,
                        RoleName = "Admin"
                    },
                    Interaction = 0
                },
                CategoryId = 1,
                Category = new Category() {
                    CategoryId = 1,
                    CategoryName = "Mock Category Blog 1",
                    CategoryType = true,
                    CategoryStatus = 1
                },
                BlogTitle = "Mock Title 1",
                BlogContent = "Mock Content 1",
                BlogImage = "featured-image-blog1",
                BlogStatus = 1,
                CreateDate = DateTime.Now
            },
            new Blog() {
                BlogId = 2,
                UserId = 1,
                User = new User() {
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
                    Status = new Status() {
                        StatusId = 1,
                        StatusName = "Active"
                    },
                    RoleId = 1,
                    Role = new Role() {
                        RoleId = 1,
                        RoleName = "Admin"
                    },
                    Interaction = 0
                },
                CategoryId = 3,
                Category = new Category() {
                    CategoryId = 3,
                    CategoryName = "Mock Category Blog 3",
                    CategoryType = true,
                    CategoryStatus = 1
                },
                BlogTitle = "Mock Title 2",
                BlogContent = "Mock Content 2",
                BlogImage = "featured-image-blog2",
                BlogStatus = 1,
                CreateDate = DateTime.Now.AddHours(1)
            },
            new Blog() {
                BlogId = 3,
                UserId = 1,
                User = new User() {
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
                    Status = new Status() {
                        StatusId = 1,
                        StatusName = "Active"
                    },
                    RoleId = 1,
                    Role = new Role() {
                        RoleId = 1,
                        RoleName = "Admin"
                    },
                    Interaction = 0
                },
                CategoryId = 4,
                Category = new Category() {
                    CategoryId = 4,
                    CategoryName = "Mock Category Blog 4",
                    CategoryType = true,
                    CategoryStatus = 1
                },
                BlogTitle = "Mock Title 3",
                BlogContent = "Mock Content 3",
                BlogImage = "featured-image-blog3",
                BlogStatus = 1,
                CreateDate = DateTime.Now.AddHours(2)
            },
            new Blog() {
                BlogId = 4,
                UserId = 1,
                User = new User() {
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
                    Status = new Status() {
                        StatusId = 1,
                        StatusName = "Active"
                    },
                    RoleId = 1,
                    Role = new Role() {
                        RoleId = 1,
                        RoleName = "Admin"
                    },
                    Interaction = 0
                },
                CategoryId = 4,
                Category = new Category() {
                    CategoryId = 4,
                    CategoryName = "Mock Category Blog 4",
                    CategoryType = true,
                    CategoryStatus = 1
                },
                BlogTitle = "Mock Title 4",
                BlogContent = "Mock Search Content 4",
                BlogImage = "featured-image-blog4",
                BlogStatus = 1,
                CreateDate = DateTime.Now.AddHours(3)
            },
            new Blog() {
                BlogId = 5,
                UserId = 1,
                User = new User() {
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
                    Status = new Status() {
                        StatusId = 1,
                        StatusName = "Active"
                    },
                    RoleId = 1,
                    Role = new Role() {
                        RoleId = 1,
                        RoleName = "Admin"
                    },
                    Interaction = 0
                },
                CategoryId = 1,
                Category = new Category() {
                    CategoryId = 1,
                    CategoryName = "Mock Category Blog 1",
                    CategoryType = true,
                    CategoryStatus = 1
                },
                BlogTitle = "Mock Title 5",
                BlogContent = "Mock Search Content 5",
                BlogImage = "featured-image-blog5",
                BlogStatus = 1,
                CreateDate = DateTime.Now.AddHours(4)
            },
        ];
        return output;
    }

}
