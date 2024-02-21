﻿using AutoMapper;
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

        public async Task<bool> CreateResetToken(ForgotPasswordDTO forgotDetail)
        {
            User? user = await _userRepository.GetUser(forgotDetail.Email);
            if (user == null)
            {
                return false;
            }
            user.ResetToken = forgotDetail.ResetToken;
            user.ResetExpire = DateTime.Now.AddHours(1);

            _userRepository.UpdateUser(user);
            _userRepository.Save();

            return true;
        }

        public async Task<int> ForgetPassword(ForgotPasswordDTO forgotDetail)
        {
            User? user = await _userRepository.GetUser(forgotDetail.Email);

            if (user == null)
            {
                return 0;
            }

            if (!user.ResetToken.Equals(forgotDetail.ResetToken))
            {
                return 1;
            }

            if(DateTime.Compare(DateTime.Now, (DateTime) user.ResetExpire) > 0)
            {
                return 2;
            }

            user.PasswordHash = forgotDetail.Password;
            user.ResetToken = null;
            user.ResetExpire = null;

            _userRepository.UpdateUser(user);
            _userRepository.Save();
            return 3;
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
