using Business.DTO;
using Business.Interfaces;
using Business.Managers;
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
        public async Task<IActionResult> GetVoucherByCode(string voucherCode)
        {
            VoucherDTO voucher;
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
        public async Task<IActionResult> CreateVoucher(VoucherDTO voucher)
        {
            try
            {
                _voucherManager.CreateVoucher(voucher);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok(voucher);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateVoucher(VoucherDTO voucher)
        {
            try
            {
                if (await _voucherManager.GetVoucher(voucher.VoucherCode) == null)
                {
                    return NotFound("Voucher not exist");
                }
                await _voucherManager.UpdateVoucher(voucher);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Update sucessfully");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteVoucher(string voucherCode)
        {
            try
            {
                if (await _voucherManager.GetVoucher(voucherCode) == null)
                {
                    return NotFound("Voucher not exist");
                }
                await _voucherManager.RemoveVoucher(voucherCode);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok("Delete sucessfully");
        }
    }
}
