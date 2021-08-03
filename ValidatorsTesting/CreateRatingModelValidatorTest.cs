using System;
using FluentValidation.TestHelper;
using NUnit.Framework;
using StudentSatisfaction.Business.Surveys.Models.Ratings;
using StudentSatisfaction.Business.Validators;

namespace ValidatorsTesting
{
    public class CreateRatingModelValidatorTest
    {
        private CreateRatingModelValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateRatingModelValidator();
        }

        [Test]
        public void Test()
        {
            //Arrange
            var model = new CreateRatingModel()
            {
                UserId = Guid.NewGuid(),
                Answear = "some answer",
                Points = -1,
                QuestionId = Guid.NewGuid()
            };

            //Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Points);
        }
    }
}