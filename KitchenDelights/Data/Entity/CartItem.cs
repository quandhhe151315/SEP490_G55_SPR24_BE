using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class CartItem
{
    public int UserId { get; set; }

    public int RecipeId { get; set; }

    public string? VoucherCode { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public virtual Voucher? VoucherCodeNavigation { get; set; }
}
