using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Voucher
{
    public string VoucherCode { get; set; } = null!;

    public int UserId { get; set; }

    public byte DiscountPercentage { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User User { get; set; } = null!;
}
