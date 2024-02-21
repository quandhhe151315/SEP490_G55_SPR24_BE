using System;
using System.Collections.Generic;

namespace Data.Entity;

public partial class Ingredient
{
    public int IngredientId { get; set; }

    public string? IngredientName { get; set; }

    public string? IngredientUnit { get; set; }

    public virtual ICollection<IngredientMarketplace> IngredientMarketplaces { get; set; } = new List<IngredientMarketplace>();

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
