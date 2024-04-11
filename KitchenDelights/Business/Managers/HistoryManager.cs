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
            foreach (PaymentHistory historyItem in history)
            {
                historyDTO.Add(_mapper.Map<PaymentHistory, PaymentHistoryDTO>(historyItem));
            }
            return historyDTO;
        }

        public async Task<List<PaymentHistoryDTO>> GetPaymentHistory(int id)
        {
            List<PaymentHistoryDTO> historyDTO = [];
            List<PaymentHistory> history = await _historyRepository.GetPaymentHistory(id);
            foreach (PaymentHistory historyItem in history)
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
            foreach (CartItemDTO cartItem in cart)
            {
                CartItem? item = await _cartRepository.GetCartItem(cartItem.UserId, cartItem.RecipeId);
                if(item != null) {
                    _cartRepository.DeleteCartItem(item);
                }
                //_cartRepository.DeleteCartItem(_mapper.Map<CartItemDTO, CartItem>(cartItem));
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
                //_voucherRepository.RemoveVoucher(used);
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
            paymentHistoriesLast = paymentHistoriesLast.Where(x => x.PurchaseDate.Month == now.AddMonths(-1).Month && x.PurchaseDate.Year == now.Year).ToList();

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

        public async Task<List<RevenueInNumberMonth>> GetNumberRevenueInNumberOfMonth(int numMonth)
        {
            List<RevenueInNumberMonth> revenueInNumberMonths = [];
            List<PaymentHistory> Histories = await _historyRepository.GetPaymentHistory();
            DateTime now = DateTime.Now;
            if (Histories.Count() > 0)
            {
                for (int i = 0; i < numMonth; i++)
                {
                    decimal revenue = 0;
                    RevenueInNumberMonth revenueInNumberMonth = new RevenueInNumberMonth();
                    List<PaymentHistory> paymentHistories = Histories.Where(x => x.PurchaseDate.Month == now.AddMonths(0-i).Month && x.PurchaseDate.Year == now.Year).ToList();
                    foreach (PaymentHistory paymentHistory in paymentHistories)
                    {
                        revenue = revenue + paymentHistory.ActualPrice;
                    }
                    switch (now.AddMonths(0-i).Month)
                    {
                        case 1:
                            revenueInNumberMonth.month = "Tháng 1";
                            break;
                        case 2:
                            revenueInNumberMonth.month = "Tháng 2";
                            break;
                        case 3:
                            revenueInNumberMonth.month = "Tháng 3";
                            break;
                        case 4:
                            revenueInNumberMonth.month = "Tháng 4";
                            break;
                        case 5:
                            revenueInNumberMonth.month = "Tháng 5";
                            break;
                        case 6:
                            revenueInNumberMonth.month = "Tháng 6";
                            break;
                        case 7:
                            revenueInNumberMonth.month = "Tháng 7";
                            break;
                        case 8:
                            revenueInNumberMonth.month = "Tháng 8";
                            break;
                        case 9:
                            revenueInNumberMonth.month = "Tháng 9";
                            break;
                        case 10:
                            revenueInNumberMonth.month = "Tháng 10";
                            break;
                        case 11:
                            revenueInNumberMonth.month = "Tháng 11";
                            break;
                        case 12:
                            revenueInNumberMonth.month = "Tháng 12";
                            break;
                    }
                    revenueInNumberMonth.revenue = revenue.ToString("0.0");
                    revenueInNumberMonths.Add(revenueInNumberMonth);
                }
            }
            return revenueInNumberMonths;
        }

        public async Task<int> GetNumberOfRecipesAreBoughtInThisMonth()
        {
            List<PaymentHistory> histories = await _historyRepository.GetPaymentHistory();
            DateTime now = DateTime.Now;
            histories = histories.Where(x => x.PurchaseDate.Month == now.Month && x.PurchaseDate.Year == now.Year).ToList();
            return histories.Count();
        }
    }
}
