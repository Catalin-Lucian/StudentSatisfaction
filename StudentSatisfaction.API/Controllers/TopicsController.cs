using Microsoft.AspNetCore.Authorization;
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
    [Route("api/topics")]
    [ApiController]
    public class TopicsController : ControllerBase
    {
        private readonly ITopicsService _topicsService;


        public TopicsController(ITopicsService topicsService)
        {
            _topicsService = topicsService;
        }


        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public IActionResult Get()
        {
            var topics = _topicsService.GetAll();

            return Ok(topics);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("{topicId}")]
        public async Task<IActionResult> Get([FromRoute] Guid topicId)
        {
            var topic = await _topicsService.GetById(topicId);

            if(topic == null)
            {
                return BadRequest();
            }

            return Ok(topic);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateTopicModel model)
        {
            var topic = await _topicsService.Create(model);

            return Created(topic.Id.ToString(), null);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{topicId}")]
        public async Task<IActionResult> Put([FromRoute] Guid topicId, [FromBody] UpdateTopicModel model)
        {
            await _topicsService.Update(topicId, model);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{topicId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid topicId)
        {
            await _topicsService.Delete(topicId);

            return NoContent();
        }
    }
}
