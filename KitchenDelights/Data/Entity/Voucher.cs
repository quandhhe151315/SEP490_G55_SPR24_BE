using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Voucher
{
    public string VoucherCode { get; set; } = null!;

    public int AccountId { get; set; }

    public byte DiscountPercentage { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
}
