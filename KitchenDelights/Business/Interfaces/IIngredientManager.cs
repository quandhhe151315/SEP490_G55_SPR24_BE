using Business.DTO;
using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IIngredientManager
    {
        Task<bool> CreateIngredient(IngredientRequestDTO ingredient);

        Task<bool> UpdateIngredient(IngredientRequestDTO ingredientDTO);

        Task<bool> DeleteIngredient(int id);
        Task<List<IngredientDTO>> GetAllIngredients();
        Task<List<IngredientDTO>> GetIngredientsByName(string name);
        Task<IngredientDTO?> GetIngredientById(int ingredientId);
    }
}
