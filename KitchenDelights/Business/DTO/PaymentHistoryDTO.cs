using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class PaymentHistoryDTO
    {
        public int UserId { get; set; }

        public string? Username { get; set; }

        public int RecipeId { get; set; }

        public string? RecipeTitle { get; set; }

        public decimal ActualPrice { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}
