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
    public class IngredientMarketplaceManager : IIngredientMarketplaceManager
    {
        private readonly IIngredientMarketplaceRepository _repostitory;
        private readonly IMapper _mapper;

        public IngredientMarketplaceManager(IIngredientMarketplaceRepository repostitory, IMapper mapper)
        {
            _repostitory = repostitory;
            _mapper = mapper;
        }

        public async Task<List<IngredientMarketplaceDTO>> GetIngredientMarketplaces()
        {
            List<IngredientMarketplaceDTO> dtos = [];
            List<IngredientMarketplace> list = await _repostitory.GetIngredientMarketplaces();
            foreach(IngredientMarketplace ingMarketplace in list)
            {
                dtos.Add(_mapper.Map<IngredientMarketplace, IngredientMarketplaceDTO>(ingMarketplace));
            }
            return dtos;
        }

        public void CreateIngredientMarketplace(IngredientMarketplaceDTO dto)
        {
            _repostitory.CreateIngredientMarketplace(_mapper.Map<IngredientMarketplaceDTO, IngredientMarketplace>(dto));
            _repostitory.Save();
        }

        public async Task<bool> UpdateIngredientMarketplace(IngredientMarketplaceDTO dto)
        {
            IngredientMarketplace? toUpdate = await _repostitory.GetIngredientMarketplace(dto.IngredientId.Value, dto.MarketplaceId.Value);
            if (toUpdate is null) return false;
            toUpdate.MarketplaceLink = dto.MarketplaceLink;
            _repostitory.UpdateIngredientMarketplace(toUpdate);
            _repostitory.Save();
            return true;
        }

        public async Task<bool> DeleteIngredientMarketplace(int ingredientId, int marketplaceId)
        {
            IngredientMarketplace? toDelete = await _repostitory.GetIngredientMarketplace(ingredientId, marketplaceId);
            if (toDelete is null) return false;
            _repostitory.DeleteIngredientMarketplace(toDelete);
            _repostitory.Save();
            return true;
        }
    }
}
