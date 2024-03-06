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
        List<IngredientDTO> GetAllIngredients();
        List<IngredientDTO> GetIngredientsByName(string name);
        IngredientDTO GetIngredientById(int ingredientId);
    }
}
