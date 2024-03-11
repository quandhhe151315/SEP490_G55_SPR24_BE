using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class AdvertisementDTO
    {
        public int? AdvertisementId { get; set; }

        public string? AdvertisementImage { get; set; }

        public string? AdvertisementLink { get; set; }

        public int AdvertisementStatus { get; set; }
    }
}
