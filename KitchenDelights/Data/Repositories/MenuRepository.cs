using Data.Entity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class MenuRepository : IMenuRepository
    {
        private readonly KitchenDelightsContext _context;

        public MenuRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public void AddRecipeToMenu(int menuId, int recipeId)
        {
            try
            {
                Menu? menu = _context.Menus
                    .Include(x => x.Recipes)
                    .FirstOrDefault(x => x.MenuId == menuId);
                Recipe? recipe = _context.Recipes
                    .FirstOrDefault(x => x.RecipeId == recipeId);
                if (recipe == null)
                {
                    throw new Exception();
                }
                menu.Recipes.Add(recipe);
                _context.Menus.Update(menu);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void CreateMenu(Menu menu)
        {
            try
            {
                _context.Menus.Add(menu);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteMenu(Menu menu)
        {
            try
            {
                if (menu != null)
                {
                    _context.Menus.Remove(menu);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public List<Menu> GetAllMenus()
        {
            List<Menu> menus = new List<Menu>();
            try
            {
                menus = _context.Menus.Include(x => x.Recipes).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return menus;
        }

        public Menu GetMenuById(int menuId)
        {
            Menu? menu = new Menu();
            try
            {
                menu = _context.Menus.Include(x => x.Recipes).FirstOrDefault(x => x.MenuId == menuId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return menu;
        }

        public List<Menu> GetMenuByUserId(int userId)
        {
            List<Menu> menus = new List<Menu>();
            try
            {
                menus = _context.Menus.Include(x => x.Recipes).Where(x => x.UserId == userId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return menus;
        }

        public void RemoveRecipeFromMenu(int menuId, int recipeId)
        {
            try
            {
                Menu? menu = _context.Menus
                    .Include(x => x.Recipes)
                    .FirstOrDefault(x => x.MenuId == menuId);
                Recipe? recipe = _context.Recipes
                    .FirstOrDefault(x => x.RecipeId == recipeId);
                if (recipe == null)
                {
                    throw new Exception();
                }
                menu.Recipes.Remove(recipe);
                _context.Menus.Update(menu);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void UpdateMenu(Menu menu)
        {
            try
            {
                _context.Menus.Update(menu);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}
