using System;
using System.ComponentModel.DataAnnotations;

namespace SportGround.Web.Models
{
	public class RegistrationModel
	{
		[Required]
		[DataType(DataType.Text)]
		public string FirstName { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string LastName { get; set; }

		[Required]
		[EmailAddress]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		[StringLength(50, MinimumLength = 6)]
		public string Password { get; set; }

		[Required]
		[Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
		[StringLength(50, MinimumLength = 6)]
		public string ConfirmPassword { get; set; }
	}
}