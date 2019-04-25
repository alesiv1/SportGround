using System;
using System.ComponentModel.DataAnnotations;
using SportGround.Data.Enums;

namespace SportGround.BusinessLogic.Models
{
	public class UserModel
	{
		[Required]
		public int Id { get; set; }
		[Required]
		public string FirstName { get; set; }
		[Required]
		public string LastName { get; set; }
		[Required]
		public string Email { get; set; }
	}

	public class UserModelWithRole : UserModel
	{
		[Required]
		public UserRole Role { get; set; }
	}

	public class UserModelWithPassword : UserModel
	{
		[Required]
		public string Password { get; set; }
		[Required]
		[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
		public string ConfirmPassword { get; set; }
	}
}
