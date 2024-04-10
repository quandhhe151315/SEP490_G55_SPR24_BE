using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Business.DTO;
using Business.Interfaces;
using FluentAssertions;
using FluentAssertions.AspNetCore.Mvc;
using KitchenDelights.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace KitchenDelights.Test;

public class PaymentControllerTest
{
    private Mock<IHistoryManager> _mockManager;
    private IConfiguration _configuration;

    public PaymentControllerTest() {
        _mockManager = new Mock<IHistoryManager>();
        _configuration = new ConfigurationBuilder().Build();
    }

    [Fact]
    public async void History_ReturnStatus200_NullIdUserIsRoleAdmin() {
        _mockManager.Setup(x => x.GetPaymentHistory()).ReturnsAsync([]);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Role, "Administrator"),
        }, "mock"));

        PaymentController _controller = new PaymentController(_mockManager.Object, _configuration){
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            }
        };
        var result = await _controller.History(null);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void History_ReturnStatus200_IdNotNull() {
        _mockManager.Setup(x => x.GetPaymentHistory(1)).ReturnsAsync([]);
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Role, "Administrator"),
        }, "mock"));

        PaymentController _controller = new PaymentController(_mockManager.Object, _configuration){
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            }
        };
        var result = await _controller.History(1);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void History_ReturnStatus400_InvalidId() {
        _mockManager.Setup(x => x.GetPaymentHistory(-1));
        var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new(ClaimTypes.Role, "Administrator"),
        }, "mock"));

        PaymentController _controller = new PaymentController(_mockManager.Object, _configuration){
            ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            }
        };
        var result = await _controller.History(-1);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Checkout_ReturnStatus200_ValidCart() {
        List<CartItemDTO> cart = [];
        _mockManager.Setup(x => x.CreatePaymentHistory(It.IsAny<List<CartItemDTO>>())).ReturnsAsync(true);

        PaymentController _controller = new PaymentController(_mockManager.Object, _configuration);
        var result = await _controller.Checkout(cart);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Checkout_ReturnStatus500_InvalidCart() {
        List<CartItemDTO> cart = [];
        _mockManager.Setup(x => x.CreatePaymentHistory(It.IsAny<List<CartItemDTO>>())).ReturnsAsync(false);

        PaymentController _controller = new PaymentController(_mockManager.Object, _configuration);
        var result = await _controller.Checkout(cart);

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }
}
