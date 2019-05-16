using System;
using SportGround.BusinessLogic.Models;
using System.Collections.Generic;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface IBookingService
	{
		CourtBookingModel GetCourtBookingById(long id);
		IReadOnlyList<CourtBookingModel> GetBookingList();
		void Create(CourtBookingModel model);
		void Update(long id, CourtBookingModel model);
		void Delete(long id);
		IReadOnlyList<CourtBookingModel> GetAllUserBooking(int userId);
	}
}
