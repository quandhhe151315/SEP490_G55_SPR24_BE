using Data.Entity;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly KitchenDelightsContext _context;

        public CountryRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public List<Country> GetCountries()
        {
            List<Country> countries = _context.Countries.ToList();
            return countries;
        }

        public Country GetCountry(int id)
        {
            Country? country = _context.Countries.FirstOrDefault(x => x.CountryId == id);
            return country;
        }
    }
}
