using StudentSatisfaction.Business.Surveys.Models.Ratings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentSatisfaction.Business.Surveys.Services.Ratings
{
    public interface IRatingService
    {
        Task<RatingModel> GetRating(Guid questionId, Guid ratingId);
        Task<IEnumerable<RatingModel>> GetAllFromUser(Guid userId);
        Task<IEnumerable<RatingModel>> GetAllFromQuestion(Guid questionId);
        Task<RatingModel> Add(Guid questionId, Guid userId, CreateRatingModel model);
        Task Update(Guid questionId, Guid ratingId, UpdateRatingModel model);
        Task Delete(Guid questionId, Guid ratingId);
    }
}
