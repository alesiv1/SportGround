using System;
using SportGround.BusinessLogic.Models;
using System.Collections.Generic;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface ICourtBookingOperations
	{
		CourtBookingModel GetById(int id);
		List<CourtBookingModel> GetAll();
		List<CourtBookingModel> GetAllForUser(int userId);
		void Create(CourtBookingModel model);
		void Update(int id, CourtBookingModel model);
		void Delete(int id);
		List<DateTimeOffset> GetAllAvailableDataTime(int courtId);
	}
}
