using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Data.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMenuManager _menuManager;

        public MenuController(IConfiguration configuration, IMenuManager menuManager)
        {
            _configuration = configuration;
            _menuManager = menuManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMenu()
        {
            List<MenuDTO> menus = [];
            try
            {
                menus = _menuManager.GetAllMenues();
                if (menus.Count <= 0)
                {
                    return NotFound("There are not exist any menu in database");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(menus);
        }

        [HttpGet]
        public async Task<IActionResult> GetMenuByUserId(int userId)
        {
            List<MenuDTO> menus = [];
            try
            {
                menus = _menuManager.GetMenuByUserId(userId);
                if (menus.Count <= 0)
                {
                    return NotFound("There are not exist any menu in database");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(menus);
        }

        [HttpGet]
        public async Task<IActionResult> GetMenuById(int menuId)
        {
            MenuDTO menu;
            try
            {
                menu = _menuManager.GetMenuById(menuId);
                if (menu.MenuId == 0)
                {
                    return NotFound("Menu not exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(menu);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMenu(MenuRequestDTO menu)
        {
            if (menu.UserId == null)
            {
                return StatusCode(StatusCodes.Status406NotAcceptable, "Please enter all require input");
            }
            else
            {
                try
                {
                    _menuManager.CreateMenu(menu);
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return Ok(menu);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateMenu(MenuRequestDTO menu)
        {
            try
            {
                if (_menuManager.GetMenuById(menu.MenuId) == null || _menuManager.GetMenuById(menu.MenuId).MenuId == 0)
                {
                    return NotFound("Menu not exist");
                }
                _menuManager.UpdateMenu(menu);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Update sucessfully");
        }

        [HttpPut]
        public async Task<IActionResult> AddRecipeToMenu(int menuId, int recipeId)
        {
            try
            {
                _menuManager.AddRecipeToMenu(menuId, recipeId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Add recipe to menu sucessful");
        }

        [HttpPut]
        public async Task<IActionResult> RemoveRecipeFromMenu(int menuId, int recipeId)
        {
            try
            {
                _menuManager.RemoveRecipeFromMenu(menuId, recipeId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Remove recipe from menu sucessful");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteMenu(int menuId)
        {
            try
            {
                if (_menuManager.GetMenuById(menuId) == null || _menuManager.GetMenuById(menuId).MenuId == 0)
                {
                    return NotFound("Menu not exist");
                }
                _menuManager.DeleteMenu(menuId);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Delete sucessfully");
        }
    }
}
