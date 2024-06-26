﻿using AutoMapper;
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
                if(ingMarketplace.Marketplace.MarketplaceStatus == 1) {
                    dtos.Add(_mapper.Map<IngredientMarketplace, IngredientMarketplaceDTO>(ingMarketplace));
                }
            }
            return dtos;
        }

        public async Task<List<IngredientMarketplaceDTO>> GetIngredientMarketplaces(int id)
        {
            List<IngredientMarketplaceDTO> dtos = [];
            List<IngredientMarketplace> list = await _repostitory.GetIngredientMarketplaces();
            foreach(IngredientMarketplace ingMarketplace in list)
            {
                if(ingMarketplace.MarketplaceId == id && ingMarketplace.Marketplace.MarketplaceStatus == 1) {
                    dtos.Add(_mapper.Map<IngredientMarketplace, IngredientMarketplaceDTO>(ingMarketplace));
                }
            }
            return dtos;
        }

        public async Task<bool> CreateIngredientMarketplace(IngredientMarketplaceDTO dto)
        {
            try
            {
                _repostitory.CreateIngredientMarketplace(_mapper.Map<IngredientMarketplaceDTO, IngredientMarketplace>(dto));
                _repostitory.Save();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
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
