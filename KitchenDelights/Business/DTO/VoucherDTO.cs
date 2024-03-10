using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class VoucherDTO
    {
        public string VoucherCode { get; set; } = null!;

        public int UserId { get; set; }

        public byte DiscountPercentage { get; set; }
    }
}
