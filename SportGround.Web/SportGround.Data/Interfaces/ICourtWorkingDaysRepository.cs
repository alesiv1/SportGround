using SportGround.Data.Entities;
using System;
using System.Collections.Generic;

namespace SportGround.Data.Interfaces
{
	public interface ICourtWorkingDaysRepository
	{
		void Add(CourtWorkingDaysEntity workingDays, int courtId);
		void Delete(int id);
		void Delete(CourtWorkingDaysEntity workingDays);
		ICollection<CourtWorkingDaysEntity> GetWorkingDays();
		CourtWorkingDaysEntity GetCourtWorkingDayById(int id);
		void Update(CourtWorkingDaysEntity workingDays);
	}
}
