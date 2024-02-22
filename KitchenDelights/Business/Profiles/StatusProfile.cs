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
    public class StatusProfile : Profile
    {
        public StatusProfile() { 
            CreateMap<Status, StatusDTO>();
            CreateMap<StatusDTO, Status>();
        }
    }
}
