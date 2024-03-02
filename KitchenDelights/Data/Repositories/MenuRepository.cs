using Data.Entity;
using Data.Interfaces;
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

        public List<Menu> GetAllMenus()
        {
            List<Menu> menus = new List<Menu>();
            try
            {
                menus = _context.Menus.ToList();
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
                menu = _context.Menus.FirstOrDefault(x => x.MenuId == menuId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
            return menu;
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
