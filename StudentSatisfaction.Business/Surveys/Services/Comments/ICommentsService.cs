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
        Task<CommentModel> Add(Guid surveyId, Guid userId, CreateCommentModel model);
        Task<CommentModel> GetCommentById(Guid commentId);
        Task<IEnumerable<CommentModel>> GetCommentsFromSurvey(Guid surveyId);
        Task<IEnumerable<CommentModel>> GetCommentsFromUser(Guid userId);
        Task Update(Guid commentId, UpdateCommentModel model);
        Task DeleteCommentFromSurvey(Guid surveyId, Guid commentId);
        Task DeleteCommentFromUser(Guid userId, Guid commentId);
    }
}
