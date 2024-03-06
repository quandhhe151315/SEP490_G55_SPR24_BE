using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class RecipeIngredientRequestDTO
    {
        public int IngredientId { get; set; }

        public decimal? UnitValue { get; set; }
    }
}
