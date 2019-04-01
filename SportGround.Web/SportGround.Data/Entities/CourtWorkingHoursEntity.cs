using SportGround.Data.Enums;
using System;

namespace SportGround.Data.Entities
{
	public class CourtWorkingHoursEntity
	{
		public int Id { get; set; }
		public CourtEntity CourtId { get; set; }
		public DaysOfTheWeek Day { get; set; }
		public DateTimeOffset StartTime { get; set; }
		public DateTimeOffset EndTime { get; set; }
	}
}
