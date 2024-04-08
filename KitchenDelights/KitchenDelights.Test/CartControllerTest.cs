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

public class CartControllerTest
{
    private Mock<ICartManager> _mockCartManager;
    private Mock<IUserManager> _mockUserManager;
    private IConfiguration _configuration;

    public CartControllerTest() {
        _mockCartManager = new Mock<ICartManager>();
        _mockUserManager = new Mock<IUserManager>();
        _configuration = new ConfigurationBuilder().Build();
    }

    [Fact]
    public async void Get_ReturnStatus200_ValidUserId() {
        _mockCartManager.Setup(x => x.GetCart(1)).ReturnsAsync(new List<CartItemDTO>(){
            new CartItemDTO() {
                RecipeId = 1,
                RecipePrice = 10000,
                VoucherCode = "validvoucher",
                DiscountPercentage = 10
            },
            new CartItemDTO() {
                RecipeId = 2,
                RecipePrice = 20000,
                VoucherCode = "validvoucher",
                DiscountPercentage = 10
            },
            new CartItemDTO() {
                RecipeId = 3,
                RecipePrice = 30000,
                VoucherCode = "validvoucher",
                DiscountPercentage = 10
            },
        });
        _mockUserManager.Setup(x => x.GetUser(1)).ReturnsAsync(new UserDTO(){
            UserId = 1,
            FirstName = "firstname",
        });

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Get(1);

        result.Should().BeOkObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus404_UserNotExist() {
        _mockCartManager.Setup(x => x.GetCart(0));
        _mockUserManager.Setup(x => x.GetUser(0));

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Get(0);

        result.Should().BeNotFoundObjectResult();
    }

    [Fact]
    public async void Get_ReturnStatus400_InvalidUserId() {
        _mockCartManager.Setup(x => x.GetCart(-1));
        _mockUserManager.Setup(x => x.GetUser(-1));

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Get(-1);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Add_ReturnStatus200_AllValid() {
        CartItemDTO toAdd = new() {
            RecipeId = 1,
            UserId = 1
        };
        _mockCartManager.Setup(x => x.CreateCartItem(It.IsAny<CartItemDTO>())).ReturnsAsync(true);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Add(toAdd);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Add_ReturnStatus400_RecipeNotExist() {
        CartItemDTO toAdd = new() {
            RecipeId = 0,
            UserId = 1
        };
        _mockCartManager.Setup(x => x.CreateCartItem(It.IsAny<CartItemDTO>())).ReturnsAsync(false);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Add(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Add_ReturnStatus400_UserNotExist() {
        CartItemDTO toAdd = new() {
            RecipeId = 1,
            UserId = 0
        };
        _mockCartManager.Setup(x => x.CreateCartItem(It.IsAny<CartItemDTO>())).ReturnsAsync(false);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Add(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Add_ReturnStatus400_InvalidRecipeId() {
        CartItemDTO toAdd = new() {
            RecipeId = -1,
            UserId = 1
        };
        _mockCartManager.Setup(x => x.CreateCartItem(It.IsAny<CartItemDTO>())).ReturnsAsync(false);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Add(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Add_ReturnStatus400_InvalidUserId() {
        CartItemDTO toAdd = new() {
            RecipeId = 1,
            UserId = -1
        };
        _mockCartManager.Setup(x => x.CreateCartItem(It.IsAny<CartItemDTO>())).ReturnsAsync(false);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Add(toAdd);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Discount_ReturnStatus200_AllValid() {
        _mockCartManager.Setup(x => x.UpdateCartItem(1, "validvoucher")).ReturnsAsync(true);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Discount(1, "validvoucher");

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Discount_ReturnStatus500_NoItemInCart() {
        _mockCartManager.Setup(x => x.UpdateCartItem(0, "validvoucher")).ReturnsAsync(false);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Discount(0, "validvoucher");

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Discount_ReturnStatus500_VoucherNotExist() {
        _mockCartManager.Setup(x => x.UpdateCartItem(1, "invalidvoucher")).ReturnsAsync(false);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Discount(1, "invalidvoucher");

        result.Should().BeObjectResult();
        (result as ObjectResult)!.StatusCode.Should().Be(500);
    }

    [Fact]
    public async void Discount_ReturnStatus400_InvalidUserId() {
        _mockCartManager.Setup(x => x.UpdateCartItem(-1, "validvoucher")).ReturnsAsync(false);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Discount(-1, "validvoucher");

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Discount_ReturnStatus400_EmptyVoucherCode() {
        _mockCartManager.Setup(x => x.UpdateCartItem(1, string.Empty)).ReturnsAsync(true);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Discount(1, string.Empty);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Remove_ReturnStatus200_AllValid() {
        CartItemDTO toDelete = new(){
            RecipeId = 1,
            UserId = 1
        };
        _mockCartManager.Setup(x => x.DeleteCartItem(It.IsAny<CartItemDTO>())).ReturnsAsync(true);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Remove(toDelete);

        result.Should().BeOkResult();
    }

    [Fact]
    public async void Remove_ReturnStatus400_RecipeNotInCart() {
        CartItemDTO toDelete = new(){
            RecipeId = 0,
            UserId = 1
        };
        _mockCartManager.Setup(x => x.DeleteCartItem(It.IsAny<CartItemDTO>())).ReturnsAsync(false);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Remove(toDelete);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Remove_ReturnStatus400_UserNotHaveItemInCart() {
        CartItemDTO toDelete = new(){
            RecipeId = 1,
            UserId = 0
        };
        _mockCartManager.Setup(x => x.DeleteCartItem(It.IsAny<CartItemDTO>())).ReturnsAsync(false);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Remove(toDelete);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Remove_ReturnStatus400_InvalidRecipeId() {
        CartItemDTO toDelete = new(){
            RecipeId = -1,
            UserId = 1
        };
        _mockCartManager.Setup(x => x.DeleteCartItem(It.IsAny<CartItemDTO>())).ReturnsAsync(false);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Remove(toDelete);

        result.Should().BeBadRequestObjectResult();
    }

    [Fact]
    public async void Remove_ReturnStatus400_InvalidUserId() {
        CartItemDTO toDelete = new(){
            RecipeId = 1,
            UserId = -1
        };
        _mockCartManager.Setup(x => x.DeleteCartItem(It.IsAny<CartItemDTO>())).ReturnsAsync(false);

        CartController _controller = new(_mockCartManager.Object, _mockUserManager.Object, _configuration);
        var result = await _controller.Remove(toDelete);

        result.Should().BeBadRequestObjectResult();
    }
}
