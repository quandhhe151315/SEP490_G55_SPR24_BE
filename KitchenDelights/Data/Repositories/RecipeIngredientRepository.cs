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
    public class RecipeIngredientRepository : IRecipeIngredientRepository
    {
        private readonly KitchenDelightsContext _context;

        public RecipeIngredientRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public void UpdateRecipeIngredient(RecipeIngredient recipeIngredient)
        {
            try
            {
                _context.RecipeIngredients.Update(recipeIngredient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void RemoveRecipeIngredient(List<RecipeIngredient> recipeIngredient)
        {
            try
            {
                _context.RecipeIngredients.RemoveRange(recipeIngredient);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task<List<RecipeIngredient>> GetRecipeIngredient(int recipeId)
        {
            return await _context.RecipeIngredients.Where(x => x.RecipeId == recipeId).ToListAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
