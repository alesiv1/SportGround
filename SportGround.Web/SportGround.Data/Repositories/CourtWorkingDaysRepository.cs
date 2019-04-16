using SportGround.Data.Context;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.Data.Enums;

namespace SportGround.Data.Repositories
{
	public class CourtWorkingDaysRepository : ICourtWorkingDaysRepository
	{
		private readonly DataContext _context;

		public CourtWorkingDaysRepository(DataContext context)
		{
			this._context = context;
		}

		public void Add(DaysOfTheWeek day, DateTimeOffset startTime, DateTimeOffset endTime, int courtId)
		{
			var court = _context.Courts.Find(courtId);
			CourtWorkingDaysEntity workingDay = new CourtWorkingDaysEntity()
			{
				Day = day,
				StartTimeOfDay = startTime,
				EndTimeOfDay = endTime,
				Court = court
			};
			_context.CourtWorkingDays.Add(workingDay);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var workingDays = _context.CourtWorkingDays.Find(id);
			_context.CourtWorkingDays.Remove(workingDays);
			_context.SaveChanges();
		}

		public void DeleteRangeByCourtId(int courtId)
		{
			var workingDays = _context.CourtWorkingDays.Where(wd => wd.Court.Id == courtId);
			_context.CourtWorkingDays.RemoveRange(workingDays);
			_context.SaveChanges();
		}

		public IReadOnlyList<CourtWorkingDaysEntity> GetWorkingDays()
		{
			return _context.CourtWorkingDays.ToList();
		}

		public CourtWorkingDaysEntity GetCourtWorkingDayById(int id)
		{
			return _context.CourtWorkingDays.Find(id);
		}

		public void Update(int id, DaysOfTheWeek day, DateTimeOffset startTime, DateTimeOffset endTime)
		{
			var workingDay = _context.CourtWorkingDays.Find(id);
			workingDay.Day = day;
			workingDay.StartTimeOfDay = startTime;
			workingDay.EndTimeOfDay = endTime;
			_context.SaveChanges();
		}
	}
}
