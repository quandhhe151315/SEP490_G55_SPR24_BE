using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using FluentAssertions;
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

namespace KitchenDelights.Test
{
    public class AdvertisementControllerTest
    {
        private Mock<IAdvertisementManager> _mockAdvertisementManager;
        private IConfiguration _configuration;

        public AdvertisementControllerTest()
        {
            _mockAdvertisementManager = new Mock<IAdvertisementManager>();
            _configuration = new ConfigurationBuilder().Build();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetAdvertisementById()
        {
            _mockAdvertisementManager.Setup(x => x.GetAdvertisementById(1)).ReturnsAsync(new AdvertisementDTO()
            {
                AdvertisementId = 1,
                AdvertisementImage = "image",
                AdvertisementLink = "link",
                AdvertisementStatus = 1
            });

            AdvertisementController _controller = new AdvertisementController(_configuration, _mockAdvertisementManager.Object);
            var result = await _controller.GetAdvertismentById(1);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus404_GetAdvertisementById()
        {
            _mockAdvertisementManager.Setup(x => x.GetAdvertisementById(0));

            AdvertisementController _controller = new AdvertisementController(_configuration, _mockAdvertisementManager.Object);
            var result = await _controller.GetAdvertismentById(0);

            result.Should().BeNotFoundObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetAdvertismentActive()
        {
            _mockAdvertisementManager.Setup(x => x.GetAdvertisements()).ReturnsAsync([
                new AdvertisementDTO()
                {
                    AdvertisementId = 1,
                    AdvertisementImage = "mock-image-link",
                    AdvertisementLink = "mock-advertisement-link",
                    AdvertisementStatus = 1
                },
                new AdvertisementDTO()
                {
                    AdvertisementId = 2,
                    AdvertisementImage = "mock-image-link",
                    AdvertisementLink = "mock-advertisement-link",
                    AdvertisementStatus = 1
                },
                new AdvertisementDTO()
                {
                    AdvertisementId = 3,
                    AdvertisementImage = "mock-image-link",
                    AdvertisementLink = "mock-advertisement-link",
                    AdvertisementStatus = 1
                }
            ]);

            AdvertisementController _controller = new AdvertisementController(_configuration, _mockAdvertisementManager.Object);
            var result = await _controller.GetAdvertismentActive();

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus404_GetAdvertismentActive()
        {
            _mockAdvertisementManager.Setup(x => x.GetAdvertisements());

            AdvertisementController _controller = new AdvertisementController(_configuration, _mockAdvertisementManager.Object);
            var result = await _controller.GetAdvertismentActive();

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Create_ReturnStatus200_AllValid()
        {
            AdvertisementDTO toAdd = new AdvertisementDTO()
            {
                AdvertisementId = 1,
                AdvertisementImage = "mock-image-link",
                AdvertisementLink = "mock-advertisement-link",
                AdvertisementStatus = 1
            };
            _mockAdvertisementManager.Setup(x => x.CreateAdvertisement(It.IsAny<AdvertisementDTO>()));

            AdvertisementController _controller = new AdvertisementController(_configuration, _mockAdvertisementManager.Object);
            var result = await _controller.Create(toAdd);

            result.Should().BeObjectResult();
        }

        [Fact]
        public async void Create_ReturnStatus500_EmptyImage()
        {
            AdvertisementDTO toAdd = new AdvertisementDTO()
            {
                AdvertisementId = 1,
                AdvertisementImage = string.Empty,
                AdvertisementLink = "mock-advertisement-link",
                AdvertisementStatus = 1
            };
            _mockAdvertisementManager.Setup(x => x.CreateAdvertisement(It.IsAny<AdvertisementDTO>()));

            AdvertisementController _controller = new AdvertisementController(_configuration, _mockAdvertisementManager.Object);
            var result = await _controller.Create(toAdd);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(500);
        }

        [Fact]
        public async void Create_ReturnStatus500_EmptyLink()
        {
            AdvertisementDTO toAdd = new AdvertisementDTO()
            {
                AdvertisementId = 1,
                AdvertisementImage = "mock-image-link",
                AdvertisementLink = string.Empty,
                AdvertisementStatus = 1
            };
            _mockAdvertisementManager.Setup(x => x.CreateAdvertisement(It.IsAny<AdvertisementDTO>()));

            AdvertisementController _controller = new AdvertisementController(_configuration, _mockAdvertisementManager.Object);
            var result = await _controller.Create(toAdd);

            result.Should().BeObjectResult();
            (result as ObjectResult)!.StatusCode.Should().Be(500);
        }

        [Fact]
        public async void Update_ReturnStatus200_AllValid()
        {
            AdvertisementDTO toUpdate = new AdvertisementDTO()
            {
                AdvertisementId = 1,
                AdvertisementImage = "mock-image-link-update",
                AdvertisementLink = "mock-advertisement-link-update",
                AdvertisementStatus = 1
            };
            _mockAdvertisementManager.Setup(x => x.UpdateAdvertisement(It.IsAny<AdvertisementDTO>())).ReturnsAsync(true);

            AdvertisementController _controller = new AdvertisementController(_configuration, _mockAdvertisementManager.Object);
            var result = await _controller.Update(toUpdate);

            result.Should().BeOkResult();
        }
    }
}
