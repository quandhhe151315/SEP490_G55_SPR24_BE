using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICountryRepository
    {
        Task<List<Country>> GetCountries();
        Task<Country?> GetCountry(int id);
    }
}
