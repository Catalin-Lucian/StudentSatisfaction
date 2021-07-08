using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class SurveysTopics:Entity
    {
        public SurveysTopics(Guid surveyId,Guid topicId)
        {
            SurveyId = surveyId;
            TopicId = topicId;
        }

        public Guid SurveyId { get; private set; }
        public Guid TopicId { get; private set; }

    }
}
