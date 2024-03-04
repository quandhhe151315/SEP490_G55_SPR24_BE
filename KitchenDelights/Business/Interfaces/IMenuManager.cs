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
        void CreateMenu(MenuRequestDTO menuRequestDTO);
        void UpdateMenu(MenuRequestDTO menuRequestDTO);
        void DeleteMenu(int menuId);
        List<MenuDTO> GetAllMenues();
        MenuDTO GetMenuById(int menuId);
        List<MenuDTO> GetMenuByUserId(int userId);
        void AddRecipeToMenu(int menuId, int recipeId);
        void RemoveRecipeFromMenu(int menuId, int recipeId);
    }
}
