using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class MenuDTO
    {
        public int MenuId { get; set; }

        public string? FeaturedImage { get; set; }

        public string? MenuName { get; set; }

        public string? MenuDescription { get; set; }

        public int UserId { get; set; }

        public bool isExistRecipe { get; set; }

        public virtual List<RecipeDTO> Recipes { get; set; } = [];
    }
}
