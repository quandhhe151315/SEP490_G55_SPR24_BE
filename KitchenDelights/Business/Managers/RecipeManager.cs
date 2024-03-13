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
        private readonly IUserRepository _userRepository;
        private readonly IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly IMapper _mapper;

        public RecipeManager(IRecipeRepository recipeRepository, 
                            ICategoryRepository categoryRepository, 
                            ICountryRepository countryRepository,
                            IUserRepository userRepository,
                            IRecipeIngredientRepository recipeIngredientRepository,
                            IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _categoryRepository = categoryRepository;
            _countryRepository = countryRepository;
            _userRepository = userRepository;
            _recipeIngredientRepository = recipeIngredientRepository;
            _mapper = mapper;
        }

        public async Task CreateRecipe(RecipeRequestDTO recipeDTO)
        {
            Country? country = await _countryRepository.GetCountry(recipeDTO.CountryId);
            Recipe recipe = _mapper.Map<RecipeRequestDTO, Recipe>(recipeDTO);
            recipe.Countries.Add(country);
            recipe.RecipeRating = 0;
            recipe.RecipeStatus = 1;
            recipe.CreateDate = DateTime.Now;
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

        public async Task<List<RecipeDTO>> GetRecipeByCountry(int country)
        {
            List<Recipe>? recipes = await _recipeRepository.GetRecipeByCountry(country);
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

        public async Task<List<RecipeDTO>> GetRecipesASC()
        {
            List<Recipe> recipes = await _recipeRepository.GetRecipesASC();
            List<RecipeDTO> recipeDTOs = [];
            foreach (Recipe recipe in recipes)
            {
                recipeDTOs.Add(_mapper.Map<Recipe, RecipeDTO>(recipe));
            }
            return recipeDTOs;
        }

        public async Task<List<RecipeDTO>> GetRecipesDESC()
        {
            List<Recipe> recipes = await _recipeRepository.GetRecipesDESC();
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
            Category? category = await _categoryRepository.GetCategoryById(categoryId);
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
            try
            {
                Recipe? recipe = await _recipeRepository.GetRecipe(recipeDTO.RecipeId.Value);

                if (recipe == null)
                {
                    return false;
                }

                //recipe = _mapper.Map<RecipeRequestDTO, Recipe>(recipeDTO);

                recipe.RecipeId = recipeDTO.RecipeId.Value;
                recipe.FeaturedImage = recipeDTO.FeaturedImage;
                recipe.RecipeDescription = recipeDTO?.RecipeDescription;
                recipe.VideoLink = recipeDTO?.VideoLink;
                recipe.RecipeTitle = recipeDTO?.RecipeTitle;
                recipe.PreparationTime = recipeDTO.PreparationTime.Value;
                recipe.CookTime = recipeDTO.CookTime.Value;
                recipe.RecipeServe = recipeDTO?.RecipeServe.Value;
                recipe.RecipeContent = recipeDTO?.RecipeContent;
                recipe.IsFree = recipeDTO.IsFree;
                recipe.RecipePrice = recipeDTO.RecipePrice.Value;
                Country? country = await _countryRepository.GetCountry(recipeDTO.CountryId);
                Country? countryRecipe = recipe.Countries.ElementAt(0);
                recipe.Countries.Remove(countryRecipe);
                recipe.Countries.Add(country);

                List<RecipeIngredient> recipeIngredients = await _recipeIngredientRepository.GetRecipeIngredient(recipeDTO.RecipeId.Value);
                _recipeIngredientRepository.RemoveRecipeIngredient(recipeIngredients);

                foreach(RecipeIngredientRequestDTO recipeIngredient in recipeDTO.RecipeIngredients)
                {
                    recipe.RecipeIngredients.Add(_mapper.Map<RecipeIngredientRequestDTO, RecipeIngredient>(recipeIngredient));
                }

                _recipeRepository.UpdateRecipe(recipe);
                _recipeRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateStatusRecipe(int recipeId, int status)
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
