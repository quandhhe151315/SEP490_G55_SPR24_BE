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

public class IngMarketplaceControllerTest
{
    private Mock<IIngredientMarketplaceManager> _mockManager;
    private IConfiguration _configuration;

    public IngMarketplaceControllerTest() {
        _mockManager = new Mock<IIngredientMarketplaceManager>();
        _configuration = new ConfigurationBuilder().Build();
    }

    [Fact]
    public async void Get_ReturnStatus200_NullId() {
        _mockManager.Setup(x => x.GetIngredientMarketplaces()).ReturnsAsync([]);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Get(null);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus200_ValidId() {
        _mockManager.Setup(x => x.GetIngredientMarketplaces(1)).ReturnsAsync([]);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Get(1);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus400_InvalidId() {
        _mockManager.Setup(x => x.GetIngredientMarketplaces(-1));

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Get(-1);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus200_AllValid() {
        IngredientMarketplaceDTO toAdd = new() {
            IngredientId = 1,
            MarketplaceId = 1,
            MarketplaceLink = "ingredient marketplace link"
        };
        _mockManager.Setup(x => x.CreateIngredientMarketplace(It.IsAny<IngredientMarketplaceDTO>())).ReturnsAsync(true);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Create(toAdd);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus500_IngredientNotExist() {
        IngredientMarketplaceDTO toAdd = new() {
            IngredientId = 0,
            MarketplaceId = 1,
            MarketplaceLink = "ingredient marketplace link"
        };
        _mockManager.Setup(x => x.CreateIngredientMarketplace(It.IsAny<IngredientMarketplaceDTO>())).ReturnsAsync(false);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Create(toAdd);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Create_ReturnStatus500_MarketplaceNotExist() {
        IngredientMarketplaceDTO toAdd = new() {
            IngredientId = 1,
            MarketplaceId = 0,
            MarketplaceLink = "ingredient marketplace link"
        };
        _mockManager.Setup(x => x.CreateIngredientMarketplace(It.IsAny<IngredientMarketplaceDTO>())).ReturnsAsync(false);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Create(toAdd);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Create_ReturnStatus400_InvalidIngredientId() {
        IngredientMarketplaceDTO toAdd = new() {
            IngredientId = -1,
            MarketplaceId = 1,
            MarketplaceLink = "ingredient marketplace link"
        };
        _mockManager.Setup(x => x.CreateIngredientMarketplace(It.IsAny<IngredientMarketplaceDTO>())).ReturnsAsync(false);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus400_InvalidMarketplaceId() {
        IngredientMarketplaceDTO toAdd = new() {
            IngredientId = 1,
            MarketplaceId = -1,
            MarketplaceLink = "ingredient marketplace link"
        };
        _mockManager.Setup(x => x.CreateIngredientMarketplace(It.IsAny<IngredientMarketplaceDTO>())).ReturnsAsync(false);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus400_EmptyLink() {
        IngredientMarketplaceDTO toAdd = new() {
            IngredientId = 1,
            MarketplaceId = 1,
            MarketplaceLink = string.Empty
        };
        _mockManager.Setup(x => x.CreateIngredientMarketplace(It.IsAny<IngredientMarketplaceDTO>())).ReturnsAsync(true);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus200_AllValid() {
        IngredientMarketplaceDTO toUpdate = new() {
            IngredientId = 1,
            MarketplaceId = 1,
            MarketplaceLink = "new link"
        };
        _mockManager.Setup(x => x.UpdateIngredientMarketplace(It.IsAny<IngredientMarketplaceDTO>())).ReturnsAsync(true);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Update_ReturnStatus404_IngredientMarketplaceNotExist() {
        IngredientMarketplaceDTO toUpdate = new() {
            IngredientId = 1,
            MarketplaceId = 2,
            MarketplaceLink = "new link"
        };
        _mockManager.Setup(x => x.UpdateIngredientMarketplace(It.IsAny<IngredientMarketplaceDTO>())).ReturnsAsync(false);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeNotFoundObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_InvalidIngredientId() {
        IngredientMarketplaceDTO toUpdate = new() {
            IngredientId = -1,
            MarketplaceId = 1,
            MarketplaceLink = "new link"
        };
        _mockManager.Setup(x => x.UpdateIngredientMarketplace(It.IsAny<IngredientMarketplaceDTO>())).ReturnsAsync(false);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_InvalidMarketplaceId() {
        IngredientMarketplaceDTO toUpdate = new() {
            IngredientId = 1,
            MarketplaceId = -1,
            MarketplaceLink = "new link"
        };
        _mockManager.Setup(x => x.UpdateIngredientMarketplace(It.IsAny<IngredientMarketplaceDTO>())).ReturnsAsync(false);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_EmptyLink() {
        IngredientMarketplaceDTO toUpdate = new() {
            IngredientId = 1,
            MarketplaceId = 1,
            MarketplaceLink = string.Empty
        };
        _mockManager.Setup(x => x.UpdateIngredientMarketplace(It.IsAny<IngredientMarketplaceDTO>())).ReturnsAsync(false);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Delete_ReturnStatus200_AllValid() {
        _mockManager.Setup(x => x.DeleteIngredientMarketplace(1, 1)).ReturnsAsync(true);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Delete(1, 1);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Delete_ReturnStatus404_IngredientMarketplaceNotExist() {
        _mockManager.Setup(x => x.DeleteIngredientMarketplace(1, 2)).ReturnsAsync(false);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Delete(1, 2);

        result.Should().BeNotFoundObjectResult();
    }

    [Fact]
    public async void Delete_ReturnStatus400_InvalidIngredientId() {
        _mockManager.Setup(x => x.DeleteIngredientMarketplace(-1, 1)).ReturnsAsync(false);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Delete(-1, 1);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Delete_ReturnStatus400_InvalidMarketplaceId() {
        _mockManager.Setup(x => x.DeleteIngredientMarketplace(1, -1)).ReturnsAsync(false);

        IngMarketplaceController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Delete(1, -1);

        result.Should().BeBadRequestObjectResult();
    }
}
