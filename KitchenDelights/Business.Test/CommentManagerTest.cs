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
using Moq;
using Xunit;

namespace Business.Test;

public class CommentManagerTest
{
    private Mock<ICommentRepository> _mockCommentRepository;

    private IMapper _mapper;

    public CommentManagerTest() {
        _mockCommentRepository = new Mock<ICommentRepository>();
        _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<BlogCommentProfile>();
            }));
    }

    [Fact]
    public async void GetComments_GetCommentList() 
    {
        var comments = GetComments();
        _mockCommentRepository.Setup(x => x.GetComments()).ReturnsAsync(comments.Where(x => x.CommentStatus != 0).ToList());

        ICommentManager _commentManager = new CommentManager(_mockCommentRepository.Object, _mapper);
        var result = await _commentManager.GetComments();

        result.Should().BeOfType<List<BlogCommentDTO>>()
        .And.NotBeNullOrEmpty();
        result.Count.Should().Be(5);
    }

    [Fact]
    public async void GetComments_GetCommentListByBlogId_ValidBlogId()
    {
        var comments = GetComments();
        _mockCommentRepository.Setup(x => x.GetComments(1)).ReturnsAsync(comments.Where(x => x.BlogId == 1 && x.ParentId == null && x.CommentStatus != 0).ToList());

        ICommentManager _commentManager = new CommentManager(_mockCommentRepository.Object, _mapper);
        var result = await _commentManager.GetComments(1);

        result.Should().BeOfType<List<BlogCommentDTO>>()
        .And.NotBeNullOrEmpty();
        result.Count.Should().Be(3);
    }

    [Fact]
    public async void GetComments_GetCommentListByBlogId_BoundaryBlogId()
    {
        var comments = GetComments();
        _mockCommentRepository.Setup(x => x.GetComments(0)).ReturnsAsync(comments.Where(x => x.BlogId == 0 && x.ParentId == null && x.CommentStatus != 0).ToList());

        ICommentManager _commentManager = new CommentManager(_mockCommentRepository.Object, _mapper);
        var result = await _commentManager.GetComments(0);

        result.Should().BeOfType<List<BlogCommentDTO>>()
        .And.BeEmpty();
    }

    [Fact]
    public async void GetComments_GetCommentListByBlogId_InvalidBlogId()
    {
        var comments = GetComments();
        _mockCommentRepository.Setup(x => x.GetComments(-1)).ReturnsAsync(comments.Where(x => x.BlogId == -1 && x.ParentId == null && x.CommentStatus != 0).ToList());

        ICommentManager _commentManager = new CommentManager(_mockCommentRepository.Object, _mapper);
        var result = await _commentManager.GetComments(-1);

        result.Should().BeOfType<List<BlogCommentDTO>>()
        .And.BeEmpty();
    }

    [Fact]
    public async void CreateComment_CreateNewComment_AllValid()
    {
        var comments = GetComments();
        BlogCommentDTO toAdd = new() {
            BlogId = 1,
            ParentId = null,
            UserId = 1,
            CommentContent = "test add comment",
            CommentStatus = 1,
            CreateDate = DateTime.Now.AddHours(5)
        };
        _mockCommentRepository.Setup(x => x.CreateComment(It.IsAny<BlogComment>())).Callback(comments.Add);

        ICommentManager _commentManager = new CommentManager(_mockCommentRepository.Object, _mapper);
        _commentManager.CreateComment(toAdd);

        comments.Count.Should().Be(6);
        _mockCommentRepository.Verify(x => x.CreateComment(It.IsAny<BlogComment>()), Times.Once);
    }

    [Fact]
    public async void CreateComment_CreateNewComment_InvalidBlogId()
    {
        var comments = GetComments();
        BlogCommentDTO toAdd = new() {
            BlogId = -1,
            ParentId = null,
            UserId = 1,
            CommentContent = "test add comment",
            CommentStatus = 1,
            CreateDate = DateTime.Now.AddHours(5)
        };
        _mockCommentRepository.Setup(x => x.CreateComment(It.IsAny<BlogComment>()));

        ICommentManager _commentManager = new CommentManager(_mockCommentRepository.Object, _mapper);
        _commentManager.CreateComment(toAdd);

        comments.Count.Should().Be(5);
        _mockCommentRepository.Verify(x => x.CreateComment(It.IsAny<BlogComment>()), Times.Once);
    }

    [Fact]
    public async void CreateComment_CreateNewComment_InvalidParentCommentId()
    {
        var comments = GetComments();
        BlogCommentDTO toAdd = new() {
            BlogId = 1,
            ParentId = -1,
            UserId = 1,
            CommentContent = "test add comment",
            CommentStatus = 1,
            CreateDate = DateTime.Now.AddHours(5)
        };
        _mockCommentRepository.Setup(x => x.CreateComment(It.IsAny<BlogComment>()));

        ICommentManager _commentManager = new CommentManager(_mockCommentRepository.Object, _mapper);
        _commentManager.CreateComment(toAdd);

        comments.Count.Should().Be(5);
        _mockCommentRepository.Verify(x => x.CreateComment(It.IsAny<BlogComment>()), Times.Once);
    }

    [Fact]
    public async void CreateComment_CreateNewComment_InvalidUserId()
    {
        var comments = GetComments();
        BlogCommentDTO toAdd = new() {
            BlogId = 1,
            ParentId = null,
            UserId = -1,
            CommentContent = "test add comment",
            CommentStatus = 1,
            CreateDate = DateTime.Now.AddHours(5)
        };
        _mockCommentRepository.Setup(x => x.CreateComment(It.IsAny<BlogComment>()));

        ICommentManager _commentManager = new CommentManager(_mockCommentRepository.Object, _mapper);
        _commentManager.CreateComment(toAdd);

        comments.Count.Should().Be(5);
        _mockCommentRepository.Verify(x => x.CreateComment(It.IsAny<BlogComment>()), Times.Once);
    }

    [Fact]
    public async void UpdateComment_UpdateExistingComment_CommentExistInRepo()
    {
        var comments = GetComments();
        BlogCommentDTO toUpdate = new() {
            CommentId = 2,
            CommentContent = "test update comment",
        };
        _mockCommentRepository.Setup(x => x.GetComment(2)).ReturnsAsync(comments.FirstOrDefault(x => x.CommentId == 2 && x.CommentStatus != 0));
        _mockCommentRepository.Setup(x => x.UpdateComment(It.IsAny<BlogComment>())).Callback<BlogComment>(comment => comments[1] = comment);

        ICommentManager _commentManager = new CommentManager(_mockCommentRepository.Object, _mapper);
        var boolResult = await _commentManager.UpdateComment(toUpdate);
        var actual = comments.FirstOrDefault(x => x.CommentId == 2 && x.CommentStatus != 0);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.CommentContent.Should().BeEquivalentTo(toUpdate.CommentContent);
    }

    [Fact]
    public async void UpdateComment_NotUpdateExistingComment_CommentNotExistInRepo()
    {
        var comments = GetComments();
        BlogCommentDTO toUpdate = new() {
            CommentId = -1,
            CommentContent = "test update comment",
        };
        _mockCommentRepository.Setup(x => x.GetComment(-1)).ReturnsAsync(comments.FirstOrDefault(x => x.CommentId == -1 && x.CommentStatus != 0));
        _mockCommentRepository.Setup(x => x.UpdateComment(It.IsAny<BlogComment>())).Callback<BlogComment>(comment => comments[-1] = comment);

        ICommentManager _commentManager = new CommentManager(_mockCommentRepository.Object, _mapper);
        var boolResult = await _commentManager.UpdateComment(toUpdate);
        var actual = comments.FirstOrDefault(x => x.CommentId == -1 && x.CommentStatus != 0);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void DeleteComment_DeleteExistingComment_CommentExistInRepo()
    {
        var comments = GetComments();
        _mockCommentRepository.Setup(x => x.GetComment(2)).ReturnsAsync(comments.FirstOrDefault(x => x.CommentId == 2 && x.CommentStatus != 0));
        _mockCommentRepository.Setup(x => x.UpdateComment(It.IsAny<BlogComment>())).Callback<BlogComment>(comment => comments[1] = comment);

        ICommentManager _commentManager = new CommentManager(_mockCommentRepository.Object, _mapper);
        var boolResult = await _commentManager.DeleteComment(2);
        var actual = comments.FirstOrDefault(x => x.CommentId == 2);

        boolResult.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.CommentStatus.Should().Be(0);
    }

    [Fact]
    public async void DeleteComment_NotDeleteExistingComment_CommentNotExistInRepo()
    {
        var comments = GetComments();
        _mockCommentRepository.Setup(x => x.GetComment(-1)).ReturnsAsync(comments.FirstOrDefault(x => x.CommentId == -1 && x.CommentStatus != 0));
        _mockCommentRepository.Setup(x => x.UpdateComment(It.IsAny<BlogComment>())).Callback<BlogComment>(comment => comments[-1] = comment);

        ICommentManager _commentManager = new CommentManager(_mockCommentRepository.Object, _mapper);
        var boolResult = await _commentManager.DeleteComment(-1);
        var actual = comments.FirstOrDefault(x => x.CommentId == -1);

        boolResult.Should().BeFalse();
        actual.Should().BeNull();
    }

    private List<BlogComment> GetComments() {
        List<BlogComment> output = [
            new BlogComment() {
                CommentId = 1,
                BlogId = 1,
                ParentId = null,
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    FirstName = "firstname1",
                    MiddleName = "middlename1",
                    LastName = "lastname1"
                },
                CommentContent = "comment content 1",
                CommentStatus = 1,
                CreateDate = DateTime.Now,
                InverseParent = [
                    new BlogComment() {
                CommentId = 4,
                BlogId = 1,
                ParentId = 1,
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    FirstName = "firstname1",
                    MiddleName = "middlename1",
                    LastName = "lastname1"
                },
                CommentContent = "comment content 4",
                CommentStatus = 1,
                CreateDate = DateTime.Now.AddHours(3)
            },
            new BlogComment() {
                CommentId = 5,
                BlogId = 1,
                ParentId = 1,
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    FirstName = "firstname1",
                    MiddleName = "middlename1",
                    LastName = "lastname1"
                },
                CommentContent = "comment content 5",
                CommentStatus = 1,
                CreateDate = DateTime.Now.AddHours(4)
            },
                ]
            },
            new BlogComment() {
                CommentId = 2,
                BlogId = 1,
                ParentId = null,
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    FirstName = "firstname1",
                    MiddleName = "middlename1",
                    LastName = "lastname1"
                },
                CommentContent = "comment content 2",
                CommentStatus = 1,
                CreateDate = DateTime.Now.AddHours(1)
            },
            new BlogComment() {
                CommentId = 3,
                BlogId = 1,
                ParentId = null,
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    FirstName = "firstname1",
                    MiddleName = "middlename1",
                    LastName = "lastname1"
                },
                CommentContent = "comment content 3",
                CommentStatus = 1,
                CreateDate = DateTime.Now.AddHours(2)
            },
            new BlogComment() {
                CommentId = 4,
                BlogId = 1,
                ParentId = 1,
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    FirstName = "firstname1",
                    MiddleName = "middlename1",
                    LastName = "lastname1"
                },
                CommentContent = "comment content 4",
                CommentStatus = 1,
                CreateDate = DateTime.Now.AddHours(3)
            },
            new BlogComment() {
                CommentId = 5,
                BlogId = 1,
                ParentId = 1,
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    FirstName = "firstname1",
                    MiddleName = "middlename1",
                    LastName = "lastname1"
                },
                CommentContent = "comment content 5",
                CommentStatus = 1,
                CreateDate = DateTime.Now.AddHours(4)
            },
        ];
        return output;
    }
    
}
