﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class RecipeRatingDTO
    {
        public int RatingValue { get; set; }

        public string? RatingContent { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
