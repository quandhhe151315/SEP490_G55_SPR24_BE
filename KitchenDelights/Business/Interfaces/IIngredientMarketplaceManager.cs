using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IIngredientMarketplaceManager
    {
        Task<List<IngredientMarketplaceDTO>> GetIngredientMarketplaces();
        Task<List<IngredientMarketplaceDTO>> GetIngredientMarketplaces(int id);
        void CreateIngredientMarketplace(IngredientMarketplaceDTO dto);
        Task<bool> UpdateIngredientMarketplace(IngredientMarketplaceDTO dto);
        Task<bool> DeleteIngredientMarketplace(int ingredientId, int marketplaceId);
    }
}
