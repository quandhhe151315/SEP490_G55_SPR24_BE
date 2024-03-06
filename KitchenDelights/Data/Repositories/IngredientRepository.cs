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
                if (ingredient != null)
                {
                    _context.Ingredients.Remove(ingredient);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public List<Ingredient> GetAllIngredients()
        {
            List<Ingredient> ingredients = new List<Ingredient>();
            try
            {
                ingredients = _context.Ingredients.Include(x => x.IngredientMarketplaces).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return ingredients;
        }

        public Ingredient GetIngredientById(int ingredientId)
        {
            Ingredient ingredient = new Ingredient();
            try
            {
                ingredient = _context.Ingredients.Include(x => x.IngredientMarketplaces).FirstOrDefault(ingredient => ingredient.IngredientId == ingredientId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return ingredient;
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
