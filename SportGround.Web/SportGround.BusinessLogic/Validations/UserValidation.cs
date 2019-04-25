using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation;
using SportGround.BusinessLogic.Models;

namespace SportGround.BusinessLogic.Validations
{
	public class UserValidation : AbstractValidator<UserModel>
	{
		public UserValidation()
		{
			RuleFor(x => x.FirstName).Length(3, 50).WithMessage("First name must have more 3 and less 50 characters!");
			RuleFor(x => x.LastName).Length(3, 50).WithMessage("Lust name must have more 3 and less 50 characters!");
			RuleFor(x => x.Email).EmailAddress().WithMessage("Email is incorrect!");
		}
	}

	public class UserWithRoleValidation : AbstractValidator<UserModelWithPassword>
	{
		public UserWithRoleValidation()
		{
			RuleFor(x => x.FirstName).Length(3, 50).WithMessage("First name must have more 3 and less 50 characters!");
			RuleFor(x => x.LastName).Length(3, 50).WithMessage("Lust name must have more 3 and less 50 characters!");
			RuleFor(x => x.Email).EmailAddress().WithMessage("Email is incorrect!");
			RuleFor(x => x.Password).Length(6, 50).WithMessage("Password must have more 6 and less 50 characters!");
			RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Password and confirm password must be same!");
		}
	}
}
