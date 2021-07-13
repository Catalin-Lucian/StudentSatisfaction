using StudentSatisfaction.Business.Surveys.Models.Questions;
using StudentSatisfaction.Business.Surveys.Models.SubmittedQuestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.SubmittedQuestions
{
    public interface ISubmittedQuestionsService
    {
        Task<SubmittedQuestionsModel> Add(Guid surveyId, CreateQuestionModel model);

        Task<IEnumerable<SubmittedQuestionsModel>> Get(Guid surveyId);

        Task Delete(Guid surveyId, Guid submittedQuestionId);
    }
}
