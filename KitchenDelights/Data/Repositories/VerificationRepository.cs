using Data.Entity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class VerificationRepository : IVerificationRepository
    {
        private readonly KitchenDelightsContext _context;

        public VerificationRepository(KitchenDelightsContext context)
        {
            _context = context;
        }

        public async Task<Verification?> GetVerification(int id)
        {
            return await _context.Verifications.AsNoTracking().Include(x => x.User).FirstOrDefaultAsync(x => x.VerificationId == id);
        }

        public async Task<List<Verification>> GetVerifications()
        {
            return await _context.Verifications.AsNoTracking().Include(x => x.User).ToListAsync();
        }

        public void CreateVerification(Verification verification)
        {
            try
            {
                _context.Verifications.Add(verification);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateVerification(Verification verification)
        {
            try
            {
                _context.Verifications.Update(verification);
            }
            catch (Exception ex)
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
