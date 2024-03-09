using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class VerificationDTO
    {
        public int? VerificationId { get; set; }

        public int? UserId { get; set; }

        public string? Username { get; set; }

        public string? VerificationPath { get; set; }

        public int VerificationStatus { get; set; }

        public DateTime VerificationDate { get; set; }
    }
}
