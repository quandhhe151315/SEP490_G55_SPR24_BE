using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IMarketplaceManager
    {
        Task<List<MarketplaceDTO>> GetMarketplaces();
        void CreateMarketplace(MarketplaceDTO marketplace);
        Task<bool> UpdateMarketplace(MarketplaceDTO marketplace);
        Task<bool> DeleteMarketplace(int id);
    }
}
