using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICartRepository
    {
        Task<List<CartItem>> GetCart(int id);
        Task<CartItem?> GetCartItem(int userId, int recipeId);
        void CreateCartItem(CartItem item);
        void UpdateCartItem(CartItem item);
        void DeleteCartItem(CartItem item);
        void Save();
    }
}
