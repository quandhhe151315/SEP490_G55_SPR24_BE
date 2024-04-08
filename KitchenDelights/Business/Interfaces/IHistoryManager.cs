using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IHistoryManager
    {
        Task<List<PaymentHistoryDTO>> GetPaymentHistory();
        Task<List<PaymentHistoryDTO>> GetPaymentHistory(int id);
        Task<Revenue> GetNumberRevenueInThisMonth();
        Task<List<RevenueInNumberMonth>> GetNumberRevenueInNumberOfMonth(int numMonth);
        Task<int> GetNumberOfRecipesAreBoughtInThisMonth();
        Task<bool> CreatePaymentHistory(List<CartItemDTO> cart);
    }
}
