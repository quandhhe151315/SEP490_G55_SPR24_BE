using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class RegisterRequestDTO
    {
        public string? Username { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public int StatusId { get; set; }

        public int RoleId { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
