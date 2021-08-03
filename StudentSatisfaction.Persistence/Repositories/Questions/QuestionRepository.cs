using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence.Repositories.Questions
{
    public sealed class QuestionRepository: IQuestionRepository
    {
        private readonly SurveysContext _context;

        public QuestionRepository(SurveysContext context)
        {
            _context = context;
        }

        public async Task Create(Question question)
        {
            await _context.Questions.AddAsync(question);
        }

        public void Delete(Question question)
        {
            _context.Questions.Remove(question);
        }

        public IEnumerable<Question> GetAll()
        {
            return  _context.Questions
                .Include(x => x.Ratings);
        }

        public async Task<Question> GetById(Guid id)
        {
            return await _context.Questions
                .Include(x => x.Ratings)
                .FirstAsync(c => c.Id == id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }

        public void Update(Question question)
        {
            _context.Questions.Update(question);
        }
    }
}
