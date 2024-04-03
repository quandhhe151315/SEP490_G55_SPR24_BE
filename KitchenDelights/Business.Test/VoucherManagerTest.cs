using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Business.Profiles;
using Data.Entity;
using Data.Interfaces;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Test
{
    public class VoucherManagerTest
    {
        private readonly Mock<IVoucherRepository> _voucherRepositoryMock;
        private readonly IMapper _mapper;

        public VoucherManagerTest()
        {
            //Initial setup
            _voucherRepositoryMock = new Mock<IVoucherRepository>();
            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<VoucherProfile>();
            }));
        }

        [Fact]
        public async void GetVoucher_GetVoucherList_ExistInRepo()
        {
            var vouchers = VouchersSample();
            _voucherRepositoryMock.Setup(x => x.GetVouchers(1)).ReturnsAsync(vouchers.Where(x => x.UserId == 1).ToList());

            IVoucherManager _voucherManager = new VoucherManager(_voucherRepositoryMock.Object, _mapper);
            var result = await _voucherManager.GetVouchers(1);

            result.Should().BeOfType<List<VoucherDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(2);
        }

        [Fact]
        public async void GetVoucher_GetVoucherList_NotExistInRepo()
        {
            var vouchers = VouchersSample();
            _voucherRepositoryMock.Setup(x => x.GetVouchers(-1)).ReturnsAsync(vouchers.Where(x => x.UserId == -1).ToList());

            IVoucherManager _voucherManager = new VoucherManager(_voucherRepositoryMock.Object, _mapper);

            var result = await _voucherManager.GetVouchers(-1);
            var actual = vouchers.Where(x => x.UserId == -1).ToList();

            result.Should().BeNullOrEmpty();
            actual.Should().BeNullOrEmpty();
        }

        private static List<Voucher> VouchersSample()
        {
            List<Voucher> output = [
                new Voucher()
                {
                    VoucherCode = "GIAM25",
                    UserId = 1,
                    DiscountPercentage = 25,
                },
                new Voucher()
                {
                    VoucherCode = "GIAM30",
                    UserId = 2,
                    DiscountPercentage = 30,
                },
                new Voucher()
                {
                    VoucherCode = "GIAM30",
                    UserId = 1,
                    DiscountPercentage = 30,
                },
            ];
            return output;
        }
    }
}
