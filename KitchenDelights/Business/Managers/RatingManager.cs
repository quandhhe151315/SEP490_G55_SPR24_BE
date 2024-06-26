﻿using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class RatingManager : IRatingManager
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public RatingManager(IRatingRepository ratingRepository, IRecipeRepository recipeRepository, IMapper mapper)
        {
            _ratingRepository = ratingRepository;
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<List<RecipeRatingDTO>> GetRecipeRatings(int id)
        {
            List<RecipeRatingDTO> ratingDTOs = [];
            List<RecipeRating> ratings = await _ratingRepository.GetRatings(id);

            foreach(RecipeRating rating in ratings)
            {
                ratingDTOs.Add(_mapper.Map<RecipeRating, RecipeRatingDTO>(rating));
            }

            return ratingDTOs;
        }

        public async Task<bool> CreateRating(RecipeRatingDTO ratingDTO)
        {
            //Get Recipe total rating
            Recipe? recipe = await _recipeRepository.GetRecipe(ratingDTO.RecipeId);
            List<RecipeRating> ratings = await _ratingRepository.GetRatings(ratingDTO.RecipeId);
            //Check if user already have active review
            foreach(RecipeRating rate in ratings) {
                if (rate.UserId == ratingDTO.UserId) return false;
            }
            if (recipe == null) return false;
            decimal totalRating = recipe.RecipeRating * ratings.Count;

            ratingDTO.CreateDate = DateTime.Now;
            _ratingRepository.CreateRating(_mapper.Map<RecipeRatingDTO, RecipeRating>(ratingDTO));
            _ratingRepository.Save();

            //Update Recipe average rating
            recipe.RecipeRating = (totalRating + ratingDTO.RatingValue) / (ratings.Count + 1);
            _recipeRepository.UpdateRecipe(recipe);
            _recipeRepository.Save();
            return true;
        }

        public async Task<bool> UpdateRating(RecipeRatingDTO ratingDTO)
        {
            //Get Recipe total rating
            Recipe? recipe = await _recipeRepository.GetRecipe(ratingDTO.RecipeId);
            List<RecipeRating> ratings = await _ratingRepository.GetRatings(ratingDTO.RecipeId);
            if (recipe == null) return false;
            decimal totalRating = recipe.RecipeRating * ratings.Count;

            //Update rating
            RecipeRating? rating = await _ratingRepository.GetRating(ratingDTO.RatingId.Value);
            if(rating == null) return false;

            //Update Recipe average rating
            recipe.RecipeRating = (totalRating - rating.RatingValue + ratingDTO.RatingValue) / ratings.Count;
            _recipeRepository.UpdateRecipe(recipe);

            rating.RatingValue = ratingDTO.RatingValue;
            rating.RatingContent = ratingDTO.RatingContent;
            _ratingRepository.UpdateRating(rating);
            _ratingRepository.Save();

            return true;
        }

        public async Task<bool> DeleteRating(int recipeId, int ratingId) {
            decimal totalRating = 0m;
            Recipe? recipe = await _recipeRepository.GetRecipe(recipeId);
            if (recipe == null) return false;

            List<RecipeRating> ratings = await _ratingRepository.GetRatings(recipeId);
            int count = ratings.Count;

            //Get actual total rating value
            foreach (RecipeRating tempRating in ratings) {
                totalRating += tempRating.RatingValue;
            }
            
            //Get Rating object to delete
            RecipeRating? rating = await _ratingRepository.GetRating(ratingId);
            if(rating == null) return false;
            
            //Update average rating
            if(count - 1 == 0) {
                recipe.RecipeRating = 0;
            } else {
                recipe.RecipeRating = (totalRating - rating.RatingValue) / (count - 1);
            }
            _recipeRepository.UpdateRecipe(recipe);

            //Soft delete rating
            rating.RatingStatus = 0;
            _ratingRepository.UpdateRating(rating);
            _ratingRepository.Save();
            return true;
        }
    }
}
