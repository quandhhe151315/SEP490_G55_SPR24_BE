using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Marketplace
{
    public int MarketplaceId { get; set; }

    public string? MarketplaceName { get; set; }

    public string? MarketplaceLogo { get; set; }

    public int MarketplaceStatus { get; set; }

    public virtual ICollection<IngredientMarketplace> IngredientMarketplaces { get; set; } = new List<IngredientMarketplace>();
}
