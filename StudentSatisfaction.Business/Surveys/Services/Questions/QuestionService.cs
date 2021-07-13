using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models.Questions;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Questions
{
    public sealed class QuestionService : IQuestionService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly IMapper _mapper;


        public QuestionService(ISurveyRepository surveyRepository, IMapper mapper)
        {
            _surveyRepository = surveyRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuestionModel>> Get(Guid surveyId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            return _mapper.Map<IEnumerable<QuestionModel>>(survey.Questions);
        }

        public async Task<QuestionModel> Add(Guid surveyId, CreateQuestionModel model)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            var question = _mapper.Map<Question>(model);
            survey.Questions.Add(question);

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();

            return _mapper.Map<QuestionModel>(question);
        }

        public async Task Delete(Guid surveyId, Guid questionId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var questionToRemove = survey.Questions.FirstOrDefault(c => c.Id == questionId);

            if (questionToRemove != null)
            {
                survey.Questions.Remove(questionToRemove);
            }

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();
        }
    }
}
