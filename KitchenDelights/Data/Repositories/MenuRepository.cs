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

        public async Task<List<Menu>> GetAllMenus()
        {
            List<Menu> menus = new List<Menu>();
            try
            {
                menus = await _context.Menus.Include(x => x.Recipes).ThenInclude(x => x.User).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return menus;
        }

        public async Task<Menu?> GetMenuById(int menuId)
        {
            Menu? menu = new Menu();
            try
            {
                menu = await _context.Menus.Include(x => x.Recipes).ThenInclude(x => x.User).FirstOrDefaultAsync(x => x.MenuId == menuId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return menu;
        }

        public async Task<List<Menu>> GetMenuByUserId(int userId)
        {
            List<Menu> menus = new List<Menu>();
            try
            {
                menus = await _context.Menus.Include(x => x.Recipes).ThenInclude(x => x.User).Where(x => x.UserId == userId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return menus;
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
