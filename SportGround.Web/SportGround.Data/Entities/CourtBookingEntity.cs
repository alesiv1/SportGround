using System;
using SportGround.Data.entities;

namespace SportGround.Data.Entities
{
	public class CourtBookingEntity
	{
		public long Id { get; set; }
		public DateTimeOffset StartDate { get; set; }
		public DateTimeOffset EndDate { get; set; }

		public virtual UserEntity User { get; set; }
		public virtual CourtEntity Court { get; set; }
	}
}
