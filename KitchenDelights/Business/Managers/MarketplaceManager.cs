using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class MarketplaceManager : IMarketplaceManager
    {
        private readonly IMarketplaceRepository _marketplaceRepository;
        private readonly IMapper _mapper;

        public MarketplaceManager(IMarketplaceRepository marketplaceRepository, IMapper mapper)
        {
            _marketplaceRepository = marketplaceRepository;
            _mapper = mapper;
        }

        public async Task<List<MarketplaceDTO>> GetMarketplaces()
        {
            List<MarketplaceDTO> dtos = [];
            List<Marketplace> marketplaces = await _marketplaceRepository.GetMarketplaces();
            foreach (Marketplace marketplace in marketplaces)
            {
                dtos.Add(_mapper.Map<Marketplace, MarketplaceDTO>(marketplace));
            }
            return dtos;
        }

        public void CreateMarketplace(MarketplaceDTO marketplace)
        {
            _marketplaceRepository.CreateMarketplace(_mapper.Map<MarketplaceDTO, Marketplace>(marketplace));
            _marketplaceRepository.Save();

        }

        public async Task<bool> UpdateMarketplace(MarketplaceDTO marketplace)
        {
            Marketplace? toUpdate = await _marketplaceRepository.GetMarketplace(marketplace.MarketplaceId.Value);
            if (toUpdate == null) return false;
            toUpdate.MarketplaceName = marketplace.MarketplaceName;
            toUpdate.MarketplaceLogo = marketplace.MarketplaceLogo;

            _marketplaceRepository.UpdateMarketplace(toUpdate);
            _marketplaceRepository.Save();
            return true;
        }

        public async Task<bool> UpdateStatus(int id) {
            Marketplace? toUpdate = await _marketplaceRepository.GetMarketplace(id);
            if (toUpdate is null) return false;
            switch(toUpdate.MarketplaceStatus) {
                case 1:
                    toUpdate.MarketplaceStatus = 2;
                    break;
                case 2:
                    toUpdate.MarketplaceStatus = 1;
                    break;
                default:
                    return false;
            }
            _marketplaceRepository.UpdateMarketplace(toUpdate);
            _marketplaceRepository.Save();
            return true;
        }

        public async Task<bool> DeleteMarketplace(int id)
        {
            Marketplace? toDelete = await _marketplaceRepository.GetMarketplace(id);
            if (toDelete == null) return false;

            toDelete.MarketplaceStatus = 0;
            _marketplaceRepository.UpdateMarketplace(toDelete);
            _marketplaceRepository.Save();
            return true;
        }
    }
}
