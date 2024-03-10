using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRecipeRepository
    {
        void CreateRecipe(Recipe recipe);
        void UpdateRecipe(Recipe recipe);
        void DeleteRecipe(Recipe recipe);
        Task<Recipe?> GetRecipe(int id);
        Task<List<Recipe>> GetRecipeByTitle(string? title);
        Task<List<Recipe>> GetRecipeByCategory(int categoryId);
        Task<List<Recipe>> GetRecipeByCountry(int countryId);
        Task<List<Recipe>> GetRecipeFree();
        Task<List<Recipe>> GetRecipePaid();
        Task<List<Recipe>> GetRecipes();
        void Save();
    }
}
