using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class IngredientManager : IIngredientManager
    {
        private readonly IIngredientRepository _ingredientRepository;
        private readonly IMapper _mapper;

        public IngredientManager(IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public async Task<bool> CreateIngredient(IngredientRequestDTO ingredient)
        {
            try
            {
                _ingredientRepository.CreateIngredient(_mapper.Map<IngredientRequestDTO, Ingredient>(ingredient));
                _ingredientRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteIngredient(int id)
        {
            Ingredient? ingredient = await _ingredientRepository.GetIngredientById(id);
            if (ingredient == null) return false;

            ingredient.IngredientStatus = 0;
            _ingredientRepository.UpdateIngredient(ingredient);
            _ingredientRepository.Save();
            return true;
        }

        public async Task<List<IngredientDTO>> GetAllIngredients()
        {
            List<Ingredient> ingredients = await _ingredientRepository.GetAllIngredients();
            List<IngredientDTO> ingredientDTOs = [];
            foreach (Ingredient ingredient in ingredients)
            {
                ingredientDTOs.Add(_mapper.Map<Ingredient, IngredientDTO>(ingredient));
            }
            ingredientDTOs = ingredientDTOs.GroupBy(x => x.IngredientName).Select(x => x.First()).ToList();
            return ingredientDTOs;
        }

        public async Task<IngredientDTO?> GetIngredientById(int ingredientId)
        {
            Ingredient? ingredient = await _ingredientRepository.GetIngredientById(ingredientId);
            return ingredient == null ? null : _mapper.Map<Ingredient, IngredientDTO>(ingredient);
        }

        public async Task<List<IngredientDTO>> GetIngredientsByName(string name)
        {
            List<Ingredient> ingredients = await _ingredientRepository.GetIngredientByName(name);
            List<IngredientDTO> ingredientDTOs = [];
            foreach (Ingredient ingredient in ingredients)
            {
                ingredientDTOs.Add(_mapper.Map<Ingredient, IngredientDTO>(ingredient));
            }
            ingredientDTOs = ingredientDTOs.GroupBy(x => x.IngredientName).Select(x => x.First()).ToList();
            return ingredientDTOs;
        }

        public async Task<bool> UpdateIngredient(IngredientRequestDTO ingredientDTO)
        {
            Ingredient? ingredient = await _ingredientRepository.GetIngredientById(ingredientDTO.IngredientId);
            if (ingredient == null) return false;

            ingredient.IngredientName = ingredientDTO.IngredientName;
            ingredient.IngredientUnit = ingredientDTO.IngredientUnit;
            _ingredientRepository.UpdateIngredient(ingredient);
            _ingredientRepository.Save();
            return true;
        }
    }
}
