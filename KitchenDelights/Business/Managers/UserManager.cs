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
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

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

        public void CreateUser(UserDTO user)
        {
            _userRepository.CreateUser(_mapper.Map<UserDTO,  User>(user));
            _userRepository.Save();
        }

        public async Task<bool> CreateResetToken(ForgotPasswordDTO forgotDetail)
        {
            User? user = await _userRepository.GetUser(forgotDetail.Email);
            if (user == null) return false;
            user.ResetToken = forgotDetail.ResetToken;
            user.ResetExpire = DateTime.Now.AddHours(1);

            _userRepository.UpdateUser(user);
            _userRepository.Save();

            return true;
        }

        public async Task<int> ForgetPassword(ForgotPasswordDTO forgotDetail)
        {
            User? user = await _userRepository.GetUser(forgotDetail.Email);

            if (user == null) return 0;

            if (!user.ResetToken.Equals(forgotDetail.ResetToken)) return 1;

            if(DateTime.Compare(DateTime.Now, (DateTime) user.ResetExpire) > 0) return 2;

            user.PasswordHash = forgotDetail.Password;
            user.ResetToken = null;
            user.ResetExpire = null;

            _userRepository.UpdateUser(user);
            _userRepository.Save();
            return 3;
        }

        public async Task<bool> ChangePassword(ChangePasswordDTO changeDetail)
        {
            User? user = await _userRepository.GetUser(changeDetail.UserId);
            if (user == null) return false;
            user.PasswordHash = changeDetail.Password;

            _userRepository.UpdateUser(user);
            _userRepository.Save();
            return true;
        }

        public async Task<bool> UpdateProfile(UserDTO userDTO)
        {
            User? user = await _userRepository.GetUser(userDTO.UserId);
            if (user == null) return false;

            //Update only Profile-related fields
            user.FirstName = userDTO.FirstName;
            user.MiddleName = userDTO.MiddleName;
            user.LastName = userDTO.LastName;
            user.Email = userDTO.Email;
            user.Phone = userDTO.Phone;
            user.Addresses.Clear();
            foreach(AddressDTO address in userDTO.Addresses)
            {
                user.Addresses.Add(_mapper.Map<AddressDTO, Address>(address));
            }
            user.Avatar = userDTO.Avatar;

            _userRepository.UpdateUser(user);
            _userRepository.Save();
            return true;
        }

        public async Task<UserDTO?> GetUser(string email)
        {
            User? user = await _userRepository.GetUser(email);

            return user == null ? null : _mapper.Map<User, UserDTO>(user);
        }

        public async Task<UserDTO?> GetUser(int id)
        {
            User? user = await _userRepository.GetUser(id);

            return user == null ? null : _mapper.Map<User, UserDTO>(user);
        }

        public async Task<List<UserDTO>> GetUsers(int id)
        {
            List<UserDTO> userDTOs = [];
            List<User> users = await _userRepository.GetUsers(id);
            foreach (User user in users)
            {
                userDTOs.Add(_mapper.Map<User, UserDTO>(user));
            }
            return userDTOs;
        }
    }
}
