using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Models.Questions;
using StudentSatisfaction.Business.Surveys.Services.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

//Context --- nu genera Guid la Add!!!!  --> NeverGenerated()

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/survey/{surveyId}/questions")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;


        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }


        [Authorize(Roles = "User, Admin")]
        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] Guid surveyId)
        {
            var questions = await _questionService.Get(surveyId);

            return Ok(questions);
        }

        [Authorize(Roles = "User, Admin")]
        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetById(Guid surveyId, Guid questionId)
        {
            var question = await _questionService.GetById(surveyId, questionId);

            if(question == null)
            {
                return BadRequest();
            }

            return Ok(question);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateQuestionModel model, [FromRoute] Guid surveyId)
        {
            var question = await _questionService.Add(surveyId, model);

            if(question == null)
            {
                return BadRequest();
            }

            return Created(question.Id.ToString(), null);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{questionId}")]
        public async Task<IActionResult> Put([FromRoute] Guid surveyId, [FromRoute] Guid questionId, [FromBody] UpdateQuestionModel model)
        {
            await _questionService.Update(surveyId, questionId, model);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{questionId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid surveyId, [FromRoute] Guid questionId)
        {
            await _questionService.Delete(surveyId, questionId);

            return NoContent();
        }
    }
}
