using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class PaymentHistory
{
    public int AccountId { get; set; }

    public int RecipeId { get; set; }

    public decimal ActualPrice { get; set; }

    public DateTime PurchaseDate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
