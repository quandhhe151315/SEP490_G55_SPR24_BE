﻿using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IRecipeManager
    {
        Task CreateRecipe(RecipeDTO recipe);

        Task<bool> UpdateRecipe(RecipeDTO recipe);

        Task<bool> DeleteRecipe(int id);

        Task<RecipeDTO?> GetRecipe(int id);

        Task<List<RecipeDTO>> GetRecipeByTitle(string? title);

        Task<List<RecipeDTO>> GetRecipeByCategory(int category);

        Task<List<RecipeDTO>> GetRecipes();
    }
}
