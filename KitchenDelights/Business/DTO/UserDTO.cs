using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string? Username { get; set; }

        public string? FirstName { get; set; }

        public string? MiddleName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Avatar { get; set; }

        public string? PasswordHash { get; set; }

        public string StatusName { get; set; }

        public string RoleName { get; set; }
    }
}
