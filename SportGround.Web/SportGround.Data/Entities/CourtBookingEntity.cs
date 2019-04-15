using System;
using SportGround.Data.entities;

namespace SportGround.Data.Entities
{
	public class CourtBookingEntity
	{
		public int Id { get; set; }
		public DateTimeOffset BookingDate { get; set; }

		public virtual UserEntity User { get; set; }
		public virtual CourtEntity Court { get; set; }
	}
}
