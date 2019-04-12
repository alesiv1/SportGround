using SportGround.Data.Enums;
using System;

namespace SportGround.Data.Entities
{
	public class CourtWorkingDaysEntity
	{
		public int Id { get; set; }
		public CourtEntity Court { get; set; }
		public DaysOfTheWeek Day { get; set; }
		public DateTimeOffset StartTimeOfDay { get; set; }
		public DateTimeOffset EndTimeOfDay { get; set; }
	}
}
