using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class UserSurvey:Entity
    {
        public UserSurvey(int totalScore,Guid surveyId,Guid userId)
        {
            TotalScore = totalScore;
            SurveyId = surveyId;
            UserId = userId;
        }

        public int TotalScore { get; private set; }
        public Guid SurveyId { get; private set; }
        public Guid UserId { get; private set; }
    }

}
