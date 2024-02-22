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
    public class AddressProfile : Profile
    {
        public AddressProfile() { 
            CreateMap<Address, AddressDTO>();
            CreateMap<AddressDTO, Address>();
        }
    }
}
