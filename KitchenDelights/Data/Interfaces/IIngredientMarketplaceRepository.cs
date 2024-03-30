using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IIngredientMarketplaceRepository
    {
        Task<IngredientMarketplace?> GetIngredientMarketplace(int ingredientId, int marketplaceId);
        Task<List<IngredientMarketplace>> GetIngredientMarketplaces();
        void CreateIngredientMarketplace(IngredientMarketplace ingmarketplace);
        void UpdateIngredientMarketplace(IngredientMarketplace ingmarketplace);
        void DeleteIngredientMarketplace(IngredientMarketplace ingmarketplace);
        void Save();
    }
}
