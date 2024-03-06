using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class IngredientManager : IIngredientManager
    {
        IIngredientRepository _ingredientRepository;
        private IMapper _mapper;

        public IngredientManager(IIngredientRepository ingredientRepository, IMapper mapper)
        {
            _ingredientRepository = ingredientRepository;
            _mapper = mapper;
        }

        public List<IngredientDTO> GetAllIngredients()
        {
            List<Ingredient> ingredients = _ingredientRepository.GetAllIngredients();
            List<IngredientDTO> ingredientDTOs = [];
            foreach (Ingredient ingredient in ingredients)
            {
                ingredientDTOs.Add(_mapper.Map<Ingredient, IngredientDTO>(ingredient));
            }
            return ingredientDTOs;
        }

        public IngredientDTO GetIngredientById(int ingredientId)
        {
            IngredientDTO ingredientDTO = new IngredientDTO();
            Ingredient ingredient = _ingredientRepository.GetIngredientById(ingredientId);
            ingredientDTO = _mapper.Map<Ingredient, IngredientDTO>(ingredient);
            return ingredientDTO;
        }
    }
}
