using StudentSatisfaction.Business.Surveys.Models.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Questions
{
    public interface IQuestionService
    {
        Task<QuestionModel> Add(Guid surveyId, CreateQuestionModel model);

        Task<IEnumerable<QuestionModel>> Get(Guid surveyId);
        Task<QuestionModel> GetById(Guid surveyId, Guid questionId);
        Task Delete(Guid surveyId, Guid questionId);
        Task Update(Guid surveyId, Guid questionId, UpdateQuestionModel model);
    }
}
