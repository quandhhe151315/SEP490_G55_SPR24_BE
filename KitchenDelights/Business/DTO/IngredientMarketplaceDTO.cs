using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class IngredientMarketplaceDTO
    {
        public int? IngredientId { get; set; }
        public string? IngredientName { get; set; }
        public int? MarketplaceId { get; set; }
        public string? MarketplaceLogo { get; set; }
        public string? MarketplaceLink { get; set; }
    }
}
