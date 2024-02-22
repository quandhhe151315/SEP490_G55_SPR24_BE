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
    public class NewsProfile : Profile
    {
        public NewsProfile() {
            CreateMap<NewsDTO, News>()
                .ForSourceMember(src => src.UserName, opt => opt.DoNotValidate());
            CreateMap<News, NewsDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => $"{src.User.LastName} {src.User.MiddleName} {src.User.FirstName}".Replace("  ", " ").Trim()));
        }
    }
}
