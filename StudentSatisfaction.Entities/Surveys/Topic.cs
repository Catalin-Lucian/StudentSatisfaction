using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class Topic: Entity
    {
        public Topic(string title, string details): base()
        {
            Title = title;
            Details = details;
            Surveys = new List<Survey>();
            //SurveyTopics = new List<SurveyTopic>();
        }

        public string Title { get; private set; }
        public string Details { get; private set; }
        public ICollection<Survey> Surveys { get; private set; }
        //public string SurveyTopics { get; private set; }
    }
}
