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
    public class CountryManagerTest
    {
        private readonly Mock<ICountryRepository> _countryRepositoryMock;
        private readonly IMapper _mapper;

        public CountryManagerTest()
        {
            //Initial setup
            _countryRepositoryMock = new Mock<ICountryRepository>();
            _mapper = new Mapper(new MapperConfiguration(options =>
            {
                options.AddProfile<CountryProfile>();
            }));
        }

        [Fact]
        public async void GetCountry_GetCountryList_ExistInRepo()
        {
            var countries = CountriesSample();
            _countryRepositoryMock.Setup(x => x.GetCountries()).ReturnsAsync(countries.ToList());

            ICountryManager _countryManager = new CountryManager(_countryRepositoryMock.Object, _mapper);
            var result = await _countryManager.GetCountries();

            result.Should().BeOfType<List<CountryDTO>>()
            .And.NotBeNullOrEmpty();
            result.Count.Should().Be(3);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetCountry_GetCountryById_CountryExistInRepo()
        {
            //Arrange
            var countries = CountriesSample();
            List<CountryDTO> countryDTOs = [];
            countryDTOs.AddRange(countries.Select(_mapper.Map<Country, CountryDTO>));
            _countryRepositoryMock.Setup(x => x.GetCountry(1)).ReturnsAsync(countries.Find(x => x.CountryId == 1)); //Mock Advertisement repository GetAdvertisementById(int id) method

            //Act
            ICountryManager _countryManager = new CountryManager(_countryRepositoryMock.Object, _mapper);
            var result = await _countryManager.GetCountry(1);
            var actual = countryDTOs.Find(x => x.CountryId == 1);

            //Assert (using FluentAssertions)
            result.Should().NotBeNull().And.BeOfType<CountryDTO>().And.BeEquivalentTo(actual!);
        }

        [Fact]
        //Naming convention is MethodName_expectedBehavior_StateUnderTest
        public async void GetCountry_GetCountryById_CountryNotExistInRepo()
        {
            var countries = CountriesSample();
            _countryRepositoryMock.Setup(x => x.GetCountry(-1)).ReturnsAsync(countries.FirstOrDefault(x => x.CountryId == -1));

            ICountryManager _countryManager = new CountryManager(_countryRepositoryMock.Object, _mapper);
            var result = await _countryManager.GetCountry(-1);
            var actual = countries.FirstOrDefault(x => x.CountryId == -1);

            result.Should().BeNull();
            actual.Should().BeNull();
        }

        private static List<Country> CountriesSample()
        {
            List<Country> output = [
                new Country()
                {
                    CountryId = 1,
                    CountryName = "Việt Nam",
                    CountryStatus = 1
                },
                new Country()
                {
                    CountryId = 2,
                    CountryName = "Mỹ",
                    CountryStatus = 1
                },
                new Country()
                {
                    CountryId = 1,
                    CountryName = "Anh",
                    CountryStatus = 1
                },
            ];
            return output;
        }
    }
}
