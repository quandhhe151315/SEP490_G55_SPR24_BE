﻿using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class News
{
    public int NewsId { get; set; }

    public int UserId { get; set; }

    public string? FeaturedImage { get; set; }

    public string? NewsTitle { get; set; }

    public string? NewsContent { get; set; }

    public int NewsStatus { get; set; }

    public DateTime? CreateDate { get; set; }

    public virtual User User { get; set; } = null!;
}
