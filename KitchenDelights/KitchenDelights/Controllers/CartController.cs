using Business.DTO;
using Business.Interfaces;
using KitchenDelights.Helper;
using KitchenDelights.Library;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartManager _cartManager;
        private readonly IUserManager _userManager;
        private readonly IConfiguration _configuration;

        public CartController(ICartManager cartManager, IUserManager userManager, IConfiguration configuration)
        {
            _cartManager = cartManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            if(id < 0) return BadRequest("Invalid Id!");
            CartDTO cart = new();
            List<CartItemDTO> cartItems = await _cartManager.GetCart(id);
            UserDTO? user = await _userManager.GetUser(id);
            if(user == null) return NotFound("User not available");
            cart.UserName = user.FirstName.IsNullOrEmpty() ? user.Username : user.FirstName;
            cart.Items = cartItems;
            cart.Count = cartItems.Count;
            if (cartItems.Count > 0)
            {
                foreach(CartItemDTO item in cartItems) {
                    cart.TotalPricePreVoucher += item.RecipePrice.Value;
                }
                if (!cartItems[0].VoucherCode.IsNullOrEmpty())
                {
                    cart.TotalPricePostVoucher += (cart.TotalPricePreVoucher * (1m - (cartItems[0].DiscountPercentage/ 100m))).Value; 
                } else {
                    cart.TotalPricePostVoucher += cart.TotalPricePreVoucher;
                }
            }
            else
            {
                cart.TotalPricePreVoucher = 0;
                cart.TotalPricePostVoucher = 0;
            }
            return Ok(cart);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> returnURLPayment(decimal amount)
        {
            //Get Config Info
            string vnp_Returnurl = "http://localhost:5000/ticket"; //URL nhan ket qua tra ve 
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = "3RXZE2T2"; //Ma website
            string vnp_HashSecret = "YIOYZTPHYQIRSGKVGXXWDTPJXHFXKLJC"; //Chuoi bi mat
            if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
            {
                StatusCode(500, "Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config");
            }
            string locale = "vn";

            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (amount * 100).ToString());
            vnpay.AddRequestData("vnp_BankCode", "VNBANK");
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", "192.168.137.23");

            if (!string.IsNullOrEmpty(locale))
            {
                vnpay.AddRequestData("vnp_Locale", locale);
            }
            else
            {
                vnpay.AddRequestData("vnp_Locale", "vn");
            }
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + DateTime.Now.Ticks);
            vnpay.AddRequestData("vnp_OrderType", "other"); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());

            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return Ok(paymentUrl);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CartItemDTO item)
        {
            if(item.RecipeId < 0) return BadRequest("Invalid recipe Id");
            if(item.UserId < 0) return BadRequest("Invalid user Id");
            bool isAdded = await _cartManager.CreateCartItem(item);
            return isAdded ? Ok() : BadRequest("Add to cart failed!");
        }

        [HttpPut]
        public async Task<IActionResult> Discount(int id, string discountCode)
        {
            if (id < 0) return BadRequest("Invalid Id");
            if (StringHelper.Process(discountCode).IsNullOrEmpty()) return BadRequest("Invalid Voucher");
            bool isUpdate = await _cartManager.UpdateCartItem(id, discountCode);
            return isUpdate ? Ok() : StatusCode(500, "Failed to use voucher!");
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(CartItemDTO item)
        {
            if(item.RecipeId < 0) return BadRequest("Invalid recipe Id");
            if(item.UserId < 0) return BadRequest("Invalid user Id");
            bool isRemoved = await _cartManager.DeleteCartItem(item);
            return isRemoved ? Ok() : BadRequest("Remove from cart failed!");
        }
    }
}
