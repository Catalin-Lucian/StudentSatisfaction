using StudentSatisfaction.Business.Surveys.Models.Topics;
using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Topics
{
    public interface ITopicsService
    {
        Task<TopicModel> GetById(Guid topicId);
        IEnumerable<TopicModel> GetAll();
        Task<TopicModel> Create(CreateTopicModel model);
        Task Update(Guid topicId, UpdateTopicModel model);
        Task Delete(Guid topicId);


        Task<IEnumerable<TopicModel>> GetAllTopicsFromSurvey(Guid surveyId);
        Task<TopicModel> AddTopicToSurvey(Guid surveyId, Guid topicId);
        Task<TopicModel> AddTopicToSurvey(Guid surveyId, CreateTopicModel model);
        Task DeleteTopicFromSurvey(Guid surveyId, Guid topicId);
        
        
        
        
    }
}
