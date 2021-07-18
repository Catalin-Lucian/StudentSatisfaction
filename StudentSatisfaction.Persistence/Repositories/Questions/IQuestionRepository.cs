using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Repositories.Questions
{
    public interface IQuestionRepository
    {
        Task Create(Question question);
        void Delete(Question question);
        IEnumerable<Question> GetAll();
        Task<Question> GetById(Guid id);
        Task SaveChanges();
        void Update(Question question);
    }
}
