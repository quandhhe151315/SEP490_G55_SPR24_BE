using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PaymentController : Controller
    {
        private readonly IHistoryManager _historyManager;
        private readonly IConfiguration _configuration;

        public PaymentController(IHistoryManager historyManager, IConfiguration configuration)
        {
            _historyManager = historyManager;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> History(int id)
        {
            List<PaymentHistoryDTO> history = await _historyManager.GetPaymentHistory(id);
            return Ok(history);
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(List<CartItemDTO> cart)
        {
            bool checkedOut = await _historyManager.CreatePaymentHistory(cart);
            return checkedOut ? Ok() : StatusCode(500, "Failed to checkout!");
        }
    }
}
