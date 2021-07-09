using System.Collections.Generic;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class Topic:Entity
    {
        public Topic(string title,string details):base()
        {
            Title = title;
            Details = details;
            //new
            SurveysTopics = new List<SurveysTopics>();
        }

        public string Title { get; private set; }
        public string Details { get; private set; }
        //new
        public ICollection<SurveysTopics> SurveysTopics { get; private set; }
    }
}