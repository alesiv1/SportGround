using System;
using System.ComponentModel.DataAnnotations;

namespace SportGround.Web.Models
{
	public class LogInModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		[DataType(DataType.Password)]
		[StringLength(50, MinimumLength = 6)]
		public string Password { get; set; }
	}
}