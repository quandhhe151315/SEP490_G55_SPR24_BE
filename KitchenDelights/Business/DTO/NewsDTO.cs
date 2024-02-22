using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DTO
{
    public class NewsDTO
    {
        public int? NewsId { get; set; }

        public int UserId { get; set; }

        public string? UserName { get; set; }

        public string? NewsTitle { get; set; }

        public string? NewsContent { get; set; }

        public bool NewsStatus { get; set; }

        public DateTime? CreateDate { get; set; }
    }
}
