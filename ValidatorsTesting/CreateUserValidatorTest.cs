using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.TestHelper;
using NUnit.Framework;
using StudentSatisfaction.Business.Surveys.Models.Ratings;
using StudentSatisfaction.Business.Surveys.Models.Users;
using StudentSatisfaction.Business.Validators;

namespace ValidatorsTesting
{
    public sealed class CreateUserValidatorTest
    {
        private CreateUserValidator _validator;

        [SetUp]
        public void Setup()
        {
            _validator = new CreateUserValidator();
        }

        [Test]
        public void Test()
        {
            //Arrange
            var model = new CreateUserModel()
            {
                Name = "name",
                BirthDate = DateTime.Now.AddYears(20),
                Email = "mail",
                Password = "password",
                FacultyName = "AC",
                Type = "some type",
                Username = "username"
            };

            //Act
            var result = _validator.TestValidate(model);

            //Assert
            result.ShouldHaveValidationErrorFor(x => x.Type);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }
    }
}
