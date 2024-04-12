using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using FluentAssertions.AspNetCore.Mvc;
using KitchenDelights.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace KitchenDelights.Test
{
    public class VoucherControllerTest
    {
        private Mock<IVoucherManager> _mockVoucherManager;
        private IConfiguration _configuration;

        public VoucherControllerTest()
        {
            _mockVoucherManager = new Mock<IVoucherManager>();
            _configuration = new ConfigurationBuilder().Build();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetAllMenus()
        {
            _mockVoucherManager.Setup(x => x.GetVouchers(1)).ReturnsAsync(new List<VoucherDTO>(){
                new VoucherDTO()
                {
                    VoucherCode = "GIAM15",
                    UserId = 1,
                    DiscountPercentage = 15
                },
                new VoucherDTO()
                {
                    VoucherCode = "GIAM5",
                    UserId = 1,
                    DiscountPercentage = 5
                }
            });

            VoucherController _controller = new(_configuration, _mockVoucherManager.Object);
            var result = await _controller.GetAllVouchers(1);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetVoucherByCode()
        {
            _mockVoucherManager.Setup(x => x.GetVoucher("GIAM15")).ReturnsAsync(new VoucherDTO()
            {
                VoucherCode = "GIAM15",
                UserId = 1,
                DiscountPercentage = 15
            });

            VoucherController _controller = new(_configuration, _mockVoucherManager.Object);
            var result = await _controller.GetVoucherByCode("GIAM15");

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus404_GetVoucherByCode()
        {
            _mockVoucherManager.Setup(x => x.GetVoucher("ABCDE"));

            VoucherController _controller = new(_configuration, _mockVoucherManager.Object);
            var result = await _controller.GetVoucherByCode("ABCDE");

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Create_ReturnStatus200_UserExist()
        {
            _mockVoucherManager.Setup(x => x.CreateVoucher(It.IsAny<VoucherDTO>())).ReturnsAsync(true);

            VoucherController _controller = new(_configuration, _mockVoucherManager.Object);
            var result = await _controller.CreateVoucher(1);

            result.Should().BeOkResult();
        }

        [Fact]
        public async void Update_ReturnStatus200_AllValid()
        {
            VoucherDTO toUpdate = new VoucherDTO()
            {
                VoucherCode = "GIAM15",
                UserId = 1,
                DiscountPercentage = 10
            };
            _mockVoucherManager.Setup(x => x.GetVoucher("GIAM15")).ReturnsAsync(new VoucherDTO());
            _mockVoucherManager.Setup(x => x.UpdateVoucher(It.IsAny<VoucherDTO>())).ReturnsAsync(true);

            VoucherController _controller = new(_configuration, _mockVoucherManager.Object);
            var result = await _controller.UpdateVoucher(toUpdate);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void Update_ReturnStatus200_NotFound()
        {
            VoucherDTO toUpdate = new VoucherDTO()
            {
                VoucherCode = "ABCDE",
                UserId = 1,
                DiscountPercentage = 10
            };
            _mockVoucherManager.Setup(x => x.UpdateVoucher(It.IsAny<VoucherDTO>())).ReturnsAsync(false);

            VoucherController _controller = new(_configuration, _mockVoucherManager.Object);
            var result = await _controller.UpdateVoucher(toUpdate);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Update_ReturnStatus200_VoucherCodeEmpty()
        {
            VoucherDTO toUpdate = new VoucherDTO()
            {
                VoucherCode = string.Empty,
                UserId = 1,
                DiscountPercentage = 10
            };
            _mockVoucherManager.Setup(x => x.UpdateVoucher(It.IsAny<VoucherDTO>())).ReturnsAsync(false);

            VoucherController _controller = new(_configuration, _mockVoucherManager.Object);
            var result = await _controller.UpdateVoucher(toUpdate);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Delete_ReturnStatus200_ExistVoucher()
        {
            _mockVoucherManager.Setup(x => x.GetVoucher("GIAM15")).ReturnsAsync(new VoucherDTO());
            _mockVoucherManager.Setup(x => x.RemoveVoucher("GIAM15")).ReturnsAsync(true);

            VoucherController _controller = new(_configuration, _mockVoucherManager.Object);
            var result = await _controller.DeleteVoucher("GIAM15");

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void Delete_ReturnStatus200_NotExistVoucher()
        {
            _mockVoucherManager.Setup(x => x.GetVoucher("ABCDE"));
            _mockVoucherManager.Setup(x => x.RemoveVoucher("ABCDE")).ReturnsAsync(false);

            VoucherController _controller = new(_configuration, _mockVoucherManager.Object);
            var result = await _controller.DeleteVoucher("ABCDE");

            result.Should().BeNotFoundObjectResult();
        }
    }
}
