using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using StudentSatisfaction.Business.Surveys.Models.Ratings;

namespace StudentSatisfaction.Business.Validators
{
    public sealed class UpdateRatingModelValidator: AbstractValidator<UpdateRatingModel>
    {
        public UpdateRatingModelValidator()
        {
            RuleFor(x => x.Points)
                .InclusiveBetween(0, 5)
                .WithMessage("Rating points must be in the following interval [0, 5]");
        }
    }
}
