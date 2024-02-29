using Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface ICommentRepository
    {
        void AddComment(Comment comment);
        void UpdateComment(Comment comment);
        void DeleteComment(Comment comment);
        void Save();
    }
}