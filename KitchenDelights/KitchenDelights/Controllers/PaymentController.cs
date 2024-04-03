using Business.DTO;
using Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> History(int? id)
        {
            List<PaymentHistoryDTO> history;
            if(id == null && (User.IsInRole("Administrator") || User.IsInRole("Moderator")))
            {
                history = await _historyManager.GetPaymentHistory();
            } else {
                if(id != null && id < 0) return BadRequest("Invalid Id");
                history = await _historyManager.GetPaymentHistory(id.Value);
            }
            return Ok(history);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> GetNumberRevenueInThisMonth()
        {
            Revenue revenue = await _historyManager.GetNumberRevenueInThisMonth();
            return Ok(revenue);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> GetNumberRevenueInNumberMonth(int numMonth)
        {
            List<RevenueInNumberMonth> revenues = await _historyManager.GetNumberRevenueInNumberOfMonth(numMonth);
            return Ok(revenues);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Checkout(List<CartItemDTO> cart)
        {
            bool checkedOut = await _historyManager.CreatePaymentHistory(cart);
            return checkedOut ? Ok() : StatusCode(500, "Failed to checkout!");
        }
    }
}
