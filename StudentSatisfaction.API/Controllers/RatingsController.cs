﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentSatisfaction.Business.Surveys.Models.Ratings;
using StudentSatisfaction.Business.Surveys.Services.Ratings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StudentSatisfaction.API.Controllers
{
    [Route("api/ratings")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingsController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetAllRatingsFromUser([FromRoute] Guid userId)
        {
            var ratings = await _ratingService.GetAllFromUser(userId);

            return Ok(ratings);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("{questionId}")]
        public async Task<IActionResult> GetAllRatingsFromQuestion([FromRoute] Guid questionId)
        {
            var ratings = await _ratingService.GetAllFromQuestion(questionId);

            return Ok(ratings);
        }

        [Authorize(Roles = "User,Admin")]
        //????????????????????
        [HttpPost("{questionId}/{userId}")]
        public async Task<IActionResult> Post([FromRoute] Guid questionId, [FromRoute] Guid userId, [FromBody] CreateRatingModel model)
        {
            model.QuestionId = questionId;
            model.UserId = userId;

            var rating = await _ratingService.Add(questionId, userId, model);

            return Created(rating.Id.ToString(), null);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPut("{questionId}/{ratingId}")]
        public async Task<IActionResult> Put([FromRoute] Guid questionId, [FromRoute] Guid ratingId, [FromBody] UpdateRatingModel model)
        {
            model.QuestionId = questionId;
            await _ratingService.Update(questionId, ratingId, model);

            return NoContent();
        }

        [Authorize(Roles = "User,Admin")]
        [HttpDelete("{questionId}/{ratingId}")]
        public async Task<IActionResult> Delete([FromRoute]Guid questionId, [FromRoute] Guid ratingId)
        {
            await _ratingService.Delete(questionId, ratingId);

            return NoContent();
        }
    }
}
