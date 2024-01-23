using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Blog
{
    public int BlogId { get; set; }

    public int AccountId { get; set; }

    public string? BlogTitle { get; set; }

    public string? BlogContent { get; set; }

    public string? BlogImage { get; set; }

    public bool? BlogStatus { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual ICollection<BlogComment> BlogComments { get; set; } = new List<BlogComment>();
}
