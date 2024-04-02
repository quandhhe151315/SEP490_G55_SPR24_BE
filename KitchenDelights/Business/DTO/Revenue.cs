using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class Revenue
    {
        public decimal revenue {  get; set; }
        public float percent { get; set; }
        public bool increase { get; set; }
    }
}
