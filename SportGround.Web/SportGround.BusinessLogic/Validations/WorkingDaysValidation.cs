using System;
using FluentValidation;
using SportGround.BusinessLogic.Models;

namespace SportGround.BusinessLogic.Validation
{
	public class WorkingDaysValidation : AbstractValidator<CourtWorkingDaysModel>
	{
		public WorkingDaysValidation()
		{
			RuleFor(x => x.StartTime).LessThanOrEqualTo(x => x.EndTime).WithMessage("Start time must be less then ent time!");
			RuleFor(x => x.EndTime).GreaterThanOrEqualTo(x => x.StartTime).WithMessage("End time must be greater then start time!");
			RuleFor(x => x.EndTime.Hour).GreaterThan(24);
			RuleFor(x => x.EndTime.Hour).LessThan(0);
			RuleFor(x => x.StartTime.Hour).GreaterThan(24);
			RuleFor(x => x.StartTime.Hour).LessThan(0);
		}
	}
}
