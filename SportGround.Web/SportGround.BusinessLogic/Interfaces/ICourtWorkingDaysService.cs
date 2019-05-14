using SportGround.BusinessLogic.Models;
using System;
using System.Collections.Generic;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface ICourtWorkingDaysService
	{
		CourtWorkingDaysModel GetWorkingDay(int id);
		List<CourtWorkingDaysModel> GetWorkingDaysList();
		void Create(int courtId, CourtWorkingDaysModel model);
		void Update(int id, CourtWorkingDaysModel model);
		void DeleteWorkingDays(int id);
		List<CourtWorkingDaysModel> GetWorkingDaysForCourt(int courtId);
	}
}
