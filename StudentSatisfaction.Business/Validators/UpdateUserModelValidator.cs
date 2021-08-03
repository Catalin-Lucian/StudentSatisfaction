using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using StudentSatisfaction.Business.Surveys.Models.Users;

namespace StudentSatisfaction.Business.Validators
{
    public sealed class UpdateUserModelValidator: AbstractValidator<UpdateUserModel>
    {
        public UpdateUserModelValidator()
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
