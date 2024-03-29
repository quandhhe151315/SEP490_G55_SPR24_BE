using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.DTO;

public class CartDTO
{
    public string UserName { get; set; }

    public List<CartItemDTO> Items { get; set; } = [];

    public int Count { get; set; } = 0;

    public decimal TotalPricePreVoucher = 0;

    public decimal TotalPricePostVoucher = 0;
}
