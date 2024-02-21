﻿using Data.Entity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly KitchenDelightsContext _context;

        public UserRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public void CreateUser(User user)
        {
            try
            {
                _context.Users.Add(user);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task<User?> GetUser(string email)
        {
            return await _context.Users.Include(x => x.Role).Include(x => x.Status).FirstOrDefaultAsync(x => x.Email == email);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}