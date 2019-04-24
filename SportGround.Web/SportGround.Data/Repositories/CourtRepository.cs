using System;
using SportGround.Data.Context;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;
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

		public void Add(string name)
		{
			CourtEntity court = new CourtEntity()
			{
				Name = name
			};
			_context.Courts.Add(court);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var court = _context.Courts.Find(id);
			_context.SaveChanges();
		}

		public IReadOnlyList<CourtEntity> GetCourts()
		{
			return _context.Courts.ToList();
		}

		public CourtEntity GetCourtById(int id)
		{
			return _context.Courts.Find(id);
		}

		public void Update(int id, string name)
		{
			var court = _context.Courts.Find(id);
			court.Name = name;
			this._context.SaveChanges();
		}

		public bool CourtExists(string name)
		{
			return _context.Courts.Any(court => court.Name.ToLower() == name.ToLower());
		}
	}
}
