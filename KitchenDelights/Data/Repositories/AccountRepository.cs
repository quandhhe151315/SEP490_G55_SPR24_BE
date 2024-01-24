using Data.Entity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly KitchenDelightsContext _context;

        public AccountRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public void CreateAccount(Account account)
        {
            try
            {
                _context.Accounts.Add(account);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task<Account?> GetAccount(string email)
        {
            return await _context.Accounts.Include(x => x.Role).Include(x => x.Status).FirstOrDefaultAsync(x => x.Email == email);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
