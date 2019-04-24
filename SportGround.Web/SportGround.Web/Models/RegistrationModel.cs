using System;
using System.ComponentModel.DataAnnotations;

namespace SportGround.Web.Models
{
	public class RegistrationModel
	{
		[Required]
		[StringLength(50, MinimumLength = 3)]
		public string FirstName { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 3)]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
		public string Email { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 6)]
		public string Password { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
		public string ConfirmPassword { get; set; }
	}
}