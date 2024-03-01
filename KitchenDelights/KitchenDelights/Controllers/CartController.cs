using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartManager _cartManager;
        private readonly IConfiguration _configuration;

        public CartController(ICartManager cartManager, IConfiguration configuration)
        {
            _cartManager = cartManager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            List<CartItemDTO> cart = await _cartManager.GetCart(id);
            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CartItemDTO item)
        {
            bool isAdded = await _cartManager.CreateCartItem(item);
            return isAdded ? Ok() : BadRequest("Add to cart failed!");
        }

        [HttpPut]
        public async Task<IActionResult> Discount(int id, string discountCode)
        {
            bool isUpdate = await _cartManager.UpdateCartItem(id, discountCode);
            return isUpdate ? Ok() : StatusCode(500, "Failed to use voucher!");
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(CartItemDTO item)
        {
            bool isRemoved = await _cartManager.DeleteCartItem(item);
            return isRemoved ? Ok() : BadRequest("Remove from cart failed!");
        }
    }
}
