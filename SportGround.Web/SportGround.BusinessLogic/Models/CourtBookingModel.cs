using SportGround.Data.entities;
using SportGround.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportGround.BusinessLogic.Models
{
	public class BaseCourtBookingModel
	{
		[Required]
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
	}

	public class CreateCourtBookingModel : BaseCourtBookingModel
	{
		[Required]
		[DataType(DataType.DateTime)]
		public List<DateTimeOffset> AvailableDate { get; set; }
	}
}
