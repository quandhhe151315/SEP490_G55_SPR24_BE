using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class CountryManager : ICountryManager
    {
        ICountryRepository _countryRepository;
        private IMapper _mapper;

        public CountryManager(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }
        public async Task<List<CountryDTO>> GetCountries()
        {
            List<Country> countries = await _countryRepository.GetCountries();
            List<CountryDTO> countryDTOs = new List<CountryDTO>();
            foreach(Country country in countries)
            {
                countryDTOs.Add(_mapper.Map<Country, CountryDTO>(country));
            }
            return countryDTOs;
        }

        public async Task<CountryDTO?> GetCountry(int id)
        {
            Country? country = await _countryRepository.GetCountry(id);
            return country == null ? null : _mapper.Map<Country, CountryDTO>(country);
        }
    }
}
