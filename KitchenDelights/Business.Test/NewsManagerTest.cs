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

public class NewsManagerTest
{
    private Mock<INewsRepository> _mockNewsRepository;
    private IMapper _mapper;

    public NewsManagerTest() {
        _mockNewsRepository = new Mock<INewsRepository>();
        _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<NewsProfile>();
            }));
    }

    [Fact]
    public async void GetNews_GetNewsById_NewsExistInRepo() {
        var news = GetNews();
        _mockNewsRepository.Setup(x => x.GetNews(1)).ReturnsAsync(news.FirstOrDefault(x => x.NewsId == 1));

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.GetNews(1);
        var actual = news.FirstOrDefault(x => x.NewsId == 1);

        actual.Should().NotBeNull();
        result.Should().BeOfType<NewsDTO>()
        .And.BeEquivalentTo(_mapper.Map<News, NewsDTO>(actual!));
    }

    [Fact]
    public async void GetNews_GetNewsById_NewsNotExistInRepo() {
        var news = GetNews();
        _mockNewsRepository.Setup(x => x.GetNews(-1)).ReturnsAsync(news.FirstOrDefault(x => x.NewsId == -1));

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.GetNews(-1);
        var actual = news.FirstOrDefault(x => x.NewsId == -1);

        actual.Should().BeNull();
        result.Should().BeNull();
    }

    [Fact]
    public async void GetNews_GetNewsList() {
        var news = GetNews();
        _mockNewsRepository.Setup(x => x.GetNews()).ReturnsAsync(news.OrderByDescending(x => x.CreateDate).ToList());

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.GetNews();

        result.Should().BeOfType<List<NewsDTO>>()
        .And.NotBeNullOrEmpty()
        .And.BeInDescendingOrder(x => x.CreateDate);
        result.Count.Should().Be(4);
    }

    [Fact]
    public async void SearchNews_GetNewsList_SearchStringInTitle() {
        var news = GetNews();
        _mockNewsRepository.Setup(x => x.GetNews()).ReturnsAsync(news.OrderByDescending(x => x.CreateDate).ToList());

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.SearchNews("title");

        result.Should().BeOfType<List<NewsDTO>>()
        .And.NotBeNullOrEmpty()
        .And.BeInDescendingOrder(x => x.CreateDate);
        result.Count.Should().Be(4);
    }

    [Fact]
    public async void SearchNews_GetNewsList_SearchStringInContent() {
        var news = GetNews();
        _mockNewsRepository.Setup(x => x.GetNews()).ReturnsAsync(news.OrderByDescending(x => x.CreateDate).ToList());

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.SearchNews("search");

        result.Should().BeOfType<List<NewsDTO>>()
        .And.NotBeNullOrEmpty()
        .And.BeInDescendingOrder(x => x.CreateDate);
        result.Count.Should().Be(2);
    }

    [Fact]
    public async void GetNewsLastest_GetNewsList_NormalCount() {
        var news = GetNews();
        _mockNewsRepository.Setup(x => x.GetNews()).ReturnsAsync(news.OrderByDescending(x => x.CreateDate).ToList());

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.GetNewsLastest(3);

        result.Should().BeOfType<List<NewsDTO>>()
        .And.NotBeNullOrEmpty()
        .And.BeInDescendingOrder(x => x.CreateDate);
        result.Count.Should().Be(3);
    }

    [Fact]
    public async void GetNewsLastest_GetNewsList_BoundaryCount() {
        var news = GetNews();
        _mockNewsRepository.Setup(x => x.GetNews()).ReturnsAsync(news.OrderByDescending(x => x.CreateDate).ToList());

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.GetNewsLastest(0);

        result.Should().BeOfType<List<NewsDTO>>()
        .And.BeEmpty();
    }

    [Fact]
    public async void GetNewsLastest_GetNewsList_AbnormalCount() {
        var news = GetNews();
        _mockNewsRepository.Setup(x => x.GetNews()).ReturnsAsync(news.OrderByDescending(x => x.CreateDate).ToList());

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.GetNewsLastest(-1);

        result.Should().BeOfType<List<NewsDTO>>()
        .And.BeEmpty();
    }

    [Fact]
    public void CreateNews_CreateNews_AllValid() {
        var news = GetNews();
        NewsDTO toAdd = new() {
            UserId = 1,
            FeaturedImage = "Test add image",
            NewsTitle = "Test add title",
            NewsContent = "Test add content",
            CreateDate = DateTime.Now.AddDays(1)
        };
        _mockNewsRepository.Setup(x => x.CreateNews(It.IsAny<News>())).Callback(news.Add);

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);   
        _newsManager.CreateNews(toAdd);

        news.Count.Should().Be(5);
        _mockNewsRepository.Verify(x => x.CreateNews(It.IsAny<News>()), Times.Once);
    }

    [Fact]
    public void CreateNews_CreateNews_InvalidUser() {
        var news = GetNews();
        NewsDTO toAdd = new() {
            UserId = -1,
            FeaturedImage = "Test add image",
            NewsTitle = "Test add title",
            NewsContent = "Test add content",
            CreateDate = DateTime.Now.AddDays(1)
        };
        _mockNewsRepository.Setup(x => x.CreateNews(It.IsAny<News>()));

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);   
        _newsManager.CreateNews(toAdd);

        news.Count.Should().Be(4);
        _mockNewsRepository.Verify(x => x.CreateNews(It.IsAny<News>()), Times.Once);
    }

    [Fact]
    public async void UpdateNews_UpdateExistingNews_NewsExistInRepo(){
        var news = GetNews();
        NewsDTO toUpdate = new() {
            NewsId = 1,
            FeaturedImage = "Test update image",
            NewsTitle = "Test update title",
            NewsContent = "Test update content"
        };
        _mockNewsRepository.Setup(x => x.GetNews(1)).ReturnsAsync(news.FirstOrDefault(x => x.NewsId == 1));
        _mockNewsRepository.Setup(x => x.UpdateNews(It.IsAny<News>())).Callback<News>((newsEntity) => news[0] = newsEntity);

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.UpdateNews(toUpdate);
        var actual = news.FirstOrDefault(x => x.NewsId == 1);

        result.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.FeaturedImage.Should().BeEquivalentTo(toUpdate.FeaturedImage);
        actual!.NewsTitle.Should().BeEquivalentTo(toUpdate.NewsTitle);
        actual!.NewsContent.Should().BeEquivalentTo(toUpdate.NewsContent);
        actual!.NewsStatus.Should().Be(2);
    }

    [Fact]
    public async void UpdateNews_NotUpdateExistingNews_NewsNotExistInRepo(){
        var news = GetNews();
        NewsDTO toUpdate = new() {
            NewsId = -1,
            FeaturedImage = "Test update image",
            NewsTitle = "Test update title",
            NewsContent = "Test update content"
        };
        _mockNewsRepository.Setup(x => x.GetNews(-1)).ReturnsAsync(news.FirstOrDefault(x => x.NewsId == -1));
        _mockNewsRepository.Setup(x => x.UpdateNews(It.IsAny<News>())).Callback<News>((newsEntity) => news[-1] = newsEntity);

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.UpdateNews(toUpdate);
        var actual = news.FirstOrDefault(x => x.NewsId == -1);

        result.Should().BeFalse();
        actual.Should().BeNull();
    }
    
    [Fact]
    public async void DeleteNews_DeleteExistingNews_NewsExistInRepo()
    {
        var news = GetNews();
        _mockNewsRepository.Setup(x => x.GetNews(1)).ReturnsAsync(news.FirstOrDefault(x => x.NewsId == 1));
        _mockNewsRepository.Setup(x => x.UpdateNews(It.IsAny<News>())).Callback<News>((newsEntity) => news[0] = newsEntity);

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.DeleteNews(1);
        var actual = news.FirstOrDefault(x => x.NewsId == 1);

        result.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.NewsStatus.Should().Be(0);
    }

    [Fact]
    public async void DeleteNews_NotDeleteExistingNews_NewsNotExistInRepo()
    {
        var news = GetNews();
        _mockNewsRepository.Setup(x => x.GetNews(-1)).ReturnsAsync(news.FirstOrDefault(x => x.NewsId == -1));
        _mockNewsRepository.Setup(x => x.UpdateNews(It.IsAny<News>())).Callback<News>((newsEntity) => news[-1] = newsEntity);

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.DeleteNews(-1);
        var actual = news.FirstOrDefault(x => x.NewsId == -1);

        result.Should().BeFalse();
        actual.Should().BeNull();
    }

    [Fact]
    public async void Accept_UpdateExsitingNewsStatus_NewsExistInRepo() {
        var news = GetNews();
        news[0].NewsStatus = 2;
        _mockNewsRepository.Setup(x => x.GetNews(1)).ReturnsAsync(news.FirstOrDefault(x => x.NewsId == 1));
        _mockNewsRepository.Setup(x => x.UpdateNews(It.IsAny<News>())).Callback<News>((newsEntity) => news[0] = newsEntity);

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.Accept(1);
        var actual = news.FirstOrDefault(x => x.NewsId == 1);

        result.Should().BeTrue();
        actual.Should().NotBeNull();
        actual!.NewsStatus.Should().NotBe(2)
        .And.Be(1);
    }

    [Fact]
    public async void Accept_NotUpdateExsitingNewsStatus_NewsNotExistInRepo() {
        var news = GetNews();
        _mockNewsRepository.Setup(x => x.GetNews(-1)).ReturnsAsync(news.FirstOrDefault(x => x.NewsId == -1));
        _mockNewsRepository.Setup(x => x.UpdateNews(It.IsAny<News>())).Callback<News>((newsEntity) => news[-1] = newsEntity);

        INewsManager _newsManager = new NewsManager(_mockNewsRepository.Object, _mapper);
        var result = await _newsManager.Accept(-1);
        var actual = news.FirstOrDefault(x => x.NewsId == -1);

        result.Should().BeFalse();
        actual.Should().BeNull();
    }

    private static List<News> GetNews() {
        List<News> output = [
            new News() {
                NewsId = 1,
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    FirstName = "firstname1",
                    MiddleName = "middlename1",
                    LastName = "lastname1"
                },
                FeaturedImage = "image-link-1",
                NewsTitle = "mock title 1",
                NewsContent = "mock content 1",
                NewsStatus = 1,
                CreateDate = DateTime.Now
            },
            new News() {
                NewsId = 2,
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    FirstName = "firstname1",
                    MiddleName = "middlename1",
                    LastName = "lastname1"
                },
                FeaturedImage = "image-link-2",
                NewsTitle = "mock title 2",
                NewsContent = "mock content 2",
                NewsStatus = 1,
                CreateDate = DateTime.Now.AddHours(1)
            },
            new News() {
                NewsId = 3,
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    FirstName = "firstname1",
                    MiddleName = "middlename1",
                    LastName = "lastname1"
                },
                FeaturedImage = "image-link-3",
                NewsTitle = "mock title 3",
                NewsContent = "mock search content 3",
                NewsStatus = 1,
                CreateDate = DateTime.Now.AddHours(2)
            },
            new News() {
                NewsId = 4,
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    FirstName = "firstname1",
                    MiddleName = "middlename1",
                    LastName = "lastname1"
                },
                FeaturedImage = "image-link-4",
                NewsTitle = "mock title 4",
                NewsContent = "mock search content 4",
                NewsStatus = 1,
                CreateDate = DateTime.Now.AddHours(3)
            },
        ];
        return output;
    }
}
