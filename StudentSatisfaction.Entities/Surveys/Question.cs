using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class Question: Entity
    {
        public Question(Guid surveyId, string type, string questionText): base()
        {
            SurveyId = surveyId;
            Type = type;
            QuestionText = questionText;

        }

        public Guid SurveyId { get; private set; }
        public string Type { get; private set; }
        public string QuestionText { get; private set; }
        public ICollection<Rating> Ratings { get; private set; }
}
}
