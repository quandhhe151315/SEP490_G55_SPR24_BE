using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class MenuManager : IMenuManager
    {
        private readonly IMenuRepository _menuRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public MenuManager(IMenuRepository menuRepository, IRecipeRepository recipeRepository, IMapper mapper)
        {
            _menuRepository = menuRepository;
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<bool> AddRecipeToMenu(int menuId, int recipeId)
        {
            try
            {
                Menu? menu = await _menuRepository.GetMenuById(menuId);
                Recipe? recipe = await _recipeRepository.GetRecipe(recipeId);

                if (recipe == null || menu == null)
                {
                    throw new Exception();
                }
                menu.Recipes.Add(recipe);
                _menuRepository.UpdateMenu(menu);
                _menuRepository.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task CreateMenu(MenuRequestDTO menuRequestDTO)
        {
            _menuRepository.CreateMenu(_mapper.Map<MenuRequestDTO, Menu>(menuRequestDTO));
            _menuRepository.Save();
        }

        public async Task<bool> DeleteMenu(int menuId)
        {
            Menu? menu = await _menuRepository.GetMenuById(menuId);
            if (menu == null) return false;
            menu.Recipes.Clear();
            _menuRepository.DeleteMenu(menu);
            _menuRepository.Save();
            return true;
        }

        public async Task<List<MenuDTO>> GetAllMenues()
        {
            List<Menu> menus = await _menuRepository.GetAllMenus();
            List<MenuDTO> menuDTOs = [];
            foreach (Menu menu in menus)
            {
                menuDTOs.Add(_mapper.Map<Menu, MenuDTO>(menu));
            }
            return menuDTOs;
        }

        public async Task<MenuDTO?> GetMenuById(int menuId)
        {
            Menu? menu = await _menuRepository.GetMenuById(menuId);
            return menu is null ? null : _mapper.Map<Menu, MenuDTO>(menu);
        }

        public async Task<List<MenuDTO>> GetMenuByUserId(int userId)
        {
            List<Menu> menus = await _menuRepository.GetMenuByUserId(userId);
            List<MenuDTO> menuDTOs = [];
            foreach (Menu menu in menus)
            {
                menuDTOs.Add(_mapper.Map<Menu, MenuDTO>(menu));
            }
            return menuDTOs;
        }

        public async Task<bool> RemoveRecipeFromMenu(int menuId, int recipeId)
        {
            try
            {
                Menu? menu = await _menuRepository.GetMenuById(menuId);
                Recipe? recipe = await _recipeRepository.GetRecipe(recipeId);

                if (recipe == null || menu == null)
                {
                    throw new Exception();
                }
                menu.Recipes.Remove(recipe);
                _menuRepository.UpdateMenu(menu);
                _menuRepository.Save();
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> UpdateMenu(MenuRequestDTO menuRequestDTO)
        {
            Menu? menu = await _menuRepository.GetMenuById(menuRequestDTO.MenuId.Value);
            if (menu == null) return false;
            //Update partially
            menu.MenuId = menuRequestDTO.MenuId.Value;
            menu.FeaturedImage = menuRequestDTO.FeaturedImage;
            menu.MenuName = menuRequestDTO.MenuName;
            menu.MenuDescription = menuRequestDTO.MenuDescription;

            _menuRepository.UpdateMenu(menu);
            _menuRepository.Save();
            return true;
        }
    }
}
