using System;
using SportGround.Data.Enums;
using SportGround.Data.Entities;
using System.Collections.Generic;

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

		public virtual List<CourtBookingEntity> BookingCourts { get; set; }
	}
}
