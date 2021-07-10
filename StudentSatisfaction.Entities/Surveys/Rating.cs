using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class Rating: Entity
    {
        public Rating(Guid questionId, Guid userId, int points, string answear): base()
        {
            QuestionId = questionId;
            UserId = userId;
            Points = points;
            Answear = answear;
        }

        public Guid QuestionId { get; private set; }
        public Guid UserId { get; private set; }
        public int Points { get; private set; }
        public string Answear { get; private set; }
    }
}
