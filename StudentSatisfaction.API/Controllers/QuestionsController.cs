using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Models.Questions;
using StudentSatisfaction.Business.Surveys.Services.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


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

        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] Guid surveyId)
        {
            var questions = await _questionService.Get(surveyId);

            return Ok(questions);
        }

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


        //????????
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateQuestionModel model, [FromRoute] Guid surveyId)
        {
            var question = await _questionService.Add(surveyId, model);

            return Created(question.Id.ToString(), null);
        }

        //// PUT api/<QuestionsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<QuestionsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
