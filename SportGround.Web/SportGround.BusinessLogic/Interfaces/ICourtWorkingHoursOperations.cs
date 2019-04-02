using SportGround.BusinessLogic.Models;
using System;
using System.Collections.Generic;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface ICourtWorkingHoursOperations
	{
		CourtWorkingHoursModel GetById(int id);
		List<CourtWorkingHoursModel> GetAll();
		void Create(int courtId, CourtWorkingHoursModel model);
		void Update(int id, CourtWorkingHoursModel model);
		void Delete(int id);
		List<CourtWorkingHoursModel> GetAllForCourt(int courtId);
	}
}
