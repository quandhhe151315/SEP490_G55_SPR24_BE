using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Menu
{
    public int MenuId { get; set; }

    public string? MenuName { get; set; }

    public int AccountId { get; set; }

    public int RecipeId { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
