using Business.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICommentManager
    {
        Task<List<BlogCommentDTO>> GetComments();
        Task<List<BlogCommentDTO>> GetComments(int id);
        void CreateComment(BlogCommentDTO commentDTO);
        Task<bool> UpdateComment(BlogCommentDTO commentDTO);
        Task<bool> DeleteComment(int id);
    }
}
