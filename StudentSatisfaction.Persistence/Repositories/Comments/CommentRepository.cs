using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Repositories.Comments
{
    public sealed class CommentRepository : ICommentRepository
    {
        private readonly SurveysContext _context;

        public CommentRepository(SurveysContext context)
        {
            _context = context;
        }

        public async Task Create(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
        }

        public void Delete(Comment comment)
        {
            _context.Comments.Remove(comment);
        }

        public IEnumerable<Comment> GetAll()
        {
            return _context.Comments;
        }

        public async Task<Comment> GetCommentById(Guid id)
        {
            return await _context.Comments.FirstAsync(c => c.Id == id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Comment comment)
        {
            _context.Comments.Update(comment);
        }
    }
}
