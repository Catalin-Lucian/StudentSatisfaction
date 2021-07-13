using Microsoft.EntityFrameworkCore;
using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence
{
    public class SurveyRepository : ISurveyRepository
    {
        private readonly SurveysContext _context;

        public SurveyRepository(SurveysContext context)
        {
            _context = context;
        }

        public async Task Create(Survey survey)
        {
            await _context.Surveys.AddAsync(survey);
        }

        public void Update(Survey survey)
        {
            _context.Surveys.Update(survey);
        }

        public void Delete(Survey survey)
        {
            _context.Remove(survey);
        }

        public IEnumerable<Survey> GetAll()
        {
            return _context.Surveys;
        }

        public async Task<Survey> GetSurveyById(Guid id)
        {
            return await _context.Surveys
                .Include(x => x.Topics)
                .Include(x => x.Questions)
                .Include(x => x.SubmittedQuestions)
                .Include(x => x.Comments)
                .Include(x => x.Users)
                .FirstAsync(i => i.Id == id);
            //return await _context.Surveys.FindAsync(id);
        }

        public async Task SaveChanges()
        {
            await _context.SaveChangesAsync();
        }
    }
}
