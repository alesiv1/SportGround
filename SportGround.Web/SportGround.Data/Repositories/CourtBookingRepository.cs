using SportGround.Data.Context;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

		public void Add(CourtBookingEntity booking, int courtId, int userId)
		{
			var court = _context.Courts.Find(courtId);
			var user = _context.Users.Find(userId);
			booking.Court = court;
			booking.User = user;
			_context.BookingCourts.Add(booking);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var booking = _context.BookingCourts.Find(id);
			_context.BookingCourts.Remove(booking);
			_context.SaveChanges();
		}

		public void Delete(CourtBookingEntity booking)
		{
			_context.BookingCourts.Remove(booking);
			_context.SaveChanges();
		}

		public ICollection<CourtBookingEntity> GetCourtBookings()
		{
			return _context.BookingCourts.ToList();
		}

		public ICollection<CourtBookingEntity> GetCourtBookingWithCourtAndUser()
		{
			return _context.BookingCourts.Include(booking => booking.User).Include(court => court.Court).ToList();
		}

		public CourtBookingEntity GetCourtBookingById(int id)
		{
			return _context.BookingCourts
				.Include(user => user.User)
				.Include(court => court.Court)
				.FirstOrDefault(booking => booking.Id == id);
		}

		public void Update(CourtBookingEntity booking)
		{
			_context.SaveChanges();
		}
	}
}
