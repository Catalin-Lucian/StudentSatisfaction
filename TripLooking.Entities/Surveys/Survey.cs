using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Survey
{
    public sealed class Survey : Entity
    {
        public Survey(string name, DateTime startDate, DateTime endDate) : base()
        {
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            Topics = new List<Topic>();
            Questions = new List<Question>();
            SubmittedQuestions = new List<SubmittedQuestion>();
            Comments = new List<Comment>();

        }

        public string Name { get; private set; }

        public DateTime StartDate { get; private set; }

        public DateTime EndDate { get; private set; }

        public ICollection<Topic> Topics { get; private set; }
        public ICollection<Question> Questions { get; private set; }
        public ICollection<SubmittedQuestion> SubmittedQuestions { get; private set; }

        public ICollection<Comment> Comments { get; private set; }

    }

  
}
