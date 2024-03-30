using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IMarketplaceRepository
    {
        Task<Marketplace?> GetMarketplace(int id);
        Task<List<Marketplace>> GetMarketplaces();
        void CreateMarketplace(Marketplace marketplace);
        void UpdateMarketplace(Marketplace marketplace);
        void Save();
    }
}
