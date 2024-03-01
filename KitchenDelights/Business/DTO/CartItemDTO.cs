using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class CartItemDTO
    {
        public int UserId { get; set; }

        public int RecipeId { get; set; }

        public string? RecipeTitle { get; set; }

        public decimal? RecipePrice { get; set; }

        public string? VoucherCode { get; set; }

        public byte? DiscountPercentage { get; set; }
    }
}
