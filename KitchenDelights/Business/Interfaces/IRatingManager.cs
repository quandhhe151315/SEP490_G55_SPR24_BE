using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRatingManager
    {
        Task<List<RecipeRatingDTO>> GetRecipeRatings(int id);
        Task<bool> CreateRating(RecipeRatingDTO ratingDTO);
        Task<bool> UpdateRating(RecipeRatingDTO ratingDTO);
    }
}
