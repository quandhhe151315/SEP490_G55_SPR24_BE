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

        public string? CardFront { get; set; }

        public string? CardBack { get; set; }

        public string? VerificationFront { get; set; }

        public string? VerificationBack { get; set; }

        public int VerificationStatus { get; set; }

        public DateTime VerificationDate { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
