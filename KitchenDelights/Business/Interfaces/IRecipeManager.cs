using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRecipeManager
    {
        Task CreateRecipe(RecipeRequestDTO recipe);

        Task<bool> UpdateRecipe(RecipeRequestDTO recipe);

        Task<bool> UpdateStatusRecipe(int recipeId, int status);

        Task<bool> UpdateCategoryRecipe(int recipeId, int categoryId, int type);

        Task<bool> DeleteRecipe(int id);

        Task<RecipeDTO?> GetRecipe(int id);

        Task<List<RecipeDTO>> GetRecipeByTitle(string? title);

        Task<List<RecipeDTO>> GetRecipeByCategory(int category);

        Task<List<RecipeDTO>> GetRecipeByCountry(int country);

        Task<List<RecipeDTO>> GetRecipeFree();

        Task<List<RecipeDTO>> GetRecipePaid();

        Task<List<RecipeDTO>> GetRecipes();
    }
}
