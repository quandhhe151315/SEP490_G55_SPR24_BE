using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IIngredientRepository
    {
        void CreateIngredient(Ingredient ingredient);
        void UpdateIngredient(Ingredient ingredient);
        void DeleteIngredient(Ingredient ingredient);
        Task<List<Ingredient>> GetAllIngredients();
        Task<List<Ingredient>> GetIngredientByName(string name);
        Task<Ingredient?> GetIngredientById(int ingredientId);
        void Save();
    }
}
