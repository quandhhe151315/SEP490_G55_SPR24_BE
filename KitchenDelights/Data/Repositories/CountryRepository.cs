using Data.Entity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<Country>> GetCountries()
        {
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountry(int id)
        {
            return await _context.Countries.FirstOrDefaultAsync(x => x.CountryId == id);
        }
    }
}
