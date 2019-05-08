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
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[StringLength(50, MinimumLength = 6)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
		public string ConfirmPassword { get; set; }
	}
}