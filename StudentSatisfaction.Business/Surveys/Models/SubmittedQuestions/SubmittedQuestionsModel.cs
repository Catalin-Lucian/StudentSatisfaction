using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Models.SubmittedQuestions
{
    public sealed class SubmittedQuestionsModel
    {
        public Guid Id { get; set; }
        public string QuestionText { get; set; }
    }
}
