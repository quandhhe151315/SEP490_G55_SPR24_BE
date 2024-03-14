using Business.DTO;
using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IVoucherManager
    {
        Task<VoucherDTO?> GetVoucher(string voucherCode);

        Task<List<VoucherDTO>> GetVouchers(int userId);

        Task<bool> CreateVoucher(VoucherDTO voucher);

        Task<bool> UpdateVoucher(VoucherDTO VoucherDTO);

        Task<bool> RemoveVoucher(string voucherCode);
    }
}
