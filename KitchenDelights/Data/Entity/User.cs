using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class User
{
    public int UserId { get; set; }

    public string? Username { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Avatar { get; set; }

    public string? PasswordHash { get; set; }

    public string? ResetToken { get; set; }

    public DateTime? ResetExpire { get; set; }

    public int StatusId { get; set; }

    public int RoleId { get; set; }

    public int Interaction { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<BlogComment> BlogComments { get; set; } = new List<BlogComment>();

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual ICollection<PaymentHistory> PaymentHistories { get; set; } = new List<PaymentHistory>();

    public virtual ICollection<RecipeRating> RecipeRatings { get; set; } = new List<RecipeRating>();

    public virtual ICollection<Recipe> RecipesNavigation { get; set; } = new List<Recipe>();

    public virtual ICollection<RestaurantRecommendation> RestaurantRecommendations { get; set; } = new List<RestaurantRecommendation>();

    public virtual Role Role { get; set; } = null!;

    public virtual Status Status { get; set; } = null!;

    public virtual ICollection<Verification> Verifications { get; set; } = new List<Verification>();

    public virtual ICollection<Voucher> Vouchers { get; set; } = new List<Voucher>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
