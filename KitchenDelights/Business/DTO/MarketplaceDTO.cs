using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class MarketplaceDTO
    {
        public int? MarketplaceId { get; set; }

        public string? MarketplaceName { get; set; }

        public string? MarketplaceLogo { get; set; }

        public int MarketplaceStatus { get; set; }
    }
}
