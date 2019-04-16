using SportGround.Data.Entities;
using System;
using System.Collections.Generic;

namespace SportGround.Data.Interfaces
{
	public interface ICourtBookingRepository
	{
		void Add(DateTimeOffset date, int courtId, int userId);
		void Delete(int id);
		void DeleteRangeByUserId(int userId);
		void DeleteRangeByCourtId(int courtId);
		IReadOnlyList<CourtBookingEntity> GetCourtBookings();
		CourtBookingEntity GetCourtBookingById(int id);
		void Update(int id, DateTimeOffset date);
	}
}
