using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class RecipeManager : IRecipeManager
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public RecipeManager(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task CreateRecipe(RecipeRequestDTO recipe)
        {
            _recipeRepository.CreateRecipe(_mapper.Map<RecipeRequestDTO, Recipe>(recipe));
            _recipeRepository.Save();
        }

        public async Task<bool> DeleteRecipe(int id)
        {
            Recipe? recipe = await _recipeRepository.GetRecipe(id);
            if (recipe == null) return false;

            _recipeRepository.DeleteRecipe(recipe);
            _recipeRepository.Save();
            return true;
        }

        public async Task<RecipeDTO?> GetRecipe(int id)
        {
            Recipe? recipe = await _recipeRepository.GetRecipe(id);
            return recipe is null ? null : _mapper.Map<Recipe, RecipeDTO>(recipe);
        }

        public async Task<List<RecipeDTO>> GetRecipeByCategory(int category)
        {
            List<Recipe>? recipes = await _recipeRepository.GetRecipeByCategory(category);
            List<RecipeDTO> recipeDTOs = [];
            foreach (Recipe recipe in recipes)
            {
                recipeDTOs.Add(_mapper.Map<Recipe, RecipeDTO>(recipe));
            }
            return recipeDTOs;
        }

        public async Task<List<RecipeDTO>> GetRecipeByTitle(string? title)
        {
            List<Recipe>? recipes = await _recipeRepository.GetRecipeByTitle(title);
            List<RecipeDTO> recipeDTOs = [];
            foreach (Recipe recipe in recipes)
            {
                recipeDTOs.Add(_mapper.Map<Recipe, RecipeDTO>(recipe));
            }
            return recipeDTOs;
        }

        public async Task<List<RecipeDTO>> GetRecipes()
        {
            List<Recipe> recipes = await _recipeRepository.GetRecipes();
            List<RecipeDTO> recipeDTOs = [];
            foreach (Recipe recipe in recipes)
            {
                recipeDTOs.Add(_mapper.Map<Recipe, RecipeDTO>(recipe));
            }
            return recipeDTOs;
        }

        public async Task<bool> UpdateRecipe(RecipeRequestDTO recipeDTO)
        {
            Recipe? recipe = await _recipeRepository.GetRecipe(recipeDTO.RecipeId.Value);
            if (recipe == null)
            {
                return false;
            }
            recipe.RecipeIngredients.Clear();
            _recipeRepository.Save();
            recipe = _mapper.Map<RecipeRequestDTO, Recipe>(recipeDTO);
            _recipeRepository.UpdateRecipe(recipe);
            _recipeRepository.Save();
            return true;
        }
    }
}
