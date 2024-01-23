using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class CartItem
{
    public int AccountId { get; set; }

    public int RecipeId { get; set; }

    public string? VoucherCode { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual Voucher? VoucherCodeNavigation { get; set; }
}
