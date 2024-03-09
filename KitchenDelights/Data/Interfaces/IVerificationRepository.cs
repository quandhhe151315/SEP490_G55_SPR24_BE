using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IVerificationRepository
    {
        Task<Verification?> GetVerification(int id);
        Task<List<Verification>> GetVerifications();
        void CreateVerification(Verification verification);
        void UpdateVerification(Verification verification);
        void Save();
    }
}
