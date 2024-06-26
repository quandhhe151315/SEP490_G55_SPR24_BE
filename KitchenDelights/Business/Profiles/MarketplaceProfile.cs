﻿using AutoMapper;
using Business.DTO;
using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Profiles
{
    public class MarketplaceProfile : Profile
    {
        public MarketplaceProfile() { 
            CreateMap<Marketplace, MarketplaceDTO>();
            CreateMap<MarketplaceDTO, Marketplace>();
            CreateMap<IngredientMarketplace, IngredientMarketplaceDTO>()
                .ForMember(dest => dest.IngredientName, opt => opt.MapFrom(src => src.Ingredient.IngredientName))
                .ForMember(dest => dest.MarketplaceLogo, opt => opt.MapFrom(src => src.Marketplace.MarketplaceLogo));
            CreateMap<IngredientMarketplaceDTO, IngredientMarketplace>();
        }
    }
}
