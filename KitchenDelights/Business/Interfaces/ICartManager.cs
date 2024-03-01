using AutoMapper;
using Business.DTO;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICartManager
    {
        Task<List<CartItemDTO>> GetCart(int id);
        Task<bool> CreateCartItem(CartItemDTO cartItemDTO);
        Task<bool> UpdateCartItem(int id, string voucherCode);
        Task<bool> DeleteCartItem(CartItemDTO cartItemDTO);
    }
}
