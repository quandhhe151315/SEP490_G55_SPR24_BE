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

public class MarketplaceControllerTest
{
    private Mock<IMarketplaceManager> _mockMarketplaceManager;
    private IConfiguration _configuration;

    public MarketplaceControllerTest() {
        _mockMarketplaceManager = new Mock<IMarketplaceManager>();
        _configuration = new ConfigurationBuilder().Build();
    }

    [Fact]
    public async void Get_ReturnStatus200() {
        _mockMarketplaceManager.Setup(x => x.GetMarketplaces()).ReturnsAsync([]);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Get();

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus200_AllValid() {
        MarketplaceDTO toAdd = new() {
            MarketplaceName = "marketplace name",
            MarketplaceLogo = "marketplace logo link"
        };
        _mockMarketplaceManager.Setup(x => x.CreateMarketplace(It.IsAny<MarketplaceDTO>()));

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Create(toAdd);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Create_ReturnStatus400_EmptyMarketplaceName() {
        MarketplaceDTO toAdd = new() {
            MarketplaceName = string.Empty,
            MarketplaceLogo = "marketplace logo link"
        };
        _mockMarketplaceManager.Setup(x => x.CreateMarketplace(It.IsAny<MarketplaceDTO>()));

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus400_EmptyMarketplaceLogo() {
        MarketplaceDTO toAdd = new() {
            MarketplaceName = "marketplace name",
            MarketplaceLogo = string.Empty
        };
        _mockMarketplaceManager.Setup(x => x.CreateMarketplace(It.IsAny<MarketplaceDTO>()));

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus200_AllValid(){
        MarketplaceDTO toUpdate = new() {
            MarketplaceId = 1,
            MarketplaceName = "new marketplace name",
            MarketplaceLogo = "new marketplace logo"
        };
        _mockMarketplaceManager.Setup(x => x.UpdateMarketplace(It.IsAny<MarketplaceDTO>())).ReturnsAsync(true);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Update_ReturnStatus404_MarketplaceNotExist(){
        MarketplaceDTO toUpdate = new() {
            MarketplaceId = 0,
            MarketplaceName = "new marketplace name",
            MarketplaceLogo = "new marketplace logo"
        };
        _mockMarketplaceManager.Setup(x => x.UpdateMarketplace(It.IsAny<MarketplaceDTO>())).ReturnsAsync(false);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeNotFoundObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus406_NullMarketplaceId(){
        MarketplaceDTO toUpdate = new() {
            MarketplaceId = null,
            MarketplaceName = "new marketplace name",
            MarketplaceLogo = "new marketplace logo"
        };
        _mockMarketplaceManager.Setup(x => x.UpdateMarketplace(It.IsAny<MarketplaceDTO>())).ReturnsAsync(false);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(406);
    }

    [Fact]
    public async void Update_ReturnStatus400_InvalidMarketplaceId(){
        MarketplaceDTO toUpdate = new() {
            MarketplaceId = -1,
            MarketplaceName = "new marketplace name",
            MarketplaceLogo = "new marketplace logo"
        };
        _mockMarketplaceManager.Setup(x => x.UpdateMarketplace(It.IsAny<MarketplaceDTO>())).ReturnsAsync(false);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_EmptyMarketplaceName(){
        MarketplaceDTO toUpdate = new() {
            MarketplaceId = 1,
            MarketplaceName = string.Empty,
            MarketplaceLogo = "new marketplace logo"
        };
        _mockMarketplaceManager.Setup(x => x.UpdateMarketplace(It.IsAny<MarketplaceDTO>())).ReturnsAsync(false);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_EmptyMarketplaceLogo(){
        MarketplaceDTO toUpdate = new() {
            MarketplaceId = 1,
            MarketplaceName = "new marketplace name",
            MarketplaceLogo = string.Empty
        };
        _mockMarketplaceManager.Setup(x => x.UpdateMarketplace(It.IsAny<MarketplaceDTO>())).ReturnsAsync(false);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Delete_ReturnStatus200_ValidMarketplaceId() {
        _mockMarketplaceManager.Setup(x => x.DeleteMarketplace(1)).ReturnsAsync(true);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Delete(1);
        
        result.Should().BeOkResult();
    }

    [Fact]
    public async void Delete_ReturnStatus404_MarketplaceNotExist() {
        _mockMarketplaceManager.Setup(x => x.DeleteMarketplace(0)).ReturnsAsync(false);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Delete(0);
        
        result.Should().BeNotFoundObjectResult();
    }

    [Fact]
    public async void Delete_ReturnStatus400_InvalidMarketplaceId() {
        _mockMarketplaceManager.Setup(x => x.DeleteMarketplace(-1)).ReturnsAsync(false);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Delete(-1);
        
        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Status_ReturnStatus200_ValidMarketplaceId() {
        _mockMarketplaceManager.Setup(x => x.UpdateStatus(1)).ReturnsAsync(true);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Status(1);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Status_ReturnStatus404_MarketplaceNotExist() {
        _mockMarketplaceManager.Setup(x => x.UpdateStatus(0)).ReturnsAsync(false);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Status(0);

        result.Should().BeNotFoundObjectResult();
    }

    [Fact]
    public async void Status_ReturnStatus400_InvalidMarketplaceId() {
        _mockMarketplaceManager.Setup(x => x.UpdateStatus(-1)).ReturnsAsync(false);

        MarketplaceController _controller = new(_mockMarketplaceManager.Object, _configuration);
        var result = await _controller.Status(-1);

        result.Should().BeBadRequestObjectResult();
    }
}
