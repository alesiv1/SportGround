using SportGround.Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportGround.BusinessLogic.Models
{
	public class CourtWorkingDaysModel
	{
		public int Id { get; set; }
		[Required]
		public CourtModel Court { get; set; }
		[Required]
		public DaysOfTheWeek Day { get; set; }
		[Required]
		public List<DaysOfTheWeek> AvailableDays { get; set; }
		[Required]
		[DataType(DataType.DateTime)]
		public DateTimeOffset StartTime { get; set; }
		[Required]
		[DataType(DataType.DateTime)]
		public DateTimeOffset EndTime { get; set; }
	}
}
