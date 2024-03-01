using Data.Entity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly KitchenDelightsContext _context;

        public VoucherRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public async Task<Voucher?> GetVoucher(string voucherCode)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(x => x.VoucherCode.Equals(voucherCode));
        }

        public async Task<List<Voucher>> GetVouchers(int id)
        {
            return await _context.Vouchers.Where(x => x.UserId == id).ToListAsync();
        }

        public void CreateVoucher(Voucher voucher)
        {
            try
            {
                _context.Vouchers.Add(voucher);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void Remove(Voucher voucher)
        {
            try
            {
                _context.Vouchers.Remove(voucher);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
