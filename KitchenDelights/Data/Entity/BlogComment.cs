using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class BlogComment
{
    public int CommentId { get; set; }

    public int BlogId { get; set; }

    public int? ParentId { get; set; }

    public int UserId { get; set; }

    public string? CommentContent { get; set; }

    public int CommentStatus { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Blog Blog { get; set; } = null!;

    public virtual ICollection<BlogComment> InverseParent { get; set; } = new List<BlogComment>();

    public virtual BlogComment? Parent { get; set; }

    public virtual User User { get; set; } = null!;
}
