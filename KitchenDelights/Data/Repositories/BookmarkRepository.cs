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
    public class BookmarkRepository : IBookmarkRepository
    {
        private readonly KitchenDelightsContext _context;

        public BookmarkRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public async Task<User?> GetBookmarkOfUser(int id)
        {
            return await _context.Users.AsNoTracking().Include(x => x.Recipes).ThenInclude(x => x.RecipeIngredients)
                                                    .Include(x => x.Recipes).ThenInclude(x => x.RecipeRatings)
                                                    .Include(x => x.Recipes).ThenInclude(x => x.Categories)
                                                    .Include(x => x.Recipes).ThenInclude(x => x.Countries)
                                                    .FirstOrDefaultAsync(x => x.UserId == id);
        }

        //public async void AddRecipeToBookmark(int userId, int recipeId)
        //{
        //    try
        //    {
        //        User user = await _context.Users.AsNoTracking().Include(x => x.Recipes).FirstOrDefaultAsync(x => x.UserId == userId);
        //        Recipe recipe = await _context.Recipes.AsNoTracking().FirstOrDefaultAsync(x => x.RecipeId == recipeId);
        //        user.Recipes.Add(recipe);
        //        _context.Users.Update(user);
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.StackTrace);
        //    }
        //}

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
