using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Models.Notifications;
using StudentSatisfaction.Business.Surveys.Models.Users;
using StudentSatisfaction.Business.Surveys.Services.Notifications;
using StudentSatisfaction.Business.Surveys.Services.Questions;
using StudentSatisfaction.Business.Surveys.Services.Users;
using StudentSatisfaction.Entities.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace StudentSatisfaction.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly INotificationsService _notificationsService;
        private readonly IQuestionService _questionService;


        public UsersController(IUsersService usersService, INotificationsService notificationsService, IQuestionService questionService)
        {
            _usersService = usersService;
            _notificationsService = notificationsService;
            _questionService = questionService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _usersService.GetAllUsers();

            return Ok(users);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById([FromRoute] Guid userId)
        {
            var user = await _usersService.GetUserById(userId);

            if (user == null)
            {
                return BadRequest();
            }

            return Ok(user);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateUserModel model)
        {
            var user = await _usersService.Create(model);

            return Created(user.Id.ToString(), null);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpPut("{userId}")]
        public async Task<IActionResult> Put(Guid userId, [FromBody] UpdateUserModel model)
        {
            await _usersService.Update(userId, model);

            return NoContent();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("deleteUserCredentials/{userId}")]
        public async Task<IActionResult> DeleteFromAspNetUsers([FromRoute] Guid userId)
        {
            await _usersService.DeleteCredentials(userId);

            return NoContent();
        }

        [Authorize(Roles = "Admin, User")]
        [HttpDelete("deleteUser/{userId}")]
        public async Task<IActionResult> Delete([FromRoute] Guid userId)
        {
            await _usersService.Delete(userId);

            return NoContent();
        }




        //Manage Notifications from UsersData
        [Authorize(Roles = "Admin, User")]
        [HttpGet("{userId}/notifications")]
        public async Task<IActionResult> GetAllNotificationsFromUser([FromRoute] Guid userId)
        {
            var notification = await _notificationsService.GetAll(userId);

            return Ok(notification);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("{userId}/notifications/{notificationId}")]
        public async Task<IActionResult> Get([FromRoute] Guid userId, [FromRoute] Guid notificationId)
        {
            var notification = await _notificationsService.GetById(userId, notificationId);

            return Ok(notification);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{userId}/notifications")]
        public async Task<IActionResult> CreateNotificationForUser([FromRoute] Guid userId, [FromBody] CreateNotificationModel model)
        {
            var user = await _notificationsService.Add(userId, model);

            return Created(user.Id.ToString(), null);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{userId}/notifications/{notificationId}")]
        public async Task<IActionResult> CreateNotificationForUser([FromRoute] Guid userId, [FromRoute] Guid notificationId, [FromBody] UpdateNotificationModel model)
        {
            await _notificationsService.Update(userId, notificationId, model);

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId}/notifications/{notificationId}")]
        public async Task<IActionResult> DeleteNotificationFromUser([FromRoute] Guid userId, [FromRoute] Guid notificationId)
        {
            await _notificationsService.Delete(userId, notificationId);

            return NoContent();
        }


        //Manage answered/not answered surveys from UserData
        [Authorize(Roles = "Admin, User")]
        [HttpGet("{userId}/answeredSurveys")]
        public async Task<IActionResult> GetAnsweredSurveys([FromRoute] Guid userId)
        {
            var surveys = await _usersService.GetAnsweredSurveys(userId);

            if (surveys == null)
            {
                return BadRequest();
            }

            return Ok(surveys);
        }

        [Authorize(Roles = "Admin, User")]
        [HttpGet("{userId}/notAnsweredSurveys")]
        public async Task<IActionResult> GetNotAnsweredSurveys([FromRoute] Guid userId)
        {
            var surveys = await _usersService.GetNotAnsweredSurveys(userId);

            if (surveys == null)
            {
                return BadRequest();
            }

            return Ok(surveys);
        }


        //Manage answered/not answered questions from a survey
        [Authorize(Roles = "Admin, User")]
        [HttpGet("{userId}/answeredQuestions")]
        public async Task<IActionResult> GetAnsweredQuestions([FromRoute] Guid userId)
        {
            var answeredQuestions = await _usersService.GetAnsweredQuestions(userId);

            if (answeredQuestions == null)
            {
                return BadRequest();
            }

            return Ok(answeredQuestions);
        }
    }
}
