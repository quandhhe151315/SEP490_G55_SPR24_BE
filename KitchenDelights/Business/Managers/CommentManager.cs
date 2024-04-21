using AutoMapper;
using Business.DTO;
using Business.Interfaces;
using Data.Entity;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Managers
{
    public class CommentManager : ICommentManager
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentManager(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<List<BlogCommentDTO>> GetComments()
        {
            List<BlogComment> comments = await _commentRepository.GetComments();
            List<BlogCommentDTO> commentDTOs = [];
            foreach (BlogComment comment in comments)
            {
                commentDTOs.Add(_mapper.Map<BlogComment, BlogCommentDTO>(comment));
            }
            return commentDTOs;
        }

        public async Task<List<BlogCommentDTO>> GetComments(int id)
        {
            List<BlogComment> comments = await _commentRepository.GetComments(id);
            List<BlogCommentDTO> commentDTOs = [];
            foreach (BlogComment comment in comments)
            {
                commentDTOs.Add(_mapper.Map<BlogComment, BlogCommentDTO>(comment));
            }
            return commentDTOs;
        }

        public async Task<int> CreateComment(BlogCommentDTO commentDTO)
        {
            _commentRepository.CreateComment(_mapper.Map<BlogCommentDTO, BlogComment>(commentDTO));
            _commentRepository.Save();
            var comments = await _commentRepository.GetComments();
            return comments[^1].CommentId;
        }

        public async Task<bool> UpdateComment(BlogCommentDTO commentDTO)
        {
            BlogComment? comment = await _commentRepository.GetComment(commentDTO.CommentId.Value);
            if (comment == null) return false;

            comment.CommentContent = commentDTO.CommentContent;

            _commentRepository.UpdateComment(comment);
            _commentRepository.Save();
            return true;
        }

        public async Task<bool> DeleteComment(int id)
        {
            BlogComment? comment = await _commentRepository.GetComment(id);
            if (comment == null) return false;

            comment.CommentStatus = 0;
            _commentRepository.UpdateComment(comment);
            _commentRepository.Save();
            return true;
        }
    }
}
