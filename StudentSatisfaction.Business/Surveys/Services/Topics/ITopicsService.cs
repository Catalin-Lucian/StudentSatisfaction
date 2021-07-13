using StudentSatisfaction.Business.Surveys.Models.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Topics
{
    public interface ITopicsService
    {
        Task<TopicModel> Add(Guid surveyId, CreateTopicModel model);

        Task<IEnumerable<TopicModel>> Get(Guid surveyId);

        Task Delete(Guid surveyId, Guid topicId);
    }
}
