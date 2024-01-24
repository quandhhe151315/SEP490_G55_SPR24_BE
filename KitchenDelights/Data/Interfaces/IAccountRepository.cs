using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IAccountRepository
    {
        void CreateAccount(Account account);

        Task<Account?> GetAccount(string email);

        void Save();
    }
}
