using SportGround.BusinessLogic.Models;
using System;
using System.Collections.Generic;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface ICourtWorkingHoursOperations
	{
		CourtWorkingHoursModel GetCourtWorkingHoursById(int id);
		List<CourtWorkingHoursModel> GetAll();
		void Create(int courtId, CourtWorkingHoursModel model);
		void Update(int id, CourtWorkingHoursModel model);
		void Delete(int id);
		List<CourtWorkingHoursModel> GetAllHoursForCourt(int courtId);
	}
}
