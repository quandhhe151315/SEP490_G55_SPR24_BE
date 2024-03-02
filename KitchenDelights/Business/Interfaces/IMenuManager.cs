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
        void CreateMenu(MenuDTO menuDTO);
        void UpdateMenu(MenuRequestDTO menuRequestDTO);
        void DeleteMenu(int menuId);
        List<MenuDTO> GetAllMenues();
        MenuDTO GetMenuById(int menuId);
    }
}
