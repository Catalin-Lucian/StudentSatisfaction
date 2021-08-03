using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Models.Ratings;
using StudentSatisfaction.Business.Surveys.Services.Ratings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentSatisfaction.Business.Surveys.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;
        private readonly ISurveyService _surveygService;

        public RatingsController(IRatingService ratingService, ISurveyService surveygService)
        {
            _ratingService = ratingService;
            _surveygService = surveygService;
        }


        [HttpGet("questions/{questionId}/{ratingId}")]
        public async Task<IActionResult> GetRatingFromQuestion([FromRoute] Guid questionId, [FromRoute] Guid ratingId)
        {
            var rating = await _ratingService.GetRating(questionId, ratingId);

            return Ok(rating);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetAllRatingsFromUser([FromRoute] Guid userId)
        {
            var ratings = await _ratingService.GetAllFromUser(userId);

            return Ok(ratings);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetAllRatingsFromQuestion([FromRoute] Guid questionId)
        {
            var ratings = await _ratingService.GetAllFromQuestion(questionId);

            return Ok(ratings);
        }

        [Authorize(Roles = "Admin, UserData")]
        //????????????????????
        [HttpPost("{questionId}/{userId}")]
        public async Task<IActionResult> Post([FromRoute] Guid questionId, [FromRoute] Guid userId, [FromBody] CreateRatingModel model)
        {
            model.QuestionId = questionId;
            model.UserId = userId;

            var rating = await _ratingService.Add(questionId, userId, model);

            return Created(rating.Id.ToString(), null);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut("{questionId}/{ratingId}")]
        public async Task<IActionResult> Put([FromRoute] Guid questionId, [FromRoute] Guid ratingId, [FromBody] UpdateRatingModel model)
        {
            model.QuestionId = questionId;
            await _ratingService.Update(questionId, ratingId, model);

            return NoContent();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("{questionId}/{ratingId}")]
        public async Task<IActionResult> Delete([FromRoute]Guid questionId, [FromRoute] Guid ratingId)
        {
            await _ratingService.Delete(questionId, ratingId);

            return NoContent();
        }

        [HttpGet("surveys/{surveyId}")]
        public IActionResult GetAllRatingsFromSurvey([FromRoute] Guid surveyId)
        {
            var ratings = _ratingService.GetAllFromSurvey(surveyId);

            return Ok(ratings);
        }

        [HttpGet("surveys/{surveyId}/users/{userId}")]
        public IActionResult GetAllRatingsFromSurveyFromCertainUser([FromRoute] Guid surveyId, [FromRoute] Guid userId)
        {
            var ratings = _ratingService.GetUserRatingFromSurvey(surveyId, userId);

            return Ok(ratings);
        }
    }
}
