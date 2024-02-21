using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class ChangePasswordDTO
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }
}
