using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using StudentSatisfaction.Business.Surveys.Models;
using StudentSatisfaction.Business.Surveys.Models.Users;

namespace StudentSatisfaction.Business.Validators
{
    public sealed class CreateUserValidator : AbstractValidator<CreateUserModel>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Type)
                .Must(type => type == "User" | type == "Admin")
                .WithMessage("User role must be 'User' or 'Admin'");

            RuleFor(x => x.Email)
                .EmailAddress()
                .WithMessage("The email address is not valid!");
        }
    }
}
