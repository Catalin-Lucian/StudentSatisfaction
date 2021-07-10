using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Entities.Surveys
{
    public sealed class Rating:Entity
    {
        public Rating(int points,Guid questionId,Guid userId):base()
        {
            Points = points;
            QuestionId = questionId;
            UserId = userId;
        }

        public int Points { get; private set; }
        public Guid QuestionId { get; private set; }
        public Guid UserId { get; private set; }


    }
}
