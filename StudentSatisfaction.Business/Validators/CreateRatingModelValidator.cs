using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentSatisfaction.Business.Surveys.Models.Ratings;

namespace StudentSatisfaction.Business.Validators
{
    public sealed class CreateRatingModelValidator: AbstractValidator<CreateRatingModel>
    {
        public CreateRatingModelValidator()
        {
            RuleFor(x => x.Points)
                .InclusiveBetween(0, 5)
                .WithMessage("Rating points must be in the following interval [0, 5]");
        }
    }
}
