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

public class HistoryManagerTest
{
    private Mock<IHistoryRepository> _mockHistoryRepository;
    private Mock<IVoucherRepository> _mockVoucherRepository;
    private Mock<ICartRepository> _mockCartRepository;
    private IMapper _mapper;

    public HistoryManagerTest() {
        _mockHistoryRepository = new Mock<IHistoryRepository>();
        _mockVoucherRepository = new Mock<IVoucherRepository>();
        _mockCartRepository = new Mock<ICartRepository>();
         _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<PaymentHistoryProfile>();
                options.AddProfile<VoucherProfile>();
                options.AddProfile<CartItemProfile>();
            }));
    }

    [Fact]
    public async void GetPaymentHistory_GetPaymentHistoryList() {
        var paymentHistory = GetPaymentHistory();
        _mockHistoryRepository.Setup(x => x.GetPaymentHistory()).ReturnsAsync(paymentHistory);

        IHistoryManager _historyManager = new HistoryManager(_mockHistoryRepository.Object, _mockVoucherRepository.Object, _mockCartRepository.Object, _mapper);
        var result = await _historyManager.GetPaymentHistory();

        result.Count.Should().Be(paymentHistory.Count);
    }

    [Fact]
    public async void GetPaymentHistory_GetPaymentHistoryByUserId_ValidUserId() {
        var paymentHistory = GetPaymentHistory();
        _mockHistoryRepository.Setup(x => x.GetPaymentHistory(1)).ReturnsAsync(paymentHistory.Where(x => x.UserId == 1).ToList());

        IHistoryManager _historyManager = new HistoryManager(_mockHistoryRepository.Object, _mockVoucherRepository.Object, _mockCartRepository.Object, _mapper);
        var result = await _historyManager.GetPaymentHistory(1);
        var actual = paymentHistory.Where(x => x.UserId == 1);

        result.Count.Should().Be(actual.Count());
    }

    [Fact]
    public async void GetPaymentHistory_GetPaymentHistoryByUserId_BoundaryUserId() {
        var paymentHistory = GetPaymentHistory();
        _mockHistoryRepository.Setup(x => x.GetPaymentHistory(0)).ReturnsAsync(paymentHistory.Where(x => x.UserId == 0).ToList());

        IHistoryManager _historyManager = new HistoryManager(_mockHistoryRepository.Object, _mockVoucherRepository.Object, _mockCartRepository.Object, _mapper);
        var result = await _historyManager.GetPaymentHistory(0);
        var actual = paymentHistory.Where(x => x.UserId == 0);

        result.Count.Should().Be(actual.Count());
    }

    [Fact]
    public async void GetPaymentHistory_GetPaymentHistoryByUserId_AbnormalUserId() {
        var paymentHistory = GetPaymentHistory();
        _mockHistoryRepository.Setup(x => x.GetPaymentHistory(-1)).ReturnsAsync(paymentHistory.Where(x => x.UserId == -1).ToList());

        IHistoryManager _historyManager = new HistoryManager(_mockHistoryRepository.Object, _mockVoucherRepository.Object, _mockCartRepository.Object, _mapper);
        var result = await _historyManager.GetPaymentHistory(-1);
        var actual = paymentHistory.Where(x => x.UserId == -1);

        result.Count.Should().Be(actual.Count());
    }

    [Fact]
    public async void CreatePaymentHistory_CreatePaymentHistory_CartWithVoucher() {
        var paymentHistory = GetPaymentHistory();
        List<CartItemDTO> toAdd = [
            new CartItemDTO() {
                UserId = 1,
                RecipeId = 4,
                VoucherCode = "validvoucher"
            },
            new CartItemDTO() {
                UserId = 1,
                RecipeId = 5,
                VoucherCode = "validvoucher"
            },
        ];
        _mockCartRepository.Setup(x => x.DeleteCartItem(It.IsAny<CartItem>()));
        _mockVoucherRepository.Setup(x => x.GetVoucher(toAdd[0].VoucherCode)).ReturnsAsync(new Voucher());
        _mockHistoryRepository.Setup(x => x.CreatePaymentHistory(It.IsAny<List<PaymentHistory>>()));

        IHistoryManager _historyManager = new HistoryManager(_mockHistoryRepository.Object, _mockVoucherRepository.Object, _mockCartRepository.Object, _mapper);
        var result = await _historyManager.CreatePaymentHistory(toAdd);
        
        result.Should().BeTrue();
        _mockCartRepository.Verify(x => x.DeleteCartItem(It.IsAny<CartItem>()), Times.Never);
        _mockHistoryRepository.Verify(x => x.CreatePaymentHistory(It.IsAny<List<PaymentHistory>>()), Times.Once);
    }

    [Fact]
    public async void CreatePaymentHistory_CreatePaymentHistory_CartWithoutVoucher() {
        var paymentHistory = GetPaymentHistory();
        List<CartItemDTO> toAdd = [
            new CartItemDTO() {
                UserId = 1,
                RecipeId = 4,
            },
            new CartItemDTO() {
                UserId = 1,
                RecipeId = 5,
            },
        ];
        _mockCartRepository.Setup(x => x.DeleteCartItem(It.IsAny<CartItem>()));
        _mockVoucherRepository.Setup(x => x.GetVoucher(toAdd[0].VoucherCode)).ReturnsAsync(new Voucher());
        _mockHistoryRepository.Setup(x => x.CreatePaymentHistory(It.IsAny<List<PaymentHistory>>()));

        IHistoryManager _historyManager = new HistoryManager(_mockHistoryRepository.Object, _mockVoucherRepository.Object, _mockCartRepository.Object, _mapper);
        var result = await _historyManager.CreatePaymentHistory(toAdd);
        
        result.Should().BeTrue();
        _mockCartRepository.Verify(x => x.DeleteCartItem(It.IsAny<CartItem>()), Times.Never);
        _mockHistoryRepository.Verify(x => x.CreatePaymentHistory(It.IsAny<List<PaymentHistory>>()), Times.Once);
    }

    private List<PaymentHistory> GetPaymentHistory() {
        List<PaymentHistory> output = [
            new PaymentHistory() {
                UserId = 1,
                User = new() {
                    UserId = 1,
                    FirstName = "firstname",
                    MiddleName = "middlename",
                    LastName = "lastname"
                },
                RecipeId = 1,
                Recipe = new() {
                    RecipeId = 1,
                    RecipeTitle = "recipe title 1"
                },
                ActualPrice = 10000.0m,
                PurchaseDate = DateTime.Now
            },
            new PaymentHistory() {
                UserId = 1,
                User = new() {
                    UserId = 1,
                    FirstName = "firstname",
                    MiddleName = "middlename",
                    LastName = "lastname"
                },
                RecipeId = 2,
                Recipe = new() {
                    RecipeId = 2,
                    RecipeTitle = "recipe title 2"
                },
                ActualPrice = 30000.0m,
                PurchaseDate = DateTime.Now.AddMonths(1)
            },
            new PaymentHistory() {
                UserId = 1,
                User = new() {
                    UserId = 1,
                    FirstName = "firstname",
                    MiddleName = "middlename",
                    LastName = "lastname"
                },
                RecipeId = 3,
                Recipe = new() {
                    RecipeId = 3,
                    RecipeTitle = "recipe title 3"
                },
                ActualPrice = 50000.0m,
                PurchaseDate = DateTime.Now.AddMonths(2)
            },
        ];
        return output;
    }
}
