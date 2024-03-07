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
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;

        public RecipeManager(IRecipeRepository recipeRepository, ICategoryRepository categoryRepository, ICountryRepository countryRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _categoryRepository = categoryRepository;
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task CreateRecipe(RecipeRequestDTO recipeDTO)
        {
            Country country = _countryRepository.GetCountry(recipeDTO.CountryId);
            Recipe recipe = _mapper.Map<RecipeRequestDTO, Recipe>(recipeDTO);
            recipe.Countries.Add(country);
            _recipeRepository.CreateRecipe(recipe);
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

        public async Task<List<RecipeDTO>> GetRecipeFree()
        {
            List<Recipe> recipes = await _recipeRepository.GetRecipeFree();
            List<RecipeDTO> recipeDTOs = [];
            foreach (Recipe recipe in recipes)
            {
                recipeDTOs.Add(_mapper.Map<Recipe, RecipeDTO>(recipe));
            }
            return recipeDTOs;
        }

        public async Task<List<RecipeDTO>> GetRecipePaid()
        {
            List<Recipe> recipes = await _recipeRepository.GetRecipePaid();
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

        public async Task<bool> UpdateCategoryRecipe(int recipeId, int categoryId, int type)
        {
            Recipe? recipe = await _recipeRepository.GetRecipe(recipeId);
            Category? category = _categoryRepository.GetCategoryById(categoryId);
            if (recipe == null || category == null) return false;
            switch (type)
            {
                case 1:
                    recipe.Categories.Add(category);
                    _recipeRepository.UpdateRecipe(recipe);
                    break;
                case 2:
                    recipe.Categories.Remove(category);
                    _recipeRepository.UpdateRecipe(recipe);
                    break;
            }
            _recipeRepository.Save();
            return true;
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

        public async Task<bool> UpdateStatusRecipe(int recipeId, bool status)
        {
            Recipe? recipe = await _recipeRepository.GetRecipe(recipeId);
            if (recipe == null)
            {
                return false;
            }
            recipe.RecipeStatus = status;
            _recipeRepository.UpdateRecipe(recipe);
            _recipeRepository.Save();
            return true;
        }
    }
}
