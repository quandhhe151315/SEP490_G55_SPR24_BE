using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Status
{
    public int StatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
