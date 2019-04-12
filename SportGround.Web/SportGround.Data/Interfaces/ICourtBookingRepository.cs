using SportGround.Data.Entities;
using System;
using System.Collections.Generic;

namespace SportGround.Data.Interfaces
{
	public interface ICourtBookingRepository
	{
		void Add(CourtBookingEntity booking, int courtId, int userId);
		void Delete(int id);
		void Delete(CourtBookingEntity booking);
		ICollection<CourtBookingEntity> GetCourtBookings();
		ICollection<CourtBookingEntity> GetCourtBookingWithCourtAndUser();
		CourtBookingEntity GetCourtBookingById(int id);
		void Update(CourtBookingEntity booking);
	}
}
