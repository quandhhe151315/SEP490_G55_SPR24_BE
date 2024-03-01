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
    public class CartItemProfile : Profile
    {
        public CartItemProfile() {
            CreateMap<CartItem, CartItemDTO>()
                .ForMember(dest => dest.RecipeTitle, opt => opt.MapFrom(src => src.Recipe.RecipeTitle))
                .ForMember(dest => dest.RecipePrice, opt => opt.MapFrom(src => src.Recipe.RecipePrice))
                .ForMember(dest => dest.DiscountPercentage, opt => opt.MapFrom(src => src.VoucherCodeNavigation.DiscountPercentage));
            CreateMap<CartItemDTO, CartItem>();
        }
    }
}
