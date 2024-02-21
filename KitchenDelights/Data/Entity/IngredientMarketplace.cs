using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class IngredientMarketplace
{
    public int IngredientId { get; set; }

    public int MarketplaceId { get; set; }

    public string? MarketplaceLink { get; set; }

    public virtual Ingredient Ingredient { get; set; } = null!;

    public virtual Marketplace Marketplace { get; set; } = null!;
}
