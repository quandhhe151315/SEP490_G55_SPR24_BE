using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class CreateUserDTO
    {
         public string? Username { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Avatar { get; set; }

        public string? Password { get; set; }

        public int StatusId { get; set; }

        public int RoleId { get; set; }
    }
}
