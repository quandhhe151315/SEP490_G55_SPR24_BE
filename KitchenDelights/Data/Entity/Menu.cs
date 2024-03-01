using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Menu
{
    public int MenuId { get; set; }

    public string? FeaturedImage { get; set; }

    public string? MenuName { get; set; }

    public string? MenuDescription { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
