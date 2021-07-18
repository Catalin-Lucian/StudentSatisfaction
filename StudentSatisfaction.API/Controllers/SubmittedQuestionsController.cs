using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Models.SubmittedQuestions;
using StudentSatisfaction.Business.Surveys.Services.SubmittedQuestions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/submittedQuestions")]
    [ApiController]
    public class SubmittedQuestionsController : ControllerBase
    {
        private readonly ISubmittedQuestionsService _submittedQuestionsService;


        public SubmittedQuestionsController(ISubmittedQuestionsService submittedQuestionsService)
        {
            _submittedQuestionsService = submittedQuestionsService;
        }

        [HttpGet("surveys/{surveyId}")]
        public async Task<IActionResult> GetAllFromSurvey([FromRoute] Guid surveyId)
        {
            var questions = await _submittedQuestionsService.GetAllFromSurvey(surveyId);

            return Ok(questions);
        }

        [HttpGet("{submittedQuestionId}/surveys/{surveyId}")]
        public async Task<IActionResult> GetQuestionFromSurvey([FromRoute] Guid submittedQuestionId, [FromRoute] Guid surveyId)
        {
            var questions = await _submittedQuestionsService.GetQuestionFromSurvey(surveyId, submittedQuestionId);

            return Ok(questions);
        }


        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetAllFromUser([FromRoute] Guid userId)
        {
            var questions = await _submittedQuestionsService.GetAllFromUser(userId);

            return Ok(questions);
        }

        [HttpGet("{submittedQuestionId}/users/{userId}")]
        public async Task<IActionResult> GetAllFromUser([FromRoute] Guid submittedQuestionId, [FromRoute] Guid userId)
        {
            var questions = await _submittedQuestionsService.GetQuestionFromUser(userId, submittedQuestionId);

            return Ok(questions);
        }


        //??????????
        [HttpPost("user/{userId}/survey/{surveyId}")]
        public async Task<IActionResult> Post([FromRoute] Guid surveyId, [FromRoute] Guid userId, [FromBody] CreateSubmittedQuestionModel model)
        {
            model.SurveyId = surveyId;
            model.UserId = userId;

            var submittedQuestion = await _submittedQuestionsService.Add(surveyId, userId, model);

            return Created(submittedQuestion.Id.ToString(), null);
        }

        [HttpPut("{submittedQuestionId}/survey/{surveyId}")]
        public async Task<IActionResult> Put([FromRoute] Guid surveyId, [FromRoute] Guid submittedQuestionId, [FromBody] UpdateSubmittedQuestionModel model)
        {
            await _submittedQuestionsService.Update(surveyId, submittedQuestionId, model);

            return NoContent();
        }

        [HttpDelete("{submittedQuestionId}/survey/{surveyId}")]
        public async Task<IActionResult> DeleteFromSurvey([FromRoute] Guid surveyId, [FromRoute] Guid submittedQuestionId)
        {
            await _submittedQuestionsService.DeleteFromSurvey(surveyId, submittedQuestionId);

            return NoContent();
        }

        [HttpDelete("{submittedQuestionId}/user/{userId}")]
        public async Task<IActionResult> DeleteFromUser([FromRoute] Guid userId, [FromRoute] Guid submittedQuestionId)
        {
            await _submittedQuestionsService.DeleteFromUser(userId, submittedQuestionId);

            return NoContent();
        }
    }
}
