using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Advertisement
{
    public int AdvertisementId { get; set; }

    public string? AdvertisementImage { get; set; }

    public string? AdvertisementLink { get; set; }

    public bool AdvertisementStatus { get; set; }
}
