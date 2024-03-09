using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class BlogCommentDTO
    {
        public int? CommentId { get; set; }

        public int BlogId { get; set; }

        public int? ParentId { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? CommentContent { get; set; }

        public int CommentStatus { get; set; } = 1;

        public DateTime CreateDate { get; set; }

        public List<BlogCommentDTO> SubComments { get; set; } = [];
    }
}
