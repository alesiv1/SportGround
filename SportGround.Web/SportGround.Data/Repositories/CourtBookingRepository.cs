using System;
using SportGround.Data.Context;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace SportGround.Data.Repositories
{
	public class CourtBookingRepository : ICourtBookingRepository
	{
		private readonly DataContext _context;

		public CourtBookingRepository(DataContext context)
		{
			this._context = context;
		}

		public void Add(DateTimeOffset date, int courtId, int userId)
		{
			var court = _context.Courts.Find(courtId);
			var user = _context.Users.Find(userId);
			CourtBookingEntity booking = new CourtBookingEntity()
			{
				BookingDate = date,
				Court = court,
				User = user
			};
			_context.BookingCourts.Add(booking);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var booking = _context.BookingCourts.Find(id);
			_context.BookingCourts.Remove(booking);
			_context.SaveChanges();
		}

		public ICollection<CourtBookingEntity> GetCourtBookings()
		{
			return _context.BookingCourts.ToList();
		}

		public CourtBookingEntity GetCourtBookingById(int id)
		{
			return _context.BookingCourts.Find(id);
		}

		public void Update(int id, DateTimeOffset date)
		{
			var booking = _context.BookingCourts.Find(id);
			booking.BookingDate = date;
			_context.SaveChanges();
		}
	}
}
