using System;
using SportGround.BusinessLogic.Models;
using System.Collections.Generic;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface IBookingService
	{
		CourtBookingModel GetCourtBookingById(int id);
		List<CourtBookingModel> GetBookingList();
		void Create(CourtBookingModel model);
		void Update(int id, CourtBookingModel model);
		void Delete(int id);
		List<DateTimeOffset> GetAllAvailableDataTime(int courtId);
		List<CourtBookingModel> GetAllUserBooking(int userId);
	}
}
