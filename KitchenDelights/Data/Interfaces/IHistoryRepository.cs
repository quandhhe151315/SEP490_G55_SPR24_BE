using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IHistoryRepository
    {
        Task<List<PaymentHistory>> GetPaymentHistory(int id);
        void CreatePaymentHistory(List<PaymentHistory> paymentHistory);
        void Save();
    }
}
