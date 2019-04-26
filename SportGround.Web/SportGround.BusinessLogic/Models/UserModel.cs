using System;
using System.ComponentModel.DataAnnotations;
using SportGround.Data.Enums;

namespace SportGround.BusinessLogic.Models
{
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

	public class UserModelWithPassword : UserModel
	{
		public string Password { get; set; }
		[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
		public string ConfirmPassword { get; set; }
	}
}
