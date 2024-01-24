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
    public class AccountProfile : Profile
    {
        public AccountProfile() {
            CreateMap<RegisterRequestDTO, Account>().ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
        }
    }
}
