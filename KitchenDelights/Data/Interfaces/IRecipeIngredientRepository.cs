using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IRecipeIngredientRepository
    {
        void UpdateRecipeIngredient(RecipeIngredient recipeIngredient);

        void RemoveRecipeIngredient(List<RecipeIngredient> recipeIngredients);

        Task<List<RecipeIngredient>> GetRecipeIngredient(int recipeId);

        void Save();
    }
}
