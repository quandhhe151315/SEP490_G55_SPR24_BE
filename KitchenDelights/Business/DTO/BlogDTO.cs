using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class BlogDTO
    {
        public int BlogId { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public string? BlogTitle { get; set; }

        public string? BlogContent { get; set; }

        public string? BlogImage { get; set; }

        public int CommentCount { get; set; } = 0;

        public int BlogStatus { get; set; } = 1;

        public DateTime CreateDate { get; set; }
    }
}
