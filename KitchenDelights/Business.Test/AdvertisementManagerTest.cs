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

    public class AdvertisementManagerTest
    {
        private readonly Mock<IAdvertisementRepository> _advertisementRepositoryMock;
        private readonly IMapper _mapper;

        public AdvertisementManagerTest()
        {
            //Initial setup
            _advertisementRepositoryMock = new Mock<IAdvertisementRepository>();
            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<AdvertisemenProfile>();
            }));
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetAdvertisement_GetAdvertisementById_AdvertisementExistInRepo()
        {
            //Arrange
            var advertisements = AdvertisementsSample();
            List<AdvertisementDTO> advertisementDTOs = [];
            advertisementDTOs.AddRange(advertisements.Select(_mapper.Map<Advertisement, AdvertisementDTO>));
            _advertisementRepositoryMock.Setup(x => x.GetAdvertisementById(1)).ReturnsAsync(advertisements.Find(advertisement => advertisement.AdvertisementId == 1)); //Mock Advertisement repository GetAdvertisementById(int id) method

            //Act
            IAdvertisementManager _advertisementManager = new AdvertisementManager(_advertisementRepositoryMock.Object, _mapper);
            var result = await _advertisementManager.GetAdvertisementById(1);
            var actual = advertisementDTOs.Find(advertisement => advertisement.AdvertisementId == 1);

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<AdvertisementDTO>().And.BeEquivalentTo(actual!);
        }

        [Fact]
        public async void GetVoucher_GetVoucherList_ExistInRepo()
        {
            var advertisements = AdvertisementsSample();
            List<AdvertisementDTO> advertisementDTOs = [];
            advertisementDTOs.AddRange(advertisements.Select(_mapper.Map<Advertisement, AdvertisementDTO>));
            _advertisementRepositoryMock.Setup(x => x.GetAdvertisements()).ReturnsAsync(advertisements.ToList());

            IAdvertisementManager _advertisementManager = new AdvertisementManager(_advertisementRepositoryMock.Object, _mapper);
            var result = await _advertisementManager.GetAdvertisements();

            result.Should().BeOfType<List<AdvertisementDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(3);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetAdvertisement_GetAdvertisementById_AdvertisementNotExistInRepo()
        {
            var advertisements = AdvertisementsSample();
            _advertisementRepositoryMock.Setup(x => x.GetAdvertisementById(-1)).ReturnsAsync(advertisements.FirstOrDefault(x => x.AdvertisementId == -1));

            IAdvertisementManager _advertisementManager = new AdvertisementManager(_advertisementRepositoryMock.Object, _mapper);
            var result = await _advertisementManager.GetAdvertisementById(-1);
            var actual = advertisements.FirstOrDefault(x => x.AdvertisementId == -1);

            result.Should().BeNull();
            actual.Should().BeNull();
        }

        [Fact]
        public async void CreateAdvertisement_CreateWithAdvertisementDTO_AdvertisementNotExistInRepo()
        {
            var advertisements = AdvertisementsSample();
            AdvertisementDTO advertisementDTO = new()
            {
                AdvertisementId = 4,
                AdvertisementImage = "mock-image-link",
                AdvertisementLink = "mock-advertisement-link",
                AdvertisementStatus = 1
            };
            _advertisementRepositoryMock.Setup(x => x.CreateAdvertisement(It.IsAny<Advertisement>())).Callback<Advertisement>(advertisements.Add);
            _advertisementRepositoryMock.Setup(x => x.GetAdvertisementById(4)).ReturnsAsync(advertisements.FirstOrDefault(advertisement => advertisement.AdvertisementId == 4));

            IAdvertisementManager _advertisementManager = new AdvertisementManager(_advertisementRepositoryMock.Object, _mapper);
            var boolResult = await _advertisementManager.CreateAdvertisement(advertisementDTO);
            var countResult = advertisements.Count;

            boolResult.Should().BeTrue();
            countResult.Should().Be(4);
        }

        [Fact]
        public async void UpdateAdvertisement_UpdateAdvertisement_AdvertisementExistInRepo()
        {
            var advertisements = AdvertisementsSample();
            AdvertisementDTO advertisementDTO = new()
            {
                AdvertisementId = 2,
                AdvertisementImage = "mock-image-link-update",
                AdvertisementLink = "mock-advertisement-link-update",
                AdvertisementStatus = 1
            };
            _advertisementRepositoryMock.Setup(x => x.GetAdvertisementById(2)).ReturnsAsync(advertisements.FirstOrDefault(advertisement => advertisement.AdvertisementId == 2));
            _advertisementRepositoryMock.Setup(x => x.UpdateAdvertisement(It.IsAny<Advertisement>())).Callback<Advertisement>((advertisement) => advertisements[0] = advertisement);

            IAdvertisementManager _advertisementManager = new AdvertisementManager(_advertisementRepositoryMock.Object, _mapper);
            var boolResult = await _advertisementManager.UpdateAdvertisement(advertisementDTO);
            var updatedAdvertisement = advertisements.FirstOrDefault(x => x.AdvertisementId == 2);

            boolResult.Should().BeTrue();
            updatedAdvertisement.Should().NotBeNull();
            updatedAdvertisement!.AdvertisementImage.Should().BeSameAs(advertisementDTO.AdvertisementImage);
            updatedAdvertisement!.AdvertisementLink.Should().BeSameAs(advertisementDTO.AdvertisementLink);
        }

        [Fact]
        public async void UpdateAdvertisement_UpdateAdvertisement_AdvertisementNotExistInRepo()
        {
            var advertisements = AdvertisementsSample();
            AdvertisementDTO advertisementDTO = new()
            {
                AdvertisementId = 4,
                AdvertisementImage = "mock-image-link-update",
                AdvertisementLink = "mock-advertisement-link-update",
                AdvertisementStatus = 1
            };
            _advertisementRepositoryMock.Setup(x => x.GetAdvertisementById(4)).ReturnsAsync(advertisements.FirstOrDefault(advertisement => advertisement.AdvertisementId == 4));
            _advertisementRepositoryMock.Setup(x => x.UpdateAdvertisement(It.IsAny<Advertisement>())).Callback<Advertisement>((advertisement) => advertisements[3] = advertisement);

            IAdvertisementManager _advertisementManager = new AdvertisementManager(_advertisementRepositoryMock.Object, _mapper);
            var boolResult = await _advertisementManager.UpdateAdvertisement(advertisementDTO);
            var updatedAdvertisement = advertisements.FirstOrDefault(x => x.AdvertisementId == 4);

            boolResult.Should().BeFalse();
            updatedAdvertisement.Should().BeNull();
        }

        [Fact]
        public async void UpdateAdvertisementStatus_UpdateAdvertisementStatus_AdvertisementExistInRepo()
        {
            var advertisements = AdvertisementsSample();
            AdvertisementDTO advertisementDTO = new()
            {
                AdvertisementId = 2,
                AdvertisementStatus = 2
            };
            _advertisementRepositoryMock.Setup(x => x.GetAdvertisementById(2)).ReturnsAsync(advertisements.FirstOrDefault(advertisement => advertisement.AdvertisementId == 2));
            _advertisementRepositoryMock.Setup(x => x.UpdateAdvertisement(It.IsAny<Advertisement>())).Callback<Advertisement>((advertisement) => advertisements[0] = advertisement);

            IAdvertisementManager _advertisementManager = new AdvertisementManager(_advertisementRepositoryMock.Object, _mapper);
            var boolResult = await _advertisementManager.UpdateAdvertisementStatus(advertisementDTO.AdvertisementId.Value , advertisementDTO.AdvertisementStatus);
            var updatedAdvertisement = advertisements.FirstOrDefault(x => x.AdvertisementId == 2);

            boolResult.Should().BeTrue();
            updatedAdvertisement.Should().NotBeNull();
            updatedAdvertisement!.AdvertisementStatus.Should().Be(advertisementDTO.AdvertisementStatus);
        }

        [Fact]
        public async void UpdateAdvertisementStatus_UpdateAdvertisementStatus_AdvertisementNotExistInRepo()
        {
            var advertisements = AdvertisementsSample();
            AdvertisementDTO advertisementDTO = new()
            {
                AdvertisementId = 4,
                AdvertisementStatus = 2
            };
            _advertisementRepositoryMock.Setup(x => x.GetAdvertisementById(4)).ReturnsAsync(advertisements.FirstOrDefault(advertisement => advertisement.AdvertisementId == 4));
            _advertisementRepositoryMock.Setup(x => x.UpdateAdvertisement(It.IsAny<Advertisement>())).Callback<Advertisement>((advertisement) => advertisements[0] = advertisement);

            IAdvertisementManager _advertisementManager = new AdvertisementManager(_advertisementRepositoryMock.Object, _mapper);
            var boolResult = await _advertisementManager.UpdateAdvertisementStatus(advertisementDTO.AdvertisementId.Value, advertisementDTO.AdvertisementStatus);
            var updatedAdvertisement = advertisements.FirstOrDefault(x => x.AdvertisementId == 4);

            boolResult.Should().BeFalse();
            updatedAdvertisement.Should().BeNull();
        }

        [Fact]
        public async void DeleteAdvertisement_DeleteAdvertisement_AdvertisementExistInRepo()
        {
            var advertisements = AdvertisementsSample();
            AdvertisementDTO advertisementDTO = new()
            {
                AdvertisementId = 2,
                AdvertisementStatus = 0
            };
            _advertisementRepositoryMock.Setup(x => x.GetAdvertisementById(2)).ReturnsAsync(advertisements.FirstOrDefault(advertisement => advertisement.AdvertisementId == 2));
            _advertisementRepositoryMock.Setup(x => x.DeleteAdvertisement(It.IsAny<Advertisement>())).Callback<Advertisement>(item => advertisements.Remove(item));

            IAdvertisementManager _advertisementManager = new AdvertisementManager(_advertisementRepositoryMock.Object, _mapper);
            var boolResult = await _advertisementManager.DeleteAdvertisement(advertisementDTO.AdvertisementId.Value);
            var actual = advertisements.ToList();

            boolResult.Should().BeTrue();
            actual.Count().Should().Be(2);
        }

        [Fact]
        public async void DeleteAdvertisement_DeleteAdvertisement_AdvertisementNotExistInRepo()
        {
            var advertisements = AdvertisementsSample();
            AdvertisementDTO advertisementDTO = new()
            {
                AdvertisementId = 4,
                AdvertisementStatus = 0
            };
            _advertisementRepositoryMock.Setup(x => x.GetAdvertisementById(4)).ReturnsAsync(advertisements.FirstOrDefault(advertisement => advertisement.AdvertisementId == 4));
            _advertisementRepositoryMock.Setup(x => x.UpdateAdvertisement(It.IsAny<Advertisement>())).Callback<Advertisement>((advertisement) => advertisements[3] = advertisement);

            IAdvertisementManager _advertisementManager = new AdvertisementManager(_advertisementRepositoryMock.Object, _mapper);
            var boolResult = await _advertisementManager.UpdateAdvertisement(advertisementDTO);
            var updatedAdvertisement = advertisements.FirstOrDefault(x => x.AdvertisementId == 4);

            boolResult.Should().BeFalse();
            updatedAdvertisement.Should().BeNull();
        }

        private static List<Advertisement> AdvertisementsSample()
        {
            List<Advertisement> output = [
                new Advertisement()
                {
                    AdvertisementId = 1,
                    AdvertisementImage = "mock-image-link",
                    AdvertisementLink = "mock-advertisement-link",
                    AdvertisementStatus = 1
                },
                new Advertisement()
                {
                    AdvertisementId = 2,
                    AdvertisementImage = "mock-image-link",
                    AdvertisementLink = "mock-advertisement-link",
                    AdvertisementStatus = 1
                },
                new Advertisement()
                {
                    AdvertisementId = 3,
                    AdvertisementImage = "mock-image-link",
                    AdvertisementLink = "mock-advertisement-link",
                    AdvertisementStatus = 1
                },
            ];
            return output;
        }
    }
}
