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
    public class AccountManager : IAccountManager
    {
        private IAccountRepository _accountRepository;
        private IMapper _mapper;

        public AccountManager(IAccountRepository accountRepository, IMapper mapper)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public void CreateAccount(RegisterRequestDTO account)
        {
            _accountRepository.CreateAccount(_mapper.Map<RegisterRequestDTO, Account>(account));
            _accountRepository.Save();
        }
    }
}
