using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class AddressDTO
    {
        public int? AddressId { get; set; }
        public int UserId { get; set; }
        public string? AddressDetails { get; set; }
    }
}
