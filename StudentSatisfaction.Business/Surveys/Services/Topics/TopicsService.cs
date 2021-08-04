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
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;


        public TopicsService(ISurveyRepository surveyRepository, ITopicRepository topicRepository, IMapper mapper)
        {
            _surveyRepository = surveyRepository;
            _topicRepository = topicRepository;
            _mapper = mapper;
        }


        public IEnumerable<TopicModel> GetAll()
        {
            return _mapper.Map<IEnumerable<TopicModel>>(_topicRepository.GetAll());
        }

        public async Task<TopicModel> GetById(Guid topicId)
        {
            var topic = await _topicRepository.GetTopicById(topicId);

            return _mapper.Map<TopicModel>(topic);
        }

        public async Task<TopicModel> Create(CreateTopicModel model)
        {
            var topic = _mapper.Map<Topic>(model);
            await _topicRepository.Create(topic);
            await _topicRepository.SaveChanges();

            return _mapper.Map<TopicModel>(topic);
        }

        public async Task Delete(Guid topicId)
        {
            var topic = await _topicRepository.GetTopicById(topicId);

            _topicRepository.Delete(topic);
            await _topicRepository.SaveChanges();
        }

        public async Task Update(Guid topicId, UpdateTopicModel model)
        {
            var topic = await _topicRepository.GetTopicById(topicId);
            _mapper.Map(model, topic);

            _topicRepository.Update(topic);
            await _topicRepository.SaveChanges();
        }

        //ADAUGARE DUPA MODEL -- functia nu e folosita in SurveyController
        public async Task<TopicModel> AddTopicToSurvey(Guid surveyId, CreateTopicModel model)
        {
            //creez noul topic
            var topic = _mapper.Map<Topic>(model);
            await _topicRepository.Create(topic);
            await _topicRepository.SaveChanges();

            var survey = await _surveyRepository.GetSurveyById(surveyId);

            survey.Topics.Add(topic);

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();

            return _mapper.Map<TopicModel>(topic);
        }

        //ADAUGARE DUPA TOPIC ID -- functie folosita in SurveyController
        public async Task<TopicModel> AddTopicToSurvey(Guid surveyId, Guid topicId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);
            var topic = await _topicRepository.GetTopicById(topicId);

            //adaug topic-ul primit 
            survey.Topics.Add(topic);

            _surveyRepository.Update(survey);
            await _surveyRepository.SaveChanges();


            return _mapper.Map<TopicModel>(topic);
        }

        public async Task DeleteTopicFromSurvey(Guid surveyId, Guid topicId)
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

        public async Task<IEnumerable<TopicModel>> GetAllTopicsFromSurvey(Guid surveyId)
        {
            var survey = await _surveyRepository.GetSurveyById(surveyId);

            return _mapper.Map<IEnumerable<TopicModel>>(survey.Topics);
        }
    }
}
