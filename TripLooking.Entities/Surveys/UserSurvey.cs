using StudentSatisfaction.Entities.Users;
using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class UserSurvey:Entity
    {
        public UserSurvey(int totalScore, Guid surveyId, Guid userId): base()
        {
            TotalScore = totalScore;
            SurveyId = surveyId;
            UserId = userId;
            Users = new List<User>();           //new
            Surveys = new List<Survey>();       //new
        }

        public int TotalScore { get; private set; }
        public Guid SurveyId { get; private set; }
        public Guid UserId { get; private set; }
        public ICollection<User> Users { get; private set; }        //new
        public ICollection<Survey> Surveys { get; private set; }    //new
    }

}
