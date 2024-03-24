using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Data.Entity;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> GetAllMenu()
        {
            List<MenuDTO> menus = [];
            try
            {
                menus = await _menuManager.GetAllMenues();
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
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> GetMenuByUserId(int userId)
        {
            List<MenuDTO> menus = [];
            try
            {
                menus = await _menuManager.GetMenuByUserId(userId);
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
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> GetMenuByUserIdAndCheckExistRecipe(int userId, int recipeId)
        {
            List<MenuDTO> menus = [];
            try
            {
                menus = await _menuManager.GetMenuByUserIdAndCheckExistRecipe(userId, recipeId);
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
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> GetMenuById(int menuId)
        {
            MenuDTO menu;
            try
            {
                menu = await _menuManager.GetMenuById(menuId);
                if (menu == null)
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
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> CreateMenu(MenuRequestDTO menu)
        {
            try
            {
                await _menuManager.CreateMenu(menu);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Create Sucessful");
        }

        [HttpPut]
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> UpdateMenu(MenuRequestDTO menu)
        {
            MenuDTO? menuDTO = await _menuManager.GetMenuById(menu.MenuId.Value);
            if (menuDTO == null) return NotFound("Menu not exist");

            bool isUpdated = await _menuManager.UpdateMenu(menu);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Update failed!") : Ok("Update sucessful");
        }

        [HttpPut]
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> AddRecipeToMenu(int menuId, int recipeId)
        {
            MenuDTO? menuDTO = await _menuManager.GetMenuById(menuId);
            if (menuDTO == null) return NotFound("Menu not exist");

            bool isUpdated = await _menuManager.AddRecipeToMenu(menuId, recipeId);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Add recipe failed!") : Ok("Add recipe sucessful");
        }

        [HttpPut]
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> RemoveRecipeFromMenu(int menuId, int recipeId)
        {
            MenuDTO? menuDTO = await _menuManager.GetMenuById(menuId);
            if (menuDTO == null) return NotFound("Menu not exist");

            bool isUpdated = await _menuManager.RemoveRecipeFromMenu(menuId, recipeId);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Remove recipe failed!") : Ok("Remove recipe sucessful");
        }

        [HttpDelete]
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> DeleteMenu(int menuId)
        {
            MenuDTO? menuDTO = await _menuManager.GetMenuById(menuId);
            if (menuDTO == null) return NotFound("Menu not exist");

            bool isDeleted = await _menuManager.DeleteMenu(menuId);
            return !isDeleted ? StatusCode(StatusCodes.Status500InternalServerError, "Delete failed!") : Ok("Delete sucessful");
        }
    }
}
