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

public class CartManagerTest
{
    private Mock<ICartRepository> _mockCartRepository;
    private Mock<IRecipeRepository> _mockRecipeRepository;
    private IMapper _mapper;

    public CartManagerTest() {
        _mockCartRepository = new Mock<ICartRepository>();
        _mockRecipeRepository = new Mock<IRecipeRepository>();
        _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<CartItemProfile>();
            }));
    }

    [Fact]
    public async void GetCart_GetCartByUserId_CartItemExistInRepo()
    {
        List<CartItem> cartItems = GetCartItems();
        _mockCartRepository.Setup(x => x.GetCart(1)).ReturnsAsync(cartItems.Where(x => x.UserId == 1).ToList());

        ICartManager _cartManager = new CartManager(_mockCartRepository.Object, _mockRecipeRepository.Object, _mapper);
        var result = await _cartManager.GetCart(1);
        var actual = cartItems.Where(x => x.UserId == 1).ToList();
        
        result.Should().BeOfType<List<CartItemDTO>>()
        .And.NotBeNullOrEmpty();
        actual.Should().NotBeNullOrEmpty();
        result.Count().Should().Be(actual.Count);
    }

    [Fact]
    public async void GetCart_NotGetCartByUserId_InvalidUser()
    {
        List<CartItem> cartItems = GetCartItems();
        _mockCartRepository.Setup(x => x.GetCart(-1)).ReturnsAsync(cartItems.Where(x => x.UserId == -1).ToList());

        ICartManager _cartManager = new CartManager(_mockCartRepository.Object, _mockRecipeRepository.Object, _mapper);
        var result = await _cartManager.GetCart(-1);
        
        result.Should().BeOfType<List<CartItemDTO>>()
        .And.BeNullOrEmpty();
    }

    [Fact]
    public async void CreateCartItem_AddItemToCart_AllValid()
    {
        List<CartItem> cartItems = GetCartItems();
        List<Recipe> recipes = GetRecipes();
        CartItemDTO toAdd = new() {
            UserId = 1,
            RecipeId = 5
        };
        _mockCartRepository.Setup(x => x.CreateCartItem(It.IsAny<CartItem>())).Callback(cartItems.Add);
        _mockRecipeRepository.Setup(x => x.GetRecipe(5)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == 5));

        ICartManager _cartManager = new CartManager(_mockCartRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _cartManager.CreateCartItem(toAdd);
        var actual = cartItems.Where(x => x.UserId == 1).ToList();

        boolResult.Should().BeTrue();
        actual.Count.Should().Be(5);
    }

    [Fact]
    public async void CreateCartItem_NotAddItemToCart_FreeRecipe()
    {
        List<CartItem> cartItems = GetCartItems();
        List<Recipe> recipes = GetRecipes();
        CartItemDTO toAdd = new() {
            UserId = 1,
            RecipeId = 6
        };
        _mockCartRepository.Setup(x => x.CreateCartItem(It.IsAny<CartItem>())).Callback(cartItems.Add);
        _mockRecipeRepository.Setup(x => x.GetRecipe(6)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == 6));

        ICartManager _cartManager = new CartManager(_mockCartRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _cartManager.CreateCartItem(toAdd);
        var actual = cartItems.Where(x => x.UserId == 1).ToList();

        boolResult.Should().BeFalse();
        actual.Count.Should().Be(4);
    }

    [Fact]
    public async void CreateCartItem_NotAddItemToCart_RecipeNotExistInRep()
    {
        List<CartItem> cartItems = GetCartItems();
        List<Recipe> recipes = GetRecipes();
        CartItemDTO toAdd = new() {
            UserId = 1,
            RecipeId = -1
        };
        _mockCartRepository.Setup(x => x.CreateCartItem(It.IsAny<CartItem>())).Callback(cartItems.Add);
        _mockRecipeRepository.Setup(x => x.GetRecipe(-1)).ReturnsAsync(recipes.FirstOrDefault(x => x.RecipeId == -1));

        ICartManager _cartManager = new CartManager(_mockCartRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _cartManager.CreateCartItem(toAdd);
        var actual = cartItems.Where(x => x.UserId == 1).ToList();

        boolResult.Should().BeFalse();
        actual.Count.Should().Be(4);
    }

    [Fact]
    public async void UpdateCartItem_AddVoucherToCartItem_AllValid()
    {
        List<CartItem> cartItems = GetCartItems();
        _mockCartRepository.Setup(x => x.GetCart(1)).ReturnsAsync(cartItems.Where(x => x.UserId == 1).ToList());
        _mockCartRepository.Setup(x => x.UpdateCartItem(It.IsAny<CartItem>())).Callback<CartItem>(item => cartItems[item.RecipeId -= 1] = item);

        ICartManager _cartManager = new CartManager(_mockCartRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _cartManager.UpdateCartItem(1, "validvoucher");
        var actual = cartItems.Where(x => x.UserId == 1).ToList();

        boolResult.Should().BeTrue();
        actual[0].VoucherCode.Should().BeEquivalentTo("validvoucher");
        _mockCartRepository.Verify(x => x.UpdateCartItem(It.IsAny<CartItem>()), Times.Exactly(actual.Count));
    }

    [Fact]
    public async void UpdateCartItem_AddVoucherToCartItem_InvalidVoucher()
    {
        List<CartItem> cartItems = GetCartItems();
        _mockCartRepository.Setup(x => x.GetCart(1)).ReturnsAsync(cartItems.Where(x => x.UserId == 1).ToList());
        _mockCartRepository.Setup(x => x.UpdateCartItem(It.IsAny<CartItem>())).Throws(new Exception());

        ICartManager _cartManager = new CartManager(_mockCartRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _cartManager.UpdateCartItem(1, "invalidvoucher");
        var actual = cartItems.Where(x => x.UserId == 1).ToList();

        boolResult.Should().BeFalse();
        actual[0].VoucherCode.Should().BeEquivalentTo("discount01");
        _mockCartRepository.Verify(x => x.UpdateCartItem(It.IsAny<CartItem>()), Times.Once());
    }

    [Fact]
    public async void UpdateCartItem_AddVoucherToCartItem_InvalidUser()
    {
        List<CartItem> cartItems = GetCartItems();
        _mockCartRepository.Setup(x => x.GetCart(-1)).ReturnsAsync(cartItems.Where(x => x.UserId == -1).ToList());
        _mockCartRepository.Setup(x => x.UpdateCartItem(It.IsAny<CartItem>())).Throws(new Exception());

        ICartManager _cartManager = new CartManager(_mockCartRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _cartManager.UpdateCartItem(-1, "validvoucher");
        var actual = cartItems.Where(x => x.UserId == -1).ToList();

        boolResult.Should().BeTrue();
        actual.Should().BeNullOrEmpty();
        _mockCartRepository.Verify(x => x.UpdateCartItem(It.IsAny<CartItem>()), Times.Never());
    }

    [Fact]
    public async void DeleteCartItem_DeleteExistingCartItem_CartItemExistInRepo()
    {
        List<CartItem> cartItems = GetCartItems();
        CartItemDTO toDelete = new() {
            UserId = 1,
            RecipeId = 1
        };
        _mockCartRepository.Setup(x => x.GetCartItem(1, 1)).ReturnsAsync(cartItems.FirstOrDefault(x => x.UserId == 1 && x.RecipeId == 1));
        _mockCartRepository.Setup(x => x.DeleteCartItem(It.IsAny<CartItem>())).Callback<CartItem>(item => cartItems.Remove(item));

        ICartManager _cartManager = new CartManager(_mockCartRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _cartManager.DeleteCartItem(toDelete);
        var actual = cartItems.Where(x => x.UserId == 1).ToList();

        boolResult.Should().BeTrue();
        actual.Count().Should().Be(3);
    }

    [Fact]
    public async void DeleteCartItem_NotDeleteExistingCartItem_CartItemNotExistInRepo()
    {
        List<CartItem> cartItems = GetCartItems();
        CartItemDTO toDelete = new() {
            UserId = 1,
            RecipeId = 5
        };
        _mockCartRepository.Setup(x => x.GetCartItem(1, 5)).ReturnsAsync(cartItems.FirstOrDefault(x => x.UserId == 1 && x.RecipeId == 5));
        _mockCartRepository.Setup(x => x.DeleteCartItem(It.IsAny<CartItem>())).Callback<CartItem>(item => cartItems.Remove(item));

        ICartManager _cartManager = new CartManager(_mockCartRepository.Object, _mockRecipeRepository.Object, _mapper);
        var boolResult = await _cartManager.DeleteCartItem(toDelete);
        var actual = cartItems.Where(x => x.UserId == 1).ToList();

        boolResult.Should().BeFalse();
        actual.Count().Should().Be(4);
    }

    private List<CartItem> GetCartItems()
    {
        List<CartItem> output = [
            new CartItem() {
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    Username = "mock_1",
                    FirstName = "firstname_1",
                    MiddleName = "middlename_1",
                    LastName = "lastname_1"
                },
                RecipeId = 1,
                Recipe = new Recipe() {
                    RecipeId = 1,
                    FeaturedImage = "recipe-image-1",
                    RecipeTitle = "recipe title 1",
                    IsFree = false,
                    RecipePrice = 10000
                },
                VoucherCode = "discount01",
                VoucherCodeNavigation = new() {
                    VoucherCode = "discount01",
                    DiscountPercentage = 20
                }
            },
            new CartItem() {
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    Username = "mock_1",
                    FirstName = "firstname_1",
                    MiddleName = "middlename_1",
                    LastName = "lastname_1"
                },
                RecipeId = 2,
                Recipe = new Recipe() {
                    RecipeId = 2,
                    FeaturedImage = "recipe-image-2",
                    RecipeTitle = "recipe title 2",
                    IsFree = false,
                    RecipePrice = 20000
                },
                VoucherCode = "discount01",
                VoucherCodeNavigation = new() {
                    VoucherCode = "discount01",
                    DiscountPercentage = 20
                }
            },
            new CartItem() {
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    Username = "mock_1",
                    FirstName = "firstname_1",
                    MiddleName = "middlename_1",
                    LastName = "lastname_1"
                },
                RecipeId = 3,
                Recipe = new Recipe() {
                    RecipeId = 3,
                    FeaturedImage = "recipe-image-3",
                    RecipeTitle = "recipe title 3",
                    IsFree = false,
                    RecipePrice = 30000
                },
                VoucherCode = "discount01",
                VoucherCodeNavigation = new() {
                    VoucherCode = "discount01",
                    DiscountPercentage = 20
                }
            },
            new CartItem() {
                UserId = 1,
                User = new User() {
                    UserId = 1,
                    Username = "mock_1",
                    FirstName = "firstname_1",
                    MiddleName = "middlename_1",
                    LastName = "lastname_1"
                },
                RecipeId = 4,
                Recipe = new Recipe() {
                    RecipeId = 4,
                    FeaturedImage = "recipe-image-4",
                    RecipeTitle = "recipe title 4",
                    IsFree = false,
                    RecipePrice = 40000
                },
                VoucherCode = "discount01",
                VoucherCodeNavigation = new() {
                    VoucherCode = "discount01",
                    DiscountPercentage = 20
                }
            }
        ];
        return output;
    }

    private List<Recipe> GetRecipes()
    {
        List<Recipe> output = [
            new Recipe() {
                RecipeId = 5,
                IsFree = false,
                RecipePrice = 50000
            },
            new Recipe() {
                RecipeId = 6,
                IsFree = true
            }
        ];
        return output;
    }
}
