using Data.Entity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private KitchenDelightsContext _context;

        public AdvertisementRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public void CreateAdvertisement(Advertisement advertisement)
        {
            try
            {
                _context.Advertisements.Add(advertisement);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteAdvertisement(Advertisement advertisement)
        {
            try
            {
                _context.Advertisements.Remove(advertisement);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task<Advertisement?> GetAdvertisementById(int id)
        {
            return await _context.Advertisements.FirstOrDefaultAsync(x => x.AdvertisementId == id);
        }

        public async Task<List<Advertisement>> GetAdvertisements()
        {
            return await _context.Advertisements.ToListAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateAdvertisement(Advertisement advertisement)
        {
            try
            {
                _context.Advertisements.Update(advertisement);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
