using Business.DTO;
using Business.Interfaces;
using Business.Managers;
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
                countries = _countryManager.GetCountries();
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
        public async Task<IActionResult> GetAllCountryById(int id)
        {
            CountryDTO country;
            try
            {
                country = _countryManager.GetCountry(id);
                if (country == null)
                {
                    return NotFound("country not exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(country);
        }
    }
}
