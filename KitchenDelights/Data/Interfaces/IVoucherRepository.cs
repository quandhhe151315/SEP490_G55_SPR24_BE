using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IVoucherRepository
    {
        Task<Voucher?> GetVoucher(string voucherCode);

        Task<List<Voucher>> GetVouchers(int id);

        void CreateVoucher(Voucher voucher);

        void Remove(Voucher voucher);

        void Save();
    }
}
