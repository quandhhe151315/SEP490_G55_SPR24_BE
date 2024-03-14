using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICommentRepository
    {
        void CreateComment(BlogComment comment);
        void UpdateComment(BlogComment comment);
        void DeleteComment(BlogComment comment);
        Task<BlogComment?> GetComment(int id);
        Task<List<BlogComment>> GetComments();
        Task<List<BlogComment>> GetComments(int id);
        void Save();
    }
}