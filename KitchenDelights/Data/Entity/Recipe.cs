using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Recipe
{
    public int RecipeId { get; set; }

    public int UserId { get; set; }

    public string? FeaturedImage { get; set; }

    public string? RecipeTitle { get; set; }

    public string? RecipeDescription { get; set; }

    public string? VideoLink { get; set; }

    public int PreparationTime { get; set; }

    public int CookTime { get; set; }

    public int? RecipeServe { get; set; }

    public string? RecipeContent { get; set; }

    public decimal RecipeRating { get; set; }

    public int RecipeStatus { get; set; }

    public bool IsFree { get; set; }

    public decimal? RecipePrice { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

    public virtual ICollection<RecipeRating> RecipeRatings { get; set; } = new List<RecipeRating>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Country> Countries { get; set; } = new List<Country>();

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
