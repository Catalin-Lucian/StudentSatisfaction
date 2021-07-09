using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentSatisfaction.Entities;
using StudentSatisfaction.Entities.Surveys;

namespace StudentSatisfaction.Entities.Users
{
    public sealed class User:Entity
    {
        public User(string type, Guid personalDataId,Guid logInId):base()
        {
            Type = type;
            PersonalDataId = personalDataId;
            LogInId = logInId;
            UserSurveys = new List<UserSurvey>();    //new
            //PersonalData= new PeersonalDetails();
        }

        public string Type { get; private set; }
        public Guid PersonalDataId { get;private set; }

        public Guid LogInId { get; private set; }

        //new
        public ICollection<UserSurvey> UserSurveys { get; private set; }
    }
}
