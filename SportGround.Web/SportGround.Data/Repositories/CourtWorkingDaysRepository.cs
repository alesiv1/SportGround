using SportGround.Data.Context;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SportGround.Data.Repositories
{
	public class CourtWorkingDaysRepository : ICourtWorkingDaysRepository
	{
		private readonly DataContext _context;

		public CourtWorkingDaysRepository(DataContext context)
		{
			this._context = context;
		}

		public void Add(CourtWorkingDaysEntity workingDays, int courtId)
		{
			var court = _context.Courts.Find(courtId);
			workingDays.Court = court;
			_context.CourtWorkingDays.Add(workingDays);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var workingDays = _context.CourtWorkingDays.Find(id);
			_context.CourtWorkingDays.Remove(workingDays);
			_context.SaveChanges();
		}

		public void Delete(CourtWorkingDaysEntity workingDays)
		{
			_context.CourtWorkingDays.Remove(workingDays);
			_context.SaveChanges();
		}

		public ICollection<CourtWorkingDaysEntity> GetWorkingDays()
		{
			return _context.CourtWorkingDays.ToList();
		}

		public CourtWorkingDaysEntity GetCourtWorkingDayById(int id)
		{
			return _context.CourtWorkingDays
				.Include(court => court.Court)
				.FirstOrDefault(workingDay => workingDay.Id == id);
		}

		public void Update(CourtWorkingDaysEntity workingDays)
		{
			_context.SaveChanges();
		}
	}
}
