using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Address
{
    public int AddressId { get; set; }

    public int UserId { get; set; }

    public string? AddressDetails { get; set; }

    public virtual User User { get; set; } = null!;
}
