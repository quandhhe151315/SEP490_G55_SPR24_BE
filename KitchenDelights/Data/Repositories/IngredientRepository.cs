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
    public class IngredientRepository : IIngredientRepository
    {
        private readonly KitchenDelightsContext _context;

        public IngredientRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public void CreateIngredient(Ingredient ingredient)
        {
            try
            {
                _context.Ingredients.Add(ingredient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteIngredient(Ingredient ingredient)
        {
            try
            {
                _context.Ingredients.Remove(ingredient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task<List<Ingredient>> GetAllIngredients()
        {
            return await _context.Ingredients.Include(x => x.IngredientMarketplaces).ToListAsync();
        }

        public async Task<Ingredient?> GetIngredientById(int ingredientId)
        {
            return await _context.Ingredients.Include(x => x.IngredientMarketplaces)
                .FirstOrDefaultAsync(ingredient => ingredient.IngredientId == ingredientId);
        }

        public async Task<List<Ingredient>> GetIngredientByName(string name)
        {
            return await _context.Ingredients.Include(x => x.IngredientMarketplaces).Where(x => x.IngredientName.Contains(name)).ToListAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateIngredient(Ingredient ingredient)
        {
            try
            {
                _context.Ingredients.Update(ingredient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
