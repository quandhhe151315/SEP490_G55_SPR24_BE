using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IMenuManager
    {
        Task<bool> CreateMenu(MenuRequestDTO menuRequestDTO);
        Task<bool> UpdateMenu(MenuRequestDTO menuRequestDTO);
        Task<bool> DeleteMenu(int menuId);
        Task<List<MenuDTO>> GetAllMenues();
        Task<MenuDTO> GetMenuById(int menuId);
        Task<List<MenuDTO>> GetMenuByUserId(int userId);
        Task<List<MenuDTO>> GetMenuByUserIdAndCheckExistRecipe(int userId, int recipeId);
        Task<bool> AddRecipeToMenu(int menuId, int recipeId);
        Task<bool> RemoveRecipeFromMenu(int menuId, int recipeId);
    }
}
