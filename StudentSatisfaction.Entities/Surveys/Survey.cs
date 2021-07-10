using StudentSatisfaction.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class Survey: Entity
    {
        public Survey(string name, DateTime startDate, DateTime endDate): base()
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Questions = new List<Question>();
            SubmittedQuestions = new List<SubmittedQuestion>();
            Comments = new List<Comment>();
            Topics = new List<Topic>();
            Users = new List<User>();
            //UserSurveys
            //SurveyToics
        }

        public string Name { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public ICollection<Question> Questions { get; private set; }
        public ICollection<SubmittedQuestion> SubmittedQuestions { get; private set; }
        public ICollection<Comment> Comments { get; private set; }
        public ICollection<Topic> Topics { get; private set; }
        public ICollection<User> Users { get; private set; }
        //UserSurveys
        //SurveyToics
    }
}
