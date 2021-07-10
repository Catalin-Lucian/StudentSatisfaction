using StudentSatisfaction.Entities.Surveys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Users
{
    public sealed class User: Entity
    {
        public User(string type, string username, string password, string name, string email, DateTime birthDate, string facultyName): base()
        {
            Type = type;
            Username = username;
            Password = password;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            FacultyName = facultyName;
            SubmittedQuestions = new List<SubmittedQuestion>();
            Ratings = new List<Rating>();
            Comments = new List<Comment>();
            Notifications = new List<Notification>();
            Surveys = new List<Survey>();
            //UserSurveys = new List<UserSurvey>();
        }


        public string Type { get; private set; }
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string FacultyName { get; private set; }
        public ICollection<SubmittedQuestion> SubmittedQuestions { get; private set; }
        public ICollection<Rating> Ratings { get; private set; }
        public ICollection<Comment> Comments { get; private set; }
        public ICollection<Notification> Notifications { get; private set; }
        public ICollection<Survey> Surveys { get; private set; }
        //public ICollection<UserSurvey> UserSurveys { get; private set; }
    }
}
