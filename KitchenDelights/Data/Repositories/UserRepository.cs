using Data.Entity;
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

        public void UpdateUser(User user)
        {
            try
            {
                _context.Users.Update(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public async Task<User?> GetUser(string email)
        {
            return await _context.Users.AsNoTracking().Include(x => x.Role).Include(x => x.Status).FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> GetUser(int id)
        {
            return await _context.Users.Include(x => x.Role).Include(x => x.Status).Include(x => x.Addresses).Include(x => x.Recipes).FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<User?> GetBookmarkOfUser(int id)
        {
            return await _context.Users.AsNoTracking().Include(x => x.Recipes).ThenInclude(x => x.RecipeIngredients)
                                                    .Include(x => x.Recipes).ThenInclude(x => x.RecipeRatings)
                                                    .Include(x => x.Recipes).ThenInclude(x => x.Categories)
                                                    .Include(x => x.Recipes).ThenInclude(x => x.Countries)
                                                    .FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<List<User>> GetUsers(int id)
        {
            return await _context.Users.AsNoTracking().Include(x => x.Role).Include(x => x.Status).Where(x => x.UserId != id).ToListAsync();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
