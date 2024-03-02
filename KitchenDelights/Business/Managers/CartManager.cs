using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;

namespace Business.Managers
{
    public class CartManager : ICartManager
    {
        private readonly ICartRepository _cartRepository;
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public CartManager(ICartRepository cartRepository, IRecipeRepository recipeRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<List<CartItemDTO>> GetCart(int id)
        {
            List<CartItemDTO> cartDTO = [];
            List<CartItem> cart = await _cartRepository.GetCart(id);
            foreach(CartItem item in cart)
            {
                cartDTO.Add(_mapper.Map<CartItem, CartItemDTO>(item));
            }
            return cartDTO;
        }

        public async Task<bool> CreateCartItem(CartItemDTO cartItemDTO)
        {
            Recipe? recipe = await _recipeRepository.GetRecipe(cartItemDTO.RecipeId);
            if (recipe == null) return false;
            if (recipe.IsFree) return false;
            try
            {
                _cartRepository.CreateCartItem(_mapper.Map<CartItemDTO, CartItem>(cartItemDTO));
                _cartRepository.Save();
                return true;
            } catch (Exception ex) { 
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> UpdateCartItem(int id, string voucherCode)
        {
            List<CartItemDTO> cart = await GetCart(id);
            foreach (CartItemDTO item in cart)
            {
                item.VoucherCode = voucherCode;
                try
                {
                    _cartRepository.UpdateCartItem(_mapper.Map<CartItemDTO, CartItem>(item));
                    _cartRepository.Save();
                } catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> DeleteCartItem(CartItemDTO cartItemDTO)
        {
            CartItem? item = await _cartRepository.GetCartItem(cartItemDTO.UserId, cartItemDTO.RecipeId);
            if (item == null) return false;

            _cartRepository.DeleteCartItem(item);
            _cartRepository.Save();
            return true;
        }
    }
}
