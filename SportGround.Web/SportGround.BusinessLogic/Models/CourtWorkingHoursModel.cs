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
		public CourtEntity CourtId { get; set; }

		[Required]
		public DaysOfTheWeek Day { get; set; }

		[Required]
		public DateTimeOffset StartTime { get; set; }

		[Required]
		public DateTimeOffset EndTime { get; set; }
	}
}
