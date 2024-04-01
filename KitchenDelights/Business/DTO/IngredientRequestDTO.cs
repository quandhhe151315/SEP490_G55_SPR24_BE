using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class IngredientRequestDTO
    {
        public int IngredientId { get; set; }

        public string? IngredientName { get; set; }

        public string? IngredientUnit { get; set; }

        public int IngredientStatus { get; set; }
    }
}
