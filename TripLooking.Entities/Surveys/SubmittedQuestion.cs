using System;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class SubmittedQuestion:Entity
    {
        public SubmittedQuestion(Guid surveyId,Guid userId,string textQuestion):base()
        {
            SurveyId = surveyId;
            UserId = userId;
            TextQuestion = textQuestion;
        }

        public Guid SurveyId { get; private set; }
        public Guid UserId { get; private set; }
        public string TextQuestion { get; private set; }

    }
}