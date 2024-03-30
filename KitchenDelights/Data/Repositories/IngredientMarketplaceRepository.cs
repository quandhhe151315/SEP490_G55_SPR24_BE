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
    public class IngredientMarketplaceRepository : IIngredientMarketplaceRepository
    {
        private readonly KitchenDelightsContext _context;

        public IngredientMarketplaceRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public async Task<IngredientMarketplace?> GetIngredientMarketplace(int ingredientId, int marketplaceId)
        {
            return await _context.IngredientMarketplaces.AsNoTracking()
                .Include(x => x.Marketplace).Include(x => x.Ingredient)
                .FirstOrDefaultAsync(x => x.IngredientId == ingredientId && x.MarketplaceId == marketplaceId);
        }

        public async Task<List<IngredientMarketplace>> GetIngredientMarketplaces()
        {
            return await _context.IngredientMarketplaces.AsNoTracking()
                .Include(x => x.Marketplace).Include(x => x.Ingredient).ToListAsync();
        }

        public void CreateIngredientMarketplace(IngredientMarketplace ingmarketplace)
        {
            try
            {
                _context.IngredientMarketplaces.Add(ingmarketplace);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
        public void UpdateIngredientMarketplace(IngredientMarketplace ingmarketplace)
        {
            try
            {
                _context.IngredientMarketplaces.Update(ingmarketplace);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteIngredientMarketplace(IngredientMarketplace ingmarketplace)
        {
            try
            {
                _context.IngredientMarketplaces.Remove(ingmarketplace);
            }
            catch (Exception ex)
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
