using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class PaymentHistory
{
    public int UserId { get; set; }

    public int RecipeId { get; set; }

    public decimal ActualPrice { get; set; }

    public DateTime PurchaseDate { get; set; }

    public virtual Recipe Recipe { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
