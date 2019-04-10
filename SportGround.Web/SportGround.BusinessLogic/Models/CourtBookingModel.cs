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
	}

	public class CourtBookingModel : BaseCourtBookingModel
	{
		[Required]
		public int CourtId { get; set; }
		[Required]
		[StringLength(50, MinimumLength = 3)]
		public string CourtName { get; set; }
		[Required]
		[DataType(DataType.DateTime)]
		public DateTimeOffset Date { get; set; }
		public string DateInString { get; set; }
		public bool IsActive { get; set; }
	}

	public class CreateCourtBookingModel : BaseCourtBookingModel
	{
		[Required]
		public CourtModel Court { get; set; }
		[Required]
		public List<string> AvailableDate { get; set; }
	}
}
