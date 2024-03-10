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
    public class RecipeRepository : IRecipeRepository
    {
        private KitchenDelightsContext _context;

        public RecipeRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public void CreateRecipe(Recipe recipe)
        {
            try
            {
                recipe.RecipeRating = 0;
                _context.Recipes.Add(recipe);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteRecipe(Recipe recipe)
        {
            try
            {
                _context.Recipes.Remove(recipe);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task<Recipe?> GetRecipe(int id)
        {
            return await _context.Recipes
                .Include(x => x.CartItems).Include(x => x.PaymentHistories)
                .Include(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient).Include(x => x.RecipeRatings)
                .Include(x => x.Categories).Include(x => x.Countries)
                .Include(x => x.Menus).Include(x => x.Users).Include(x => x.User)
                .FirstOrDefaultAsync(x => x.RecipeId == id);
        }

        public async Task<List<Recipe>> GetRecipeByCategory(int categoryId)
        {
            List<Recipe> recipes = [];
            recipes = await _context.Recipes.AsNoTracking()
                .Include(x => x.CartItems).Include(x => x.PaymentHistories)
                .Include(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient).Include(x => x.RecipeRatings)
                .Include(x => x.Categories).Include(x => x.Countries)
                .Include(x => x.Menus).Include(x => x.Users).Include(x => x.User)
                .Where(x => x.Categories.Any(x => x.CategoryId == categoryId))
                .ToListAsync();
            return recipes;
        }

        public async Task<List<Recipe>> GetRecipeByCountry(int countryId)
        {
            List<Recipe> recipes = [];
            recipes = await _context.Recipes.AsNoTracking()
                .Include(x => x.CartItems).Include(x => x.PaymentHistories)
                .Include(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient).Include(x => x.RecipeRatings)
                .Include(x => x.Categories).Include(x => x.Countries)
                .Include(x => x.Menus).Include(x => x.Users).Include(x => x.User)
                .Where(x => x.Countries.Any(x => x.CountryId == countryId))
                .ToListAsync();
            return recipes;
        }

        public async Task<List<Recipe>> GetRecipeByTitle(string? title)
        {
            List<Recipe> recipes = [];
            recipes = await _context.Recipes.AsNoTracking()
                .Include(x => x.CartItems).Include(x => x.PaymentHistories)
                .Include(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient).Include(x => x.RecipeRatings)
                .Include(x => x.Categories).Include(x => x.Countries)
                .Include(x => x.Menus).Include(x => x.Users).Include(x => x.User)
                .Where(x => x.RecipeTitle.Contains(title))
                .ToListAsync();
            return recipes;
        }

        public async Task<List<Recipe>> GetRecipeFree()
        {
            List<Recipe> recipes = [];
            recipes = await _context.Recipes.AsNoTracking()
                .Include(x => x.CartItems).Include(x => x.PaymentHistories)
                .Include(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient).Include(x => x.RecipeRatings)
                .Include(x => x.Categories).Include(x => x.Countries)
                .Include(x => x.Menus).Include(x => x.Users).Include(x => x.User)
                .Where(x => x.IsFree == true)
                .ToListAsync();
            return recipes;
        }

        public async Task<List<Recipe>> GetRecipePaid()
        {
            List<Recipe> recipes = [];
            recipes = await _context.Recipes.AsNoTracking()
                .Include(x => x.CartItems).Include(x => x.PaymentHistories)
                .Include(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient).Include(x => x.RecipeRatings)
                .Include(x => x.Categories).Include(x => x.Countries)
                .Include(x => x.Menus).Include(x => x.Users).Include(x => x.User)
                .Where(x => x.IsFree == false)
                .ToListAsync();
            return recipes;
        }

        public async Task<List<Recipe>> GetRecipes()
        {
            
            return await _context.Recipes.AsNoTracking()
                .Include(x => x.CartItems).Include(x => x.PaymentHistories)
                .Include(x => x.RecipeIngredients).ThenInclude(x => x.Ingredient).Include(x => x.RecipeRatings)
                .Include(x => x.Categories).Include(x => x.Countries)
                .Include(x => x.Menus).Include(x => x.Users).Include(x => x.User)
                .ToListAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateRecipe(Recipe recipe)
        {
            try
            {
                _context.Recipes.Update(recipe);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
