using System;
using SportGround.Data.entities;

namespace SportGround.Data.Entities
{
	public class CourtBookingEntity
	{
		public int Id { get; set; }
		public UserEntity User { get; set; }
		public CourtEntity Court { get; set; }
		public DateTimeOffset Date { get; set; }
	}
}
