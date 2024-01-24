using Data.Entity;
using Data.Interfaces;

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

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
