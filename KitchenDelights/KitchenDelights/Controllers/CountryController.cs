using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ICountryManager _countryManager;

        public CountryController(IConfiguration configuration, ICountryManager countryManager)
        {
            _configuration = configuration;
            _countryManager = countryManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountry()
        {
            List<CountryDTO> countries = [];
            try
            {
                countries = await _countryManager.GetCountries();
                if (countries.Count <= 0)
                {
                    return NotFound("There are not exist any country in database");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(countries);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCountryById(int countryId)
        {
            try
            {
                if (countryId != 0)
                {
                    CountryDTO? country = await _countryManager.GetCountry(countryId);
                    if (country == null)
                    {
                        return NotFound("country not exist");
                    }
                    return Ok(country);
                }
                else
                {
                    List<CountryDTO> countries = await _countryManager.GetCountries();
                    if (countries.Count <= 0)
                    {
                        return NotFound("There are not exist any country in database");
                    }
                    return Ok(countries);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
