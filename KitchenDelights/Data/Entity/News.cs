using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class News
{
    public int NewsId { get; set; }

    public int AccountId { get; set; }

    public string? NewsTitle { get; set; }

    public string? NewsContent { get; set; }

    public bool NewsStatus { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual Account Account { get; set; } = null!;
}
