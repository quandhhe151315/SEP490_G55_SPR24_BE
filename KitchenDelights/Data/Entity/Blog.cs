using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Blog
{
    public int BlogId { get; set; }

    public int UserId { get; set; }

    public int CategoryId { get; set; }

    public string? BlogTitle { get; set; }

    public string? BlogContent { get; set; }

    public string? BlogImage { get; set; }

    public int BlogStatus { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual ICollection<BlogComment> BlogComments { get; set; } = new List<BlogComment>();

    public virtual Category Category { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
