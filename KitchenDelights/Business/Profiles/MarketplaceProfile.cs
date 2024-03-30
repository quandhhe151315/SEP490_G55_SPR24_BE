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
    public class MarketplaceProfile : Profile
    {
        public MarketplaceProfile() { 
            CreateMap<Marketplace, MarketplaceDTO>();
            CreateMap<MarketplaceDTO, Marketplace>();
        }
    }
}
