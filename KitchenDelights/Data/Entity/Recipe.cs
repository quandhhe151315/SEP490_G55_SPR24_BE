using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public int AccountId { get; set; }

    public int CategoryId { get; set; }

    public string? RecipeTitle { get; set; }

    public string? RecipeContent { get; set; }

    public int RecipeRating { get; set; }

    public bool RecipeStatus { get; set; }

    public bool IsFree { get; set; }

    public decimal? RecipePrice { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();

    public virtual ICollection<RecipeRating> RecipeRatings { get; set; } = new List<RecipeRating>();

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
