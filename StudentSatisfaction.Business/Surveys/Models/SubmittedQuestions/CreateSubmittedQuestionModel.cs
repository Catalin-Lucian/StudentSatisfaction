using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Models.SubmittedQuestions
{
    public sealed class CreateSubmittedQuestionModel
    {
        public Guid SurveyId { get; set; }
        public Guid UserId { get; set; }
        public string QuestionText { get; set; }
    }
}
