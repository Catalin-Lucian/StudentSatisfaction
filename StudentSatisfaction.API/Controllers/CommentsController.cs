using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Models.Comments;
using StudentSatisfaction.Business.Surveys.Services;
using StudentSatisfaction.Business.Surveys.Services.Comments;
using StudentSatisfaction.Business.Surveys.Services.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace StudentSatisfaction.API.Controllers
{
    [Route("api/comments")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentsService _commentsService;


        public CommentsController(ICommentsService commentsService)
        {
            _commentsService = commentsService;
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("survey/{surveyId}")]
        public async Task<IActionResult> GetFromSurvey([FromRoute] Guid surveyId)
        {
            var comments = await _commentsService.GetCommentsFromSurvey(surveyId);

            return Ok(comments);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetFromUser([FromRoute] Guid userId)
        {
            var comments = await _commentsService.GetCommentsFromUser(userId);

            return Ok(comments);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("{commentId}")]
        public async Task<IActionResult> Get([FromRoute] Guid commentId)
        {
            var comment = await _commentsService.GetCommentById(commentId);

            return Ok(comment);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPost("{userId}/{surveyId}")]
        public async Task<IActionResult> Post([FromRoute] Guid userId, [FromRoute] Guid surveyId, [FromBody] CreateCommentModel model)
        {
            var comment = await _commentsService.Add(surveyId, userId, model);

            return Created(comment.Id.ToString(), null);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpPut("{commentId}")]
        public async Task<IActionResult> Put([FromRoute] Guid commentId, [FromBody] UpdateCommentModel model)
        {
            await _commentsService.Update(commentId, model);

            return NoContent();
        }

        [Authorize(Roles = "User, Admin")]
        [HttpDelete("{commentId}/survey/{surveyId}/deleteFromSurvey")]
        public async Task<IActionResult> DeleteFromSurvey([FromRoute] Guid surveyId, [FromRoute] Guid commentId)
        {
            await _commentsService.DeleteCommentFromSurvey(surveyId, commentId);

            return NoContent();
        }

        [Authorize(Roles = "User, Admin")]
        [HttpDelete("{commentId}/user/{userId}/deleteFromUser")]
        public async Task<IActionResult> DeleteFromUser([FromRoute] Guid userId, [FromRoute] Guid commentId)
        {
            await _commentsService.DeleteCommentFromUser(userId, commentId);

            return NoContent();
        }
    }
}
