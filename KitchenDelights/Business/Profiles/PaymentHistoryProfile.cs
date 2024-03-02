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
    public class PaymentHistoryProfile : Profile
    {
        public PaymentHistoryProfile() {
            CreateMap<CartItemDTO, PaymentHistoryDTO>()
                .ForMember(dest => dest.ActualPrice, opt => opt.MapFrom(src => src.RecipePrice));
            CreateMap<PaymentHistoryDTO, PaymentHistory>();
            CreateMap<PaymentHistory, PaymentHistoryDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => $"{src.User.LastName} {src.User.MiddleName} {src.User.FirstName}".Replace("  ", " ").Trim()))
                .ForMember(dest => dest.RecipeTitle, opt => opt.MapFrom(src => src.Recipe.RecipeTitle));
        }
    }
}
