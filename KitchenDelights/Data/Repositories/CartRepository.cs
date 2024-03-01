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
    public class CartRepository : ICartRepository
    {
        private readonly KitchenDelightsContext _context;

        public CartRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetCart(int id)
        {
            return await _context.CartItems.AsNoTracking().Include(x => x.Recipe).Include(x => x.VoucherCodeNavigation).Where(x => x.UserId == id).ToListAsync();
        }

        public async Task<CartItem?> GetCartItem(int userId, int recipeId)
        {
            return await _context.CartItems.AsNoTracking().Include(x => x.Recipe).Include(x => x.VoucherCodeNavigation).FirstOrDefaultAsync(x => x.UserId == userId && x.RecipeId == recipeId);
        }

        public void CreateCartItem(CartItem item)
        {
            try
            {
                _context.CartItems.Add(item);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateCartItem(CartItem item)
        {
            try
            {
                _context.CartItems.Update(item);
            } catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteCartItem(CartItem item)
        {
            try
            {
                _context.CartItems.Remove(item);
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
    }
}
