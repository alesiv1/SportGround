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
			RuleFor(x => x.Court).NotNull().WithMessage("Court can't be null or empty!");
			RuleFor(x => x.Day).NotNull().WithMessage("Day is required!");
			RuleFor(x => x.EndTime).NotNull().WithMessage("End time is required!");
			RuleFor(x => x.StartTime).NotNull().WithMessage("Start time is required!");
		}
	}
}
