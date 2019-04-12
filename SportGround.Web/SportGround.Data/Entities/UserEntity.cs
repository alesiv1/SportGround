using SportGround.Data.Enums;
using System;
using System.Collections.Generic;
using SportGround.Data.Entities;

namespace SportGround.Data.entities
{
    public class UserEntity
    {
	    public int Id { get; set; }
	    public string FirstName { get; set; }
	    public string LastName { get; set; }
	    public string Email { get; set; }
		public UserRole Role { get; set; }
		public string Password { get; set; }
		public string Salt { get; set; }

		public List<CourtBookingEntity> BookingCourts { get; set; }
	}
}
