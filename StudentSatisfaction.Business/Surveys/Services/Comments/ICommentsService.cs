using StudentSatisfaction.Business.Surveys.Models.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Comments
{
    public interface ICommentsService
    {
        Task<CommentModel> Add(Guid surveyId, CreateCommentModel model);
        Task<IEnumerable<CommentModel>> Get(Guid surveyId);
        Task Delete(Guid surveyId, Guid commentId);
    }
}
