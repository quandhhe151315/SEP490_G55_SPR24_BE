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
    public class UserManager : IUserManager
    {
        private IUserRepository _userRepository;
        private IMapper _mapper;

        public UserManager(IUserRepository accountRepository, IMapper mapper)
        {
            _userRepository = accountRepository;
            _mapper = mapper;
        }

        public void CreateUser(RegisterRequestDTO user)
        {
            _userRepository.CreateUser(_mapper.Map<RegisterRequestDTO, User>(user));
            _userRepository.Save();
        }

        public async Task<UserDTO?> GetUser(string email)
        {
            User? user = await _userRepository.GetUser(email);
            if (user == null)
            {
                return null;
            }
            return _mapper.Map<User, UserDTO>(user);
        }
    }
}
