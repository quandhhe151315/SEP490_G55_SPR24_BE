﻿using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class RecipeRating
{
    public int RatingId { get; set; }

    public int RecipeId { get; set; }

    public int AccountId { get; set; }

    public int RatingValue { get; set; }

    public string? RatingContent { get; set; }

    public DateTime CreateDate { get; set; }

    public virtual Account Account { get; set; } = null!;

    public virtual Recipe Recipe { get; set; } = null!;
}
