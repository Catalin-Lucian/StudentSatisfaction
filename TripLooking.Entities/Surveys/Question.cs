using System;
using System.Collections.Generic;
using StudentSatisfaction.Entities.Surveys;

namespace StudentSatisfaction.Entities.Survey
{
    public sealed class Question:Entity
    {
        public Question(string textQuestion,Guid surveyId):base()
        {
            TextQuestion = textQuestion;
            SurveyId = surveyId;
            Ratings = new List<Rating>();
        }

        public string TextQuestion { get; private set; }
        public Guid SurveyId { get; private set; }

        public ICollection<Rating> Ratings { get; private set; }

    }
}