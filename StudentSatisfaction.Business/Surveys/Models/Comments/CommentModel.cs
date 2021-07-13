using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Models.Comments
{
    public sealed class CommentModel
    {
        public Guid Id { get; set; }
        public string CommentText { get; set; }
    }
}
