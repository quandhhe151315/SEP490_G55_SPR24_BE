using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IIngredientManager
    {
        Task<List<IngredientDTO>> GetAllIngredients();
        Task<List<IngredientDTO>> GetIngredientsByName(string name);
        Task<IngredientDTO?> GetIngredientById(int ingredientId);
    }
}
