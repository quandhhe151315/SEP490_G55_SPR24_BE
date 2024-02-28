using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class BookmarkDTO
    {
        public int UserId { get; set; }

        public virtual List<RecipeDTO> Recipes { get; set; } = [];
    }
}
