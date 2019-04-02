using SportGround.Data.Entities;
using SportGround.Data.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace SportGround.BusinessLogic.Models
{
	public class CourtWorkingHoursModel
	{
		[Required]
		public int Id { get; set; }

		[Required]
		public CourtModel Court { get; set; }

		[Required]
		public DaysOfTheWeek Day { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTimeOffset StartTime { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTimeOffset EndTime { get; set; }
	}
}
