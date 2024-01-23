using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class RestaurantRecommendation
{
    public int RestaurantId { get; set; }

    public int AccountId { get; set; }

    public string? RestaurantName { get; set; }

    public string? RestaurantDetails { get; set; }

    public string? RestaurantLocation { get; set; }

    public virtual Account Account { get; set; } = null!;
}
