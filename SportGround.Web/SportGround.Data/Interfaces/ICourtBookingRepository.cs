using SportGround.Data.Entities;
using System;
using System.Collections.Generic;

namespace SportGround.Data.Interfaces
{
	public interface ICourtBookingRepository
	{
		void Add(DateTimeOffset startDate, DateTimeOffset EndDate, int courtId, int userId);
		void Delete(long id);
		void DeleteRangeByUserId(int userId);
		void DeleteRangeByCourtId(int courtId);
		IReadOnlyList<CourtBookingEntity> GetCourtBookings();
		CourtBookingEntity GetCourtBookingById(long id);
		void Update(long id, DateTimeOffset startDate, DateTimeOffset EndDate);
	}
}
