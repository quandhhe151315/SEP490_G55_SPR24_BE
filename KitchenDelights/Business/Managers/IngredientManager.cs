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

        public async Task<List<IngredientDTO>> GetAllIngredients()
        {
            List<Ingredient> ingredients = await _ingredientRepository.GetAllIngredients();
            List<IngredientDTO> ingredientDTOs = [];
            foreach (Ingredient ingredient in ingredients)
            {
                ingredientDTOs.Add(_mapper.Map<Ingredient, IngredientDTO>(ingredient));
            }
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
            return ingredientDTOs;
        }
    }
}
