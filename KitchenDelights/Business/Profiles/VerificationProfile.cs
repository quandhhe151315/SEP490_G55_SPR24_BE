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
    public class VerificationProfile : Profile
    {
        public VerificationProfile() { 
            CreateMap<Verification, VerificationDTO>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => $"{src.User.LastName} {src.User.MiddleName} {src.User.FirstName}".Replace("  ", " ").Trim()));
            CreateMap<VerificationDTO, Verification>();
        }
    }
}
