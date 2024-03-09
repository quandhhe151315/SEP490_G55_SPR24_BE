using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Category
{
    public int CategoryId { get; set; }

    public int? ParentId { get; set; }

    public string? CategoryName { get; set; }

    public bool CategoryType { get; set; }

    public int CategoryStatus { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; } = new List<Blog>();

    public virtual ICollection<Category> InverseParent { get; set; } = new List<Category>();

    public virtual Category? Parent { get; set; }

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
}
