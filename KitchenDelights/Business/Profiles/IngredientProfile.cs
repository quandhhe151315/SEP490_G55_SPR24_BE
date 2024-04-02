using AutoMapper;
using Business.DTO;
using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Profiles
{
    public class IngredientProfile : Profile
    {
        public IngredientProfile()
        {
            CreateMap<IngredientMarketplace, IngredientMarketplaceDTO>();
            CreateMap<IngredientMarketplaceDTO, IngredientMarketplace>();

            CreateMap<Ingredient, IngredientDTO>()
                .ForMember(dest => dest.IngredientMarketplaces, opt => opt.MapFrom(src => src.IngredientMarketplaces));
            CreateMap<IngredientDTO, Ingredient>();

            CreateMap<Ingredient, IngredientRequestDTO>();
            CreateMap<IngredientRequestDTO, Ingredient>();
        }
    }
}
