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

        Task<bool> CreateResetToken(ForgotPasswordDTO forgotDetail);

        Task<int> ForgetPassword(ForgotPasswordDTO forgotDetail);

        Task<UserDTO?> GetUser(string email);
    }
}
