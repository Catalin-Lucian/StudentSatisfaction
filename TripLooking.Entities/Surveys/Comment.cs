using System;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class Comment:Entity
    {
        public Comment(string textComment, Guid userId, Guid surveyId):base()
        {
            TextComment = textComment;
            UserId = userId;
            SurveyId = surveyId;
        }

        public string TextComment { get; private set; }

        public Guid UserId { get; private set; }

        public Guid SurveyId { get; private set; }
    }
}