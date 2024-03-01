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
    public class HistoryRepository : IHistoryRepository
    {
        private readonly KitchenDelightsContext _context;

        public HistoryRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public async Task<List<PaymentHistory>> GetPaymentHistory(int id)
        {
            return await _context.PaymentHistories.AsNoTracking().Include(x => x.User).Include(x => x.Recipe).Where(x => x.UserId == id).ToListAsync();
        }

        public void CreatePaymentHistory(List<PaymentHistory> paymentHistory)
        {
            try
            {
                _context.PaymentHistories.AddRange(paymentHistory);
            } catch (Exception ex)
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
