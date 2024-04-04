using Business.DTO;
using Business.Interfaces;
using Business.Managers;
using Data.Entity;
using KitchenDelights.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KitchenDelights.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VoucherController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IVoucherManager _voucherManager;

        public VoucherController(IConfiguration configuration, IVoucherManager voucherManager)
        {
            _configuration = configuration;
            _voucherManager = voucherManager;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> GetAllVouchers(int userId)
        {
            List<VoucherDTO> vouchers = [];
            try
            {
                vouchers = await _voucherManager.GetVouchers(userId);
                if (vouchers.Count <= 0)
                {
                    return NotFound("There are not exist any voucher in database");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(vouchers);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator,Moderator,Writer,Chef,users")]
        public async Task<IActionResult> GetVoucherByCode(string voucherCode)
        {
            VoucherDTO? voucher;
            try
            {
                voucher = await _voucherManager.GetVoucher(voucherCode);
                if (voucher == null)
                {
                    return NotFound("Voucher not exist");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(voucher);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateVoucher(int id)
        {
            bool flag = true;

            Random randomizer = new();

            VoucherDTO voucher = new()
            {
                UserId = id,
                DiscountPercentage = Convert.ToByte(randomizer.Next(5, 20))
            };

            //Run until successfully create new voucher
            while(flag)
            {
                voucher.VoucherCode = StringHelper.GenerateRandomString(10);
                flag = !await _voucherManager.CreateVoucher(voucher);
            }

            return Ok();
        }

        [HttpPut]
        [Authorize(Roles = "Administrator,Moderator")]
        public async Task<IActionResult> UpdateVoucher(VoucherDTO voucher)
        {
            VoucherDTO? voucherDTO = await _voucherManager.GetVoucher(voucher.VoucherCode);
            if (voucherDTO == null) return NotFound("Voucher not exist");

            bool isUpdated = await _voucherManager.UpdateVoucher(voucher);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Update failed!") : Ok("Update sucessful");
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeleteVoucher(string voucherCode)
        {
            VoucherDTO? voucherDTO = await _voucherManager.GetVoucher(voucherCode);
            if (voucherDTO == null) return NotFound("Voucher not exist");

            bool isUpdated = await _voucherManager.RemoveVoucher(voucherCode);
            return !isUpdated ? StatusCode(StatusCodes.Status500InternalServerError, "Delete failed!") : Ok("Delete sucessful");
        }
    }
}
