﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Models.Comments
{
    public sealed class UpdateCommentModel
    {
        public Guid UserId { get; set; }
        public Guid SurveyId { get; set; }
        public string CommentText { get; set; }
    }
}
