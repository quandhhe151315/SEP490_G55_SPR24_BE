using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAccountManager
    {
        void CreateAccount(RegisterRequestDTO account);

        Task<AccountDTO?> GetAccount(string email);
    }
}
