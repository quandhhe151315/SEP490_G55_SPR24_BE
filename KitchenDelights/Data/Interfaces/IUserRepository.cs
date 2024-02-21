﻿using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IUserRepository
    {
        void CreateUser(User user);

        void UpdateUser(User user);

        Task<User?> GetUser(string email);

        Task<User?> GetUser(int id);

        void Save();
    }
}
