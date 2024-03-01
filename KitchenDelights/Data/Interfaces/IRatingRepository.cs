using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRatingRepository
    {
        Task<RecipeRating?> GetRating(int id);
        Task<List<RecipeRating>> GetRatings(int id);
        void CreateRating(RecipeRating rating);
        void UpdateRating(RecipeRating rating);
        void Save();
    }
}
