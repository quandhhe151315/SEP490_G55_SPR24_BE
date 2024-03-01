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
    public class RatingRepository : IRatingRepository
    {
        private readonly KitchenDelightsContext _context;

        public RatingRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public async Task<RecipeRating?> GetRating(int id)
        {
            return await _context.RecipeRatings.AsNoTracking().Include(x => x.User).FirstOrDefaultAsync(x => x.RatingId == id);
        }

        public async Task<List<RecipeRating>> GetRatings(int id)
        {
            return await _context.RecipeRatings.AsNoTracking().Include(x => x.User).Where(rating => rating.RecipeId == id).ToListAsync();
        }

        public void CreateRating(RecipeRating rating)
        {
            try
            {
                _context.RecipeRatings.Add(rating);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }


        public void UpdateRating(RecipeRating rating)
        {
            try
            {
                _context.RecipeRatings.Update(rating);
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
