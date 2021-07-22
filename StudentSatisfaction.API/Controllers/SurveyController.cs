using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Business.Surveys.Models.Topics;
using StudentSatisfaction.Business.Surveys.Models.Users;
using StudentSatisfaction.Business.Surveys.Services;
using StudentSatisfaction.Business.Surveys.Services.Topics;
using StudentSatisfaction.Business.Surveys.Services.Users;
using StudentSatisfaction.Persistence;
using System;
using System.Threading.Tasks;

//CreateSurveyModel --> genereaza in Mapper un ID nou
//SurveyModel ---> are ID-ul deja setat
namespace StudentSatisfaction.API.Controllers
{
    [Route("api/survey")]
    [ApiController]
    public class SurveyController: ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly ITopicsService _topicsService;
        private readonly IUsersService _usersService;


        public SurveyController(ISurveyService surveyService, ITopicsService topicsService, IUsersService usersService)
        {
            _surveyService = surveyService;
            _topicsService = topicsService;
            _usersService = usersService;
        }

        [HttpGet("{surveyId}")]
        public async Task<IActionResult> GetSurvey([FromRoute] Guid surveyId)
        {
            var survey = await _surveyService.GetById(surveyId);

            if(survey == null)
            {
                return BadRequest();
            }

            return Ok(survey);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSurvey([FromBody] CreateSurveyModel model)
        {
            var result = await _surveyService.Create(model);

            return Created(result.Id.ToString(), null);
        }

        //[Authorize(Roles = "User")]
        [Authorize(Roles = "Admin, User")]
        [HttpGet]
        public IActionResult GetAllSurveys()
        {
            var trips = _surveyService.GetAll();

            return Ok(trips);
        }

        [HttpDelete("{surveyId}")]
        public async Task<IActionResult> DeleteSurvey([FromRoute] Guid surveyId)
        {
            await _surveyService.Delete(surveyId);

            return NoContent();
        }

        //primeste ca parametru SurveyModel pt. ca nu vrem sa se genereze un nou PK pt. Survey-ul pe care vrem sa il updatam
        [HttpPut("{surveyId}")]
        public async Task<IActionResult> UpdateSurvey([FromRoute] Guid surveyId, [FromBody] UpdateSurveyModel model)
        {
            await _surveyService.Update(surveyId, model);

            return NoContent();
        }


        //Manage topics from survey

        //delete a certain topic from a certain survey
        [HttpDelete("{surveyId}/deleteCertain" +
            "Topic/{topicId}")]
        public async Task<IActionResult> DeleteTopicFromSurvey([FromRoute] Guid surveyId, [FromRoute] Guid topicId)
        {
            await _topicsService.DeleteTopicFromSurvey(surveyId, topicId);

            return NoContent();
        }


        //functia AddTopicToSurvey e overloaded in TopicRepository --> adauga dupa id sau dupa model
        [HttpPost("{surveyId}/addTopic/{topicId}")]
        public async Task<IActionResult> AddTopicToSurvey([FromRoute] Guid surveyId, [FromRoute] Guid topicId/*[FromBody] CreateTopicModel model*/)
        {
            //var topic = await _topicsService.AddTopicToSurvey(surveyId, model);
            var topic = await _topicsService.AddTopicToSurvey(surveyId, topicId);

            if (topic == null)
            {
                return BadRequest();
            }

            return Created(topic.Id.ToString(), null);
        }

        [HttpGet("{surveyId}/topics")]
        public async Task<IActionResult> GetAllTopicsFromSurvey([FromRoute] Guid surveyId)
        {
            var topics = await _topicsService.GetAllTopicsFromSurvey(surveyId);

            return Ok(topics);
        }


        //Manage the Users that completed a Survey

        //get all users that completed a certain survey
        [HttpGet("{surveyId}/users")]
        public async Task<IActionResult> GetAllUsers([FromRoute] Guid surveyId)
        {
            var users = await _usersService.GetAllUsersFromSurvey(surveyId);

            return Ok(users);
        }

        //get a certain user that completed a certain survey
        [HttpGet("{surveyId}/users/{userId}")]
        public async Task<IActionResult> Get([FromRoute] Guid surveyId, [FromRoute] Guid userId)
        {
            var user = await _usersService.GetUserFromSurvey(surveyId, userId);

            return Ok(user);
        }

        //Mark a user as he never completed the survey
        [HttpDelete("{surveyId}/users/{userId}")]
        public async Task<IActionResult> DeleteUserFromSurvey([FromRoute] Guid surveyId, [FromRoute] Guid userId)
        {
            await _usersService.DeleteUserFromSurvey(surveyId, userId);

            return NoContent();
        }

        //user completed a survey
        [HttpPost("{surveyId}/addUser/{userId}")]
        public async Task<IActionResult> AddUserToSurvey([FromRoute] Guid surveyId, [FromRoute] Guid userId)
        {
            var user = await _usersService.AddUserToSurvey(surveyId, userId);

            if (user == null)
            {
                return BadRequest();
            }

            return Created(user.Id.ToString(), null);
        }
    }
}
