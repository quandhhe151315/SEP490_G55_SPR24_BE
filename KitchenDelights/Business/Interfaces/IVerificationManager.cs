using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IVerificationManager
    {
        Task<List<VerificationDTO>> GetVerifications();
        Task<VerificationDTO?> GetVerification(int id);
        Task<bool> CreateVerification(VerificationDTO verification);
        Task<bool> UpdateVerification(VerificationDTO verification);
    }
}
