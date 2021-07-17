using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Repositories.SubmittedQuestions
{
    public interface ISubmittedQuestionRepository
    {
        IEnumerable<SubmittedQuestion> GetAll();
        Task<SubmittedQuestion> GetSubmittedQuestionById(Guid id);
        Task Create(SubmittedQuestion submittedQuestion);
        void Update(SubmittedQuestion submittedQuestion);
        void Delete(SubmittedQuestion submittedQuestion);
        Task SaveChanges();
    }
}
