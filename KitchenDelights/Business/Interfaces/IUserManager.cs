using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserManager
    {
        void CreateUser(RegisterRequestDTO user);

        void CreateUser(CreateUserDTO user);

        Task<bool> CreateResetToken(ForgotPasswordDTO forgotDetail);

        Task<int> ForgetPassword(ForgotPasswordDTO forgotDetail);

        Task<bool> ChangePassword(ChangePasswordDTO changeDetail);

        Task<bool> UpdateProfile(UserDTO user);

        Task<UserDTO?> GetUser(string email);

        Task<UserDTO?> GetUser(int id);

        Task<List<UserDTO>> GetUsers(int id);

        Task<bool> UpdateRole(int userId, int roleId);

        Task<bool> UpdateStatus(int userId, int statusId);
    }
}
