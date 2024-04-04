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

        [Fact]
        public async void GetVoucher_GetVoucherByVoucherCode_ExistInRepo()
        {
            //Arrange
            var vouchers = VouchersSample();
            List<VoucherDTO> voucherDTOs = [];
            voucherDTOs.AddRange(vouchers.Select(_mapper.Map<Voucher, VoucherDTO>));
            _voucherRepositoryMock.Setup(x => x.GetVoucher("GIAM15")).ReturnsAsync(vouchers.Find(x => x.VoucherCode.Equals("GIAM15"))); //Mock Advertisement repository GetAdvertisementById(int id) method

            //Act
            IVoucherManager _voucherManager = new VoucherManager(_voucherRepositoryMock.Object, _mapper);
            var result = await _voucherManager.GetVoucher("GIAM15");
            var actual = voucherDTOs.Find(x => x.VoucherCode.Equals("GIAM15"));

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<VoucherDTO>().And.BeEquivalentTo(actual!);
        }

        [Fact]
        public async void GetVoucher_GetVoucherByVoucherCode_NotExistInRepo()
        {
            var vouchers = VouchersSample();
            _voucherRepositoryMock.Setup(x => x.GetVoucher("ABCDE")).ReturnsAsync(vouchers.FirstOrDefault(x => x.VoucherCode.Equals("ABCDE")));

            IVoucherManager _voucherManager = new VoucherManager(_voucherRepositoryMock.Object, _mapper);
            var result = await _voucherManager.GetVoucher("ABCDE");
            var actual = vouchers.FirstOrDefault(x => x.VoucherCode.Equals("ABCDE"));

            result.Should().BeNull();
            actual.Should().BeNull();
        }

        [Fact]
        public async void CreateVoucher_CreateWithVoucherDTO_VoucherNotExistInRepo()
        {
            var vouchers = VouchersSample();
            VoucherDTO voucherDTO = new()
            {
                VoucherCode = "GIAM20",
                UserId = 1,
                DiscountPercentage = 20
            };
            _voucherRepositoryMock.Setup(x => x.CreateVoucher(It.IsAny<Voucher>())).Callback<Voucher>(vouchers.Add);
            _voucherRepositoryMock.Setup(x => x.GetVoucher("GIAM20")).ReturnsAsync(vouchers.FirstOrDefault(x => x.VoucherCode == "GIAM20"));

            IVoucherManager _voucherManager = new VoucherManager(_voucherRepositoryMock.Object, _mapper);
            var boolResult = await _voucherManager.CreateVoucher(voucherDTO);
            var countResult = vouchers.Count;

            boolResult.Should().BeTrue();
            countResult.Should().Be(4);
        }

        [Fact]
        public async void CreateVoucher_CreateWithVoucherDTO_VoucherExistInRepo()
        {
            var vouchers = VouchersSample();
            VoucherDTO voucherDTO = new()
            {
                VoucherCode = "GIAM15",
                UserId = 1,
                DiscountPercentage = 15
            };
            _voucherRepositoryMock.Setup(x => x.CreateVoucher(It.IsAny<Voucher>())).Callback<Voucher>(vouchers.Add);
            _voucherRepositoryMock.Setup(x => x.GetVoucher("GIAM15")).ReturnsAsync(vouchers.FirstOrDefault(x => x.VoucherCode == "GIAM15"));

            IVoucherManager _voucherManager = new VoucherManager(_voucherRepositoryMock.Object, _mapper);
            var boolResult = await _voucherManager.CreateVoucher(voucherDTO);
            var countResult = vouchers.Count;

            boolResult.Should().BeFalse();
            countResult.Should().Be(3);
        }

        [Fact]
        public async void UpdateVoucher_UpdateVoucher_VoucherExistInRepo()
        {
            var vouchers = VouchersSample();
            VoucherDTO voucherDTO = new()
            {
                VoucherCode = "GIAM15",
                UserId = 1,
                DiscountPercentage = 5
            };
            _voucherRepositoryMock.Setup(x => x.GetVoucher("GIAM15")).ReturnsAsync(vouchers.FirstOrDefault(x => x.VoucherCode == "GIAM15"));
            _voucherRepositoryMock.Setup(x => x.UpdateVoucher(It.IsAny<Voucher>())).Callback<Voucher>((voucher) => vouchers[0] = voucher);

            IVoucherManager _voucherManager = new VoucherManager(_voucherRepositoryMock.Object, _mapper);
            var boolResult = await _voucherManager.UpdateVoucher(voucherDTO);
            var updatedVoucher = vouchers.FirstOrDefault(x => x.VoucherCode == "GIAM15");

            boolResult.Should().BeTrue();
            updatedVoucher.Should().NotBeNull();
            updatedVoucher!.VoucherCode.Should().BeSameAs(voucherDTO.VoucherCode);
            updatedVoucher!.UserId.Should().Be(voucherDTO.UserId);
            updatedVoucher!.DiscountPercentage.Should().Be(voucherDTO.DiscountPercentage);
        }

        [Fact]
        public async void UpdateVoucher_UpdateVoucher_VoucherNotExistInRepo()
        {
            var vouchers = VouchersSample();
            VoucherDTO voucherDTO = new()
            {
                VoucherCode = "GIAM99",
                UserId = 1,
                DiscountPercentage = 5
            };
            _voucherRepositoryMock.Setup(x => x.GetVoucher("GIAM99")).ReturnsAsync(vouchers.FirstOrDefault(x => x.VoucherCode == "GIAM99"));
            _voucherRepositoryMock.Setup(x => x.UpdateVoucher(It.IsAny<Voucher>())).Callback<Voucher>((voucher) => vouchers[0] = voucher);

            IVoucherManager _voucherManager = new VoucherManager(_voucherRepositoryMock.Object, _mapper);
            var boolResult = await _voucherManager.UpdateVoucher(voucherDTO);
            var updatedVoucher = vouchers.FirstOrDefault(x => x.VoucherCode == "GIAM99");

            boolResult.Should().BeFalse();
            updatedVoucher.Should().BeNull();
        }

        [Fact]
        public async void DeleteVoucher_DeleteExistingVoucher_VoucherExistInRepo()
        {
            var vouchers = VouchersSample();
            VoucherDTO voucherDTO = new()
            {
                VoucherCode = "GIAM15",
                UserId = 1,
                DiscountPercentage = 5
            };
            _voucherRepositoryMock.Setup(x => x.GetVoucher("GIAM15")).ReturnsAsync(vouchers.FirstOrDefault(x => x.VoucherCode == "GIAM15"));
            _voucherRepositoryMock.Setup(x => x.RemoveVoucher(It.IsAny<Voucher>())).Callback<Voucher>(item => vouchers.Remove(item));

            IVoucherManager _voucherManager = new VoucherManager(_voucherRepositoryMock.Object, _mapper);
            var boolResult = await _voucherManager.RemoveVoucher("GIAM15");
            var actual = vouchers.ToList();

            boolResult.Should().BeTrue();
            actual.Count.Should().Be(2);
        }

        [Fact]
        public async void DeleteVoucher_DeleteNotExistingVoucher_VoucherNotExistInRepo()
        {
            var vouchers = VouchersSample();
            VoucherDTO voucherDTO = new()
            {
                VoucherCode = "GIAM99",
                UserId = 1,
                DiscountPercentage = 5
            };
            _voucherRepositoryMock.Setup(x => x.GetVoucher("GIAM99")).ReturnsAsync(vouchers.FirstOrDefault(x => x.VoucherCode == "GIAM99"));
            _voucherRepositoryMock.Setup(x => x.RemoveVoucher(It.IsAny<Voucher>())).Callback<Voucher>(item => vouchers.Remove(item));

            IVoucherManager _voucherManager = new VoucherManager(_voucherRepositoryMock.Object, _mapper);
            var boolResult = await _voucherManager.RemoveVoucher("GIAM15");
            var actual = vouchers.ToList();

            boolResult.Should().BeFalse();
            actual.Count.Should().Be(3);
        }

        private static List<Voucher> VouchersSample()
        {
            List<Voucher> output = [
                new Voucher()
                {
                    VoucherCode = "GIAM15",
                    UserId = 1,
                    DiscountPercentage = 15,
                },
                new Voucher()
                {
                    VoucherCode = "GIAM20",
                    UserId = 2,
                    DiscountPercentage = 20,
                },
                new Voucher()
                {
                    VoucherCode = "GIAM10",
                    UserId = 1,
                    DiscountPercentage = 10,
                },
            ];
            return output;
        }
    }
}
