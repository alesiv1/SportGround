using SportGround.BusinessLogic.Models;
using System;
using System.Collections.Generic;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface ICourtWorkingHoursOperations
	{
		CourtWorkingHoursModel GetCourtById(int id);
		List<CourtWorkingHoursModel> GetAll();
		void Create(CourtWorkingHoursModel model);
		void Update(int id, CourtWorkingHoursModel model);
		void Delete(int id);
	}
}
