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
    public class AdvertisemenProfile : Profile
    {
        public AdvertisemenProfile()
        {
            CreateMap<Advertisement, AdvertisementDTO>();
            CreateMap<AdvertisementDTO, Advertisement>();
        }
    }
}
