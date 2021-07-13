using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models.Questions;
using StudentSatisfaction.Business.Surveys.Models.SubmittedQuestions;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.SubmittedQuestions
{
    public sealed class SubmittedQuestionService : ISubmittedQuestionsService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly IMapper _mapper;


        public SubmittedQuestionService(ISurveyRepository surveyRepository, IMapper mapper)
        {
            _surveyRepository = surveyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubmittedQuestionsModel>> Get(Guid surveyId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            return _mapper.Map<IEnumerable<SubmittedQuestionsModel>>(survey.SubmittedQuestions);
        }

        public async Task<SubmittedQuestionsModel> Add(Guid surveyId, CreateQuestionModel model)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            var submittedQuestion = _mapper.Map<SubmittedQuestion>(model);
            survey.SubmittedQuestions.Add(submittedQuestion);

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();

            return _mapper.Map<SubmittedQuestionsModel>(submittedQuestion);
        }

        public async Task Delete(Guid surveyId, Guid submittedQuestionId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var submittedQuestionToRemove = survey.SubmittedQuestions.FirstOrDefault(q => q.Id == submittedQuestionId);

            if(submittedQuestionToRemove != null)
            {
                survey.SubmittedQuestions.Remove(submittedQuestionToRemove);
            }

            _surveyRepository.Update(survey);

            await _surveyRepository.SaveChanges();
        }
    }
}
