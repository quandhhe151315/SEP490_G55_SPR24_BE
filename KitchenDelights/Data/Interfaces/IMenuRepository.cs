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
        Task<List<Menu>> GetAllMenus();
        Task<Menu?> GetMenuById(int menuId);
        Task<List<Menu>> GetMenuByUserId(int userId);
        void Save();
    }
}
