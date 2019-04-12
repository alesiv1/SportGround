using SportGround.Data.Context;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SportGround.Data.Repositories
{
	public class CourtRepository : ICourtRepository
	{
		private readonly DataContext _context;

		public CourtRepository(DataContext context)
		{
			this._context = context;
		}

		public void Add(CourtEntity court)
		{
			_context.Courts.Add(court);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var court = _context
				.Courts
				.Include(wd => wd.WorkingDays)
				.Include(booking => booking.Bookings)
				.FirstOrDefault(x => x.Id == id);
			_context.Courts.Remove(court);
			_context.SaveChanges();
		}

		public void Delete(CourtEntity court)
		{
			_context.Courts.Remove(court);
			_context.SaveChanges();

		}

		public ICollection<CourtEntity> GetCourts()
		{
			return _context.Courts.Include(court => court.WorkingDays).ToList();
		}

		public CourtEntity GetCourtWithWorkingDays(int id)
		{
			return _context.Courts
				.Include(court => court.WorkingDays)
				.FirstOrDefault(court => court.Id == id);
		}

		public CourtEntity GetCourtById(int id)
		{
			return _context.Courts.Find(id);
		}

		public void Update(CourtEntity entity)
		{
			this._context.SaveChanges();
		}

		public bool CourtExists(string name)
		{
			return _context.Courts.Any(court => court.Name == name);
		}
	}
}
