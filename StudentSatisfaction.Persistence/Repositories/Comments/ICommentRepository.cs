using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Repositories.Comments
{
    public interface ICommentRepository
    {
        Task Create(Comment comment);
        void Update(Comment comment);
        void Delete(Comment comment);
        IEnumerable<Comment> GetAll();
        Task<Comment> GetCommentById(Guid id);
        Task SaveChanges();
    }
}
