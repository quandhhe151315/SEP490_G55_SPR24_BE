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

public class VerificationControllerTest
{
    private Mock<IVerificationManager> _mockManager;
    private IConfiguration _configuration;

    public VerificationControllerTest() {
        _mockManager = new Mock<IVerificationManager>();
        _configuration = new ConfigurationBuilder().Build();
    }

    [Fact]
    public async void Get_ReturnStatus200_NullId() {
        _mockManager.Setup(x => x.GetVerifications()).ReturnsAsync([]);

        VerificationController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Get(null);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_Return200_ValidIdVerificationExist() {
        _mockManager.Setup(x => x.GetVerification(1)).ReturnsAsync(new VerificationDTO());

        VerificationController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Get(1);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_Return404_ValidIdVerificationNotExist() {
        _mockManager.Setup(x => x.GetVerification(5));

        VerificationController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Get(5);

        result.Should().BeNotFoundResult();
    }

    [Fact]
    public async void Get_Return400_InvalidId() {
        _mockManager.Setup(x => x.GetVerification(-1));

        VerificationController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Get(-1);

        result.Should().BeBadRequestResult();
    }

    [Fact]
    public async void Create_ReturnStatus200_AllValid() {
        VerificationDTO toAdd = new() {
            UserId = 1
        };
        _mockManager.Setup(x => x.CreateVerification(It.IsAny<VerificationDTO>())).ReturnsAsync(true);

        VerificationController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Create(toAdd);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Create_ReturnStatus400_UserNotExist() {
        VerificationDTO toAdd = new() {
            UserId = 11
        };
        _mockManager.Setup(x => x.CreateVerification(It.IsAny<VerificationDTO>())).ReturnsAsync(false);

        VerificationController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Create_ReturnStatus400_InvalidUserId() {
        VerificationDTO toAdd = new() {
            UserId = -1
        };
        _mockManager.Setup(x => x.CreateVerification(It.IsAny<VerificationDTO>())).ReturnsAsync(false);

        VerificationController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Create(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus200_AllValid() {
        VerificationDTO toUpdate = new() {
            VerificationId = 1,
            VerificationStatus = 1
        };
        _mockManager.Setup(x => x.UpdateVerification(It.IsAny<VerificationDTO>())).ReturnsAsync(true);

        VerificationController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_VerificationNotExist() {
        VerificationDTO toUpdate = new() {
            VerificationId = 10,
            VerificationStatus = 1
        };
        _mockManager.Setup(x => x.UpdateVerification(It.IsAny<VerificationDTO>())).ReturnsAsync(false);

        VerificationController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_InvalidVerificationId() {
        VerificationDTO toUpdate = new() {
            VerificationId = -1,
            VerificationStatus = 1
        };
        _mockManager.Setup(x => x.UpdateVerification(It.IsAny<VerificationDTO>())).ReturnsAsync(false);

        VerificationController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Update_ReturnStatus400_InvalidVerificationStatus() {
        VerificationDTO toUpdate = new() {
            VerificationId = 1,
            VerificationStatus = 3
        };
        _mockManager.Setup(x => x.UpdateVerification(It.IsAny<VerificationDTO>())).ReturnsAsync(true);

        VerificationController _controller = new(_mockManager.Object, _configuration);
        var result = await _controller.Update(toUpdate);

        result.Should().BeBadRequestObjectResult();
    }
}
