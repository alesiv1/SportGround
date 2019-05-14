using SportGround.Data.Entities;
using SportGround.Data.Enums;
using System;
using System.Collections.Generic;

namespace SportGround.Data.Interfaces
{
	public interface ICourtWorkingDaysRepository
	{
		void Add(DaysOfTheWeek day, DateTimeOffset startTime, DateTimeOffset endTime, int courtId);
		void Delete(int id);
		void DeleteRangeByCourtId(int courtId);
		IReadOnlyList<CourtWorkingDaysEntity> GetWorkingDays();
		CourtWorkingDaysEntity GetWorkingDayById(int id);
		void Update(int id, DaysOfTheWeek day, DateTimeOffset startTime, DateTimeOffset endTime);
	}
}
