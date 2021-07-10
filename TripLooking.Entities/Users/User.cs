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
            SubmittedQuestions = new List<SubmittedQuestion>();  //new
            Ratings = new List<Rating>();           //new
            Comments = new List<Comment>();        //new
        }

        public string Type { get; private set; }
        public Guid PersonalDataId { get;private set; }

        public Guid LogInId { get; private set; }

        //new
        public ICollection<UserSurvey> UserSurveys { get; private set; }
        //new
        public ICollection<SubmittedQuestion> SubmittedQuestions { get; private set; }
        //new
        public ICollection<Rating> Ratings { get; private set; }
        //new
        public ICollection<Comment> Comments { get; private set; }
    }
}
