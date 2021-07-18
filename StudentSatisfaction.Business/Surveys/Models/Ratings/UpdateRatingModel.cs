using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Models.Ratings
{
    public sealed class UpdateRatingModel
    {
        public Guid QuestionId { get; set; }
        public Guid UserId { get; set; }
        public int Points { get; set; }
        public string Answear { get; set; }
    }
}
