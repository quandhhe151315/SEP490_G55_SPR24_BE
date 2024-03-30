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
    public class MarketplaceRepository : IMarketplaceRepository
    {
        private readonly KitchenDelightsContext _context;

        public MarketplaceRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public async Task<Marketplace?> GetMarketplace(int id)
        {
            return await _context.Marketplaces.AsNoTracking().SingleOrDefaultAsync(m => m.MarketplaceId == id && m.MarketplaceStatus != 0);
        }

        public async Task<List<Marketplace>> GetMarketplaces()
        {
            return await _context.Marketplaces.AsNoTracking().Where(m => m.MarketplaceStatus != 0).ToListAsync();
        }

        public void CreateMarketplace(Marketplace marketplace)
        {
            try
            {
                _context.Marketplaces.Add(marketplace);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateMarketplace(Marketplace marketplace)
        {
            try
            {
                _context.Marketplaces.Update(marketplace);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
