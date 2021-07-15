using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Persistence
{
    public interface ISurveyRepository
    {
        IEnumerable<Survey> GetAll();
        Task<Survey> GetSurveyById(Guid id);
        Task Create(Survey survey);
        void Update(Survey survey);
        void Delete(Survey survey);
        Task SaveChanges();
        void CreateTopic(Topic topic);
    }
}
