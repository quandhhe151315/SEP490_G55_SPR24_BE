using Data.Entity;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly KitchenDelightsContext _context;

        public CommentRepository(KitchenDelightsContext context) 
        {
            _context = context;
        }

        public void AddComment(Comment comment) 
        {
            try
            {
                _context.Comments.Add(comment);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void UpdateComment(Comment comment)
        {
            try
            {
                _context.Comments.Update(comment);
            } catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void DeleteComment(Comment comment)
        {
            try
            {
                _context.Comments.Remove(comment);
            } catch (Exception ex) {
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}