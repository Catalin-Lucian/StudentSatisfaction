using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Models.Ratings
{
    public sealed class RatingModel
    {
        public Guid Id { get; private set; }
        public Guid QuestionId { get; private set; }
        public Guid UserId { get; private set; }
        public int Points { get; private set; }
        public string Answear { get; private set; }
    }
}
