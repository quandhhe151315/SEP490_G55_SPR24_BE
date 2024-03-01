using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class RestaurantRecommendation
{
    public int RestaurantId { get; set; }

    public int UserId { get; set; }

    public string? RestaurantName { get; set; }

    public string? RestaurantDetails { get; set; }

    public string? RestaurantLocation { get; set; }

    public string? FeaturedImage { get; set; }

    public virtual User User { get; set; } = null!;
}
