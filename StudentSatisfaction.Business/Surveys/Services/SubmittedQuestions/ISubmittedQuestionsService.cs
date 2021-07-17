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
        Task<SubmittedQuestionsModel> Add(Guid surveyId, Guid userId, CreateSubmittedQuestionModel model);
        Task<IEnumerable<SubmittedQuestionsModel>> GetAllFromSurvey(Guid surveyId);
        Task<IEnumerable<SubmittedQuestionsModel>> GetAllFromUser(Guid userId);
        Task<SubmittedQuestionsModel> GetQuestionFromUser(Guid userId, Guid questionId);
        Task<SubmittedQuestionsModel> GetQuestionFromSurvey(Guid surveyId, Guid questionId);
        Task Update(Guid surveyId, Guid submittedQuestionId, UpdateSubmittedQuestionModel model);
        Task DeleteFromSurvey(Guid surveyId, Guid submittedQuestionId);
        Task DeleteFromUser(Guid userId, Guid submittedQuestionId);
    }
}
