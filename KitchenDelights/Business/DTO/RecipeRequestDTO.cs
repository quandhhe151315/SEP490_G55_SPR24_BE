﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class RecipeRequestDTO
    {
        public int? RecipeId { get; set; }

        public int UserId { get; set; }

        public string? FeaturedImage { get; set; }

        public string? VideoLink { get; set; }

        public string? RecipeTitle { get; set; }

        public int? RecipeServe { get; set; }

        public string? RecipeContent { get; set; }

        public decimal RecipeRating { get; set; }

        public bool RecipeStatus { get; set; }

        public bool IsFree { get; set; }

        public decimal? RecipePrice { get; set; }

        public DateTime CreateDate { get; set; }

        public virtual List<RecipeIngredientRequestDTO> RecipeIngredients { get; set; } = [];
    }
}