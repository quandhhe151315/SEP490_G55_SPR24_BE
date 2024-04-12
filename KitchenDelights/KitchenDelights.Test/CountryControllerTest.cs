using Business.DTO;
using Business.Interfaces;
using FluentAssertions.AspNetCore.Mvc;
using KitchenDelights.Controllers;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KitchenDelights.Test
{
    public class CountryControllerTest
    {
        private Mock<ICountryManager> _mockCountryManager;
        private IConfiguration _configuration;

        public CountryControllerTest()
        {
            _mockCountryManager = new Mock<ICountryManager>();
            _configuration = new ConfigurationBuilder().Build();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetCountries()
        {
            _mockCountryManager.Setup(x => x.GetCountries()).ReturnsAsync(new List<CountryDTO>(){
                new CountryDTO() {
                    CountryId = 1,
                    CountryName = "Việt Nam",
                },
                new CountryDTO() {
                    CountryId = 1,
                    CountryName = "Mỹ",
                },
                new CountryDTO() {
                    CountryId = 1,
                    CountryName = "Canada",
                },
            });

            CountryController _controller = new(_configuration, _mockCountryManager.Object);
            var result = await _controller.GetAllCountry();

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus200_GetCountryById()
        {
            _mockCountryManager.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryDTO()
            {
                CountryId = 1,
                CountryName = "Việt Nam",
            });

            CountryController _controller = new(_configuration, _mockCountryManager.Object);
            var result = await _controller.GetAllCountryById(1);

            result.Should().BeOkObjectResult();
        }

        [Fact]
        public async void Get_ReturnStatus404_GetCountryById()
        {
            _mockCountryManager.Setup(x => x.GetCountry(-1));

            CountryController _controller = new(_configuration, _mockCountryManager.Object);
            var result = await _controller.GetAllCountryById(-1);

            result.Should().BeNotFoundObjectResult();
        }
    }
}
