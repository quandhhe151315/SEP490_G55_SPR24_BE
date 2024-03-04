using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class MenuManager : IMenuManager
    {
        IMenuRepository _menuRepository;
        private IMapper _mapper;

        public MenuManager(IMenuRepository menuRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _mapper = mapper;
        }

        public void AddRecipeToMenu(int menuId, int recipeId)
        {
            _menuRepository.AddRecipeToMenu(menuId, recipeId);
            _menuRepository.Save();
        }

        public void CreateMenu(MenuRequestDTO menuRequestDTO)
        {
            _menuRepository.CreateMenu(_mapper.Map<MenuRequestDTO, Menu>(menuRequestDTO));
            _menuRepository.Save();
        }

        public void DeleteMenu(int menuId)
        {
            Menu? menu = _menuRepository.GetMenuById(menuId);
            if (menu != null)
            {
                _menuRepository.DeleteMenu(menu);
                _menuRepository.Save();
            }
        }

        public List<MenuDTO> GetAllMenues()
        {
            List<Menu> menus = _menuRepository.GetAllMenus();
            List<MenuDTO> menuDTOs = [];
            foreach (Menu menu in menus)
            {
                menuDTOs.Add(_mapper.Map<Menu, MenuDTO>(menu));
            }
            return menuDTOs;
        }

        public MenuDTO GetMenuById(int menuId)
        {
            Menu? menu = _menuRepository.GetMenuById(menuId);
            return menu is null ? null : _mapper.Map<Menu, MenuDTO>(menu);
        }

        public List<MenuDTO> GetMenuByUserId(int userId)
        {
            List<Menu> menus = _menuRepository.GetMenuByUserId(userId);
            List<MenuDTO> menuDTOs = [];
            foreach (Menu menu in menus)
            {
                menuDTOs.Add(_mapper.Map<Menu, MenuDTO>(menu));
            }
            return menuDTOs;
        }

        public void RemoveRecipeFromMenu(int menuId, int recipeId)
        {
            _menuRepository.RemoveRecipeFromMenu(menuId, recipeId);
            _menuRepository.Save();
        }

        public void UpdateMenu(MenuRequestDTO menuRequestDTO)
        {
            Menu? menu = _menuRepository.GetMenuById(menuRequestDTO.MenuId);
            if (menu != null)
            {
                //Update partially
                menu.MenuId = menuRequestDTO.MenuId;
                menu.FeaturedImage = menuRequestDTO.FeaturedImage;
                menu.MenuName = menuRequestDTO.MenuName;
                menu.MenuDescription = menuRequestDTO.MenuDescription;

                _menuRepository.UpdateMenu(menu);
                _menuRepository.Save();
            }
        }
    }
}
