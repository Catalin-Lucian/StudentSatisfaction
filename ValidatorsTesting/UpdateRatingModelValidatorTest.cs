using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using NUnit.Framework;
using StudentSatisfaction.Business.Surveys.Models.Ratings;
using StudentSatisfaction.Business.Validators;

namespace ValidatorsTesting
{
    public class UpdateRatingModelValidatorTest
    {
        private UpdateRatingModelValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new UpdateRatingModelValidator();
        }

        [Test]
        public void Test()
        {
            //Arrange
            var model = new UpdateRatingModel()
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
