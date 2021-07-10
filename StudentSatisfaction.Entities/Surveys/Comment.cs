using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class Comment: Entity
    {
        public Comment(Guid userId, Guid surveyId, string commentText): base()
        {
            UserId = userId;
            SurveyId = surveyId;
            CommentText = commentText;
        }

        public Guid UserId { get; private set; }
        public Guid SurveyId { get; private set; }
        public string CommentText { get; private set; }
    }
}
