using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Configuration;

namespace SportGround.Web.Models
{
	public class LogInModel
	{
		[Required]
		[EmailAddress]
		[RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
		public string Email { get; set; }

		[Required]
		[StringLength(50, MinimumLength = 6)]
		public string Password { get; set; }
	}
}