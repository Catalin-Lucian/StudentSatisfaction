using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Models.Topics;
using StudentSatisfaction.Business.Surveys.Services.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/survey/{surveyId}/topics")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicsService _topicsService;


        public TopicsController(ITopicsService topicsService)
        {
            _topicsService = topicsService;
        }


        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] Guid surveyId)
        {
            var topics = await _topicsService.Get(surveyId);

            return Ok(topics);
        }

        //// GET api/<TopicsController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //adaug un Topic nou
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] CreateTopicModel model)
        //{
        //    var topic = await _topicsService.CreateNewTopic(model);

        //    if (topic == null)
        //    {
        //        return BadRequest();
        //    }

        //    return Created(topic.Id.ToString(), null);
        //}

        //adaug un topic in Survey
        [HttpPost]
        public async Task<IActionResult> Post([FromRoute] Guid surveyId, [FromBody] CreateTopicModel model)
        {

            var topic = await _topicsService.AddTopicToSurvey(surveyId, model);

            if (topic == null)
            {
                return BadRequest();
            }

            return Created(topic.Id.ToString(), null);
        }

        //// PUT api/<TopicsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}


        //delete a certain topic from a certain survey
        // DELETE api/<TopicsController>/5
        [HttpDelete("{topicId}")]
        public async Task<IActionResult> DeleteFromSurvey([FromRoute] Guid surveyId, [FromRoute] Guid topicId)
        {
            await _topicsService.Delete(surveyId, topicId);

            return NoContent();
        }
    }
}
