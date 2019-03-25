using System;
using System.ComponentModel.DataAnnotations;

namespace SportGround.BusinessLogic.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Role { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
	}

	public class UserRegistrationModel : UserModel
	{
		[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
		public string ConfirmPassword { get; set; }
	}
}
