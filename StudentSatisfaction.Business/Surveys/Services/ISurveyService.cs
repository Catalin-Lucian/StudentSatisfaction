using StudentSatisfaction.Business.Surveys.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services
{
    public interface ISurveyService
    {
        IEnumerable<SurveyModel> GetAll();
        Task<SurveyModel> GetById(Guid surveyId);
        Task<SurveyModel> Create(SurveyModel model);
        Task Delete(Guid surveyId);
        Task Update(Guid surveyId, SurveyModel model);
    }
}
