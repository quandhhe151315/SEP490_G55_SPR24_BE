using Business.DTO;
using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICountryManager
    {
        Task<List<CountryDTO>> GetCountries();
        Task<CountryDTO?> GetCountry(int id);
    }
}
