using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class HistoryManager : IHistoryManager
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;

        public HistoryManager(IHistoryRepository historyRepository, IVoucherRepository voucherRepository, ICartRepository cartRepository, IMapper mapper)
        {
            _historyRepository = historyRepository;
            _voucherRepository = voucherRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
        }

        public async Task<List<PaymentHistoryDTO>> GetPaymentHistory()
        {
            List<PaymentHistoryDTO> historyDTO = [];
            List<PaymentHistory> history = await _historyRepository.GetPaymentHistory();
            foreach(PaymentHistory historyItem in history)
            {
                historyDTO.Add(_mapper.Map<PaymentHistory, PaymentHistoryDTO>(historyItem));
            }
            return historyDTO;
        }

        public async Task<List<PaymentHistoryDTO>> GetPaymentHistory(int id)
        {
            List<PaymentHistoryDTO> historyDTO = [];
            List<PaymentHistory> history = await _historyRepository.GetPaymentHistory(id);
            foreach(PaymentHistory historyItem in history)
            {
                historyDTO.Add(_mapper.Map<PaymentHistory, PaymentHistoryDTO>(historyItem));
            }
            return historyDTO;
        }

        public async Task<bool> CreatePaymentHistory(List<CartItemDTO> cart)
        {
            List<PaymentHistoryDTO> historyDTO = [];
            string? usedVoucher = cart[0].VoucherCode;
            
            //Remove all checkout cart item from cart
            //Then add to temp list of payment history
            foreach(CartItemDTO cartItem in cart)
            {
                _cartRepository.DeleteCartItem(_mapper.Map<CartItemDTO, CartItem>(cartItem));
                historyDTO.Add(_mapper.Map<CartItemDTO, PaymentHistoryDTO>(cartItem));
            }

            //In case user used voucher
            if (usedVoucher != null)
            {
                Voucher? used = await _voucherRepository.GetVoucher(usedVoucher);
                if (used == null) return false;

                //Apply voucher to purchased price
                foreach (PaymentHistoryDTO payment in historyDTO)
                {
                    payment.ActualPrice *= 1.0m - (used.DiscountPercentage / 100.0m);
                }

                //Remove used voucher
                _voucherRepository.RemoveVoucher(used);
            }

            List<PaymentHistory> history = [];
            foreach (PaymentHistoryDTO payment in historyDTO)
            {
                payment.PurchaseDate = DateTime.Now;
                history.Add(_mapper.Map<PaymentHistoryDTO, PaymentHistory>(payment));
            }
            _historyRepository.CreatePaymentHistory(history);
            _historyRepository.Save();
            return true;
        }

        public async Task<Revenue> GetNumberRevenueInThisMonth()
        {
            List<PaymentHistory> paymentHistoriesNow = await _historyRepository.GetPaymentHistory();
            DateTime now = DateTime.Now;
            paymentHistoriesNow = paymentHistoriesNow.Where(x => x.PurchaseDate.Month == now.Month).ToList();

            decimal revenueNow = 0;
            foreach (PaymentHistory paymentHistory in paymentHistoriesNow)
            {
                revenueNow = revenueNow + paymentHistory.ActualPrice;
            }

            List<PaymentHistory> paymentHistoriesLast = await _historyRepository.GetPaymentHistory();
            paymentHistoriesLast = paymentHistoriesLast.Where(x => x.PurchaseDate.Month == now.AddMonths(-1).Month).ToList();

            decimal revenueLast = 0;
            foreach (PaymentHistory paymentHistory in paymentHistoriesLast)
            {
                revenueLast = revenueLast + paymentHistory.ActualPrice;
            }

            Revenue revenue = new Revenue();
            revenue.revenue = revenueNow;
            if (revenueNow == 0 || revenueLast == 0)
            {
                revenue.percent = "0.0";
                revenue.increase = true;
            }
            else
            {
                if (revenueNow > revenueLast)
                {
                    revenue.percent = ((float)((revenueNow - revenueLast) / revenueLast * 100)).ToString("0.0"); ;
                    revenue.increase = true;
                }
                else
                {
                    revenue.percent = ((float)((revenueLast - revenueNow) / revenueNow * 100)).ToString("0.0"); ;
                    revenue.increase = false;
                }
            }
            return revenue;
        }
    }
}
