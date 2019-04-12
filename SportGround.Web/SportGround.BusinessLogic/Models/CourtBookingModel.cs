using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportGround.BusinessLogic.Models
{
	public class BaseCourtBookingModel
	{
		public int Id { get; set; }
		[Required]
		public UserModel User { get; set; }
		[Required]
		public CourtModel Court { get; set; }
	}

	public class CourtBookingModel : BaseCourtBookingModel
	{
		[Required]
		[DataType(DataType.DateTime)]
		public DateTimeOffset Date { get; set; }
		public string DateInString { get; set; }
		public bool IsActive { get; set; }
	}

	public class CreateCourtBookingModel : BaseCourtBookingModel
	{ 
		[Required]
		public List<string> AvailableDate { get; set; }
	}
}
