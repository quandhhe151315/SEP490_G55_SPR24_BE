using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IMenuRepository
    {
        void CreateMenu(Menu menu);
        void UpdateMenu(Menu menu);
        void DeleteMenu(Menu menu);
        List<Menu> GetAllMenus();
        Menu GetMenuById(int menuId);
        List<Menu> GetMenuByUserId(int userId);
        void AddRecipeToMenu(int menuId, int recipeId);
        void RemoveRecipeFromMenu(int menuId, int recipeId);
        void Save();
    }
}
