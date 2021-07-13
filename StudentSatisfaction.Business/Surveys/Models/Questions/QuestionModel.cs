using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Models.Questions
{
    public sealed class QuestionModel
    {
        public Guid Id { get; set; }
        public Guid SurveyId { get; set; }
        public string QuestionText { get; set; }
        public string Type { get; set; }
    }
}
