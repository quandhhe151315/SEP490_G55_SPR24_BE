using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class CategoryDTO
    {
        public int? CategoryId { get; set; }

        public int? ParentId { get; set; }

        public string? ParentName { get; set; }

        public string? CategoryName { get; set; }

        public bool CategoryType { get; set; }

        public bool isExistRecipe { get; set; }
    }
}
