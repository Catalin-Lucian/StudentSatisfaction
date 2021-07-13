using AutoMapper;
using StudentSatisfaction.Business.Surveys.Models.Topics;
using StudentSatisfaction.Entities.Surveys;
using StudentSatisfaction.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Topics
{
    public sealed class TopicsService : ITopicsService
    {
        private readonly ISurveyRepository _surveyRepository;
        private readonly IMapper _mapper;

        public TopicsService(ISurveyRepository surveyRepository, IMapper mapper)
        {
            _surveyRepository = surveyRepository;
            _mapper = mapper;
        }

        public async Task<TopicModel> Add(Guid surveyId, CreateTopicModel model)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            var topic = _mapper.Map<Topic>(model);
            survey.Topics.Add(topic);

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();

            return _mapper.Map<TopicModel>(topic);
        }

        public async Task Delete(Guid surveyId, Guid topicId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var topicToRemove = survey.Topics.FirstOrDefault(t => t.Id == topicId);

            if (topicToRemove != null)
            {
                survey.Topics.Remove(topicToRemove);
            }

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();
        }

        public async Task<IEnumerable<TopicModel>> Get(Guid surveyId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            return _mapper.Map<IEnumerable<TopicModel>>(survey.Topics);
        }
    }
}
