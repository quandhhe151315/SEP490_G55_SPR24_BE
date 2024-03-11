using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class RoleManager : IRoleManager
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleManager(IRoleRepository roleRepository, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<List<RoleDTO>> GetRoles()
        {
            List<RoleDTO> roleDTOs = [];
            List<Role> roles = await _roleRepository.GetRoles();
            roleDTOs.AddRange(roles.Select(role => _mapper.Map<Role, RoleDTO>(role)));
            return roleDTOs;
        }
    }
}
