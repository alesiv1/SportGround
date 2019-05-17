using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using SportGround.BusinessLogic.Validations;
using SportGround.Data.Enums;

namespace SportGround.BusinessLogic.Models
{
	[Validator(typeof(UserValidation))]
	public class UserModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
	}

	public class UserModelWithRole : UserModel
	{
		[Required]
		public UserRole Role { get; set; }
	}

	[Validator(typeof(UserWithPasswordValidation))]
	public class UserModelWithPassword : UserModel
	{
		[DataType(DataType.Password)]
		public string Password { get; set; }
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
		public string ConfirmPassword { get; set; }
	}
}
