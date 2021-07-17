using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Repositories.SubmittedQuestions
{
    public sealed class SubmittedQuestionRepository : ISubmittedQuestionRepository
    {
        private readonly SurveysContext _context;

        public SubmittedQuestionRepository(SurveysContext context)
        {
            _context = context;
        }

        public async Task Create(SubmittedQuestion submittedQuestion)
        {
            await _context.SubmittedQuestions.AddAsync(submittedQuestion);
        }

        public void Delete(SubmittedQuestion submittedQuestion)
        {
            _context.SubmittedQuestions.Remove(submittedQuestion);
        }

        public IEnumerable<SubmittedQuestion> GetAll()
        {
            return _context.SubmittedQuestions;
        }

        public async Task<SubmittedQuestion> GetSubmittedQuestionById(Guid id)
        {
            return await _context.SubmittedQuestions.FirstAsync(ctor => ctor.Id == id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(SubmittedQuestion submittedQuestion)
        {
            _context.SubmittedQuestions.Update(submittedQuestion);
        }
    }
}
