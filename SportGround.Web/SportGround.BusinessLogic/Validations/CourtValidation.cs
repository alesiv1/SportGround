using System;
using FluentValidation;
using SportGround.BusinessLogic.Models;

namespace SportGround.BusinessLogic.Validations
{
	public class CourtValidation : AbstractValidator<CourtModel>
	{
		public CourtValidation()
		{
			RuleFor(x => x.Name).Length(5, 50).WithMessage("Name must have more 5 and less 50 characters!");
		}
	}
}
