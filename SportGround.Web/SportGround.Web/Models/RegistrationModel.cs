using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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
		[DataType(DataType.Text)]
		public string Login { get; set; }

		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		[Required]
		[DataType(DataType.Text)]
		public string Country { get; set; }
	}
}