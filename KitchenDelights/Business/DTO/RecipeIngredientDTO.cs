using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class RecipeIngredientDTO
    {
        public int IngredientId { get; set; }

        public string? IngredientName {  get; set; }

        public decimal? UnitValue { get; set; }

        public string? IngredientUnit { get; set; }

        public decimal? UnitPersonValue { get; set; } = 0;

        public List<IngredientMarketplaceDTO> IngredientMarketplaces { get; set; } = [];

    }
}
