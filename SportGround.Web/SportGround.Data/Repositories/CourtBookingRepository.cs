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

		public void Add(DateTimeOffset startDate, DateTimeOffset EndDate, int courtId, int userId)
		{
			var court = _context.Courts.Find(courtId);
			var user = _context.Users.Find(userId);
			CourtBookingEntity booking = new CourtBookingEntity()
			{
				StartDate = startDate,
				EndDate = EndDate,
				Court = court,
				User = user
			};
			_context.BookingCourts.Add(booking);
			_context.SaveChanges();
		}

		public void Delete(long id)
		{
			var booking = _context.BookingCourts.Find(id);
			_context.BookingCourts.Remove(booking);
			_context.SaveChanges();
		}

		public void DeleteRangeByUserId(int userId)
		{
			var bookings = _context.BookingCourts.Where(book => book.User.Id == userId);
			_context.BookingCourts.RemoveRange(bookings);
			_context.SaveChanges();
		}

		public void DeleteRangeByCourtId(int courtId)
		{
			var bookings = _context.BookingCourts.Where(book => book.Court.Id == courtId);
			_context.BookingCourts.RemoveRange(bookings);
			_context.SaveChanges();
		}

		public IReadOnlyList<CourtBookingEntity> GetCourtBookings()
		{
			return _context.BookingCourts.ToList();
		}

		public CourtBookingEntity GetCourtBookingById(long id)
		{
			return _context.BookingCourts.Find(id);
		}

		public void Update(long id, DateTimeOffset startDate, DateTimeOffset EndDate)
		{
			var booking = _context.BookingCourts.Find(id);
			booking.StartDate = startDate;
			booking.EndDate = EndDate;
			_context.SaveChanges();
		}
	}
}
