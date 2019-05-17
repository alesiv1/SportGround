using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportGround.BusinessLogic.Models
{
	public class BaseCourtBookingModel
	{
		public long Id { get; set; }
		[Required]
		public UserModel User { get; set; }
		[Required]
		public CourtModel Court { get; set; }
	}

	public class CourtBookingModel : BaseCourtBookingModel
	{
		[Required]
		[DataType(DataType.DateTime)]
		public DateTimeOffset StartDate { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTimeOffset EndDate { get; set; }

		public string DateInString { get; set; }
		public bool IsActive { get; set; }
	}
}
