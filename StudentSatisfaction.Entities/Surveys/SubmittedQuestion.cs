using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class SubmittedQuestion: Entity
    {
        public SubmittedQuestion(Guid surveyId, Guid userId, string questionText): base()
        {
            SurveyId = surveyId;
            UserId = userId;
            QuestionText = questionText;
        }

        public Guid SurveyId { get; private set; }
        public Guid UserId { get; private set; }
        public string QuestionText { get; private set; }
    }
}
