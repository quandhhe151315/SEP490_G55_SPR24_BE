using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class RecipeRatingDTO
    {
        public int? RatingId { get; set; }

        public int RecipeId { get; set; }

        public int UserId { get; set; }

        public string? Username { get; set; }

        public int RatingValue { get; set; }

        public string? RatingContent { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
