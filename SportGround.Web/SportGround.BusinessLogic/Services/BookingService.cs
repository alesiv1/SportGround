using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.Interfaces;

namespace SportGround.BusinessLogic.Operations
{
	public class BookingService : IBookingService
	{
		private ICourtBookingRepository _bookingRepository;
		private IUserRepository _userRepository;
		private ICourtRepository _courtRepository;

		public BookingService(ICourtBookingRepository bookingRepository, IUserRepository userRepository, ICourtRepository courtRepository)
		{
			_bookingRepository = bookingRepository;
			_userRepository = userRepository;
			_courtRepository = courtRepository;
		}

		public void Create(CourtBookingModel model)
		{
			_bookingRepository.Add(model.StartDate, model.EndDate, model.Court.Id, model.User.Id);
		}

		public void Delete(long id)
		{
			_bookingRepository.Delete(id);
		}

		public IReadOnlyList<CourtBookingModel> GetBookingList()
		{
			return _bookingRepository.GetCourtBookings()
				.Select(booking => new CourtBookingModel()
					{
						Id = booking.Id,
						User = new UserModel()
						{
							Id = booking.User.Id,
							Email = booking.User.Email,
							FirstName = booking.User.FirstName,
							LastName = booking.User.LastName
						},
						Court = new CourtModel()
						{
							Id = booking.Court.Id,
							Name = booking.Court.Name
						},
						StartDate = booking.StartDate,
						EndDate = booking.EndDate
					})
				.ToList();
		}

		public CourtBookingModel GetCourtBookingById(long id)
		{
			var booking = _bookingRepository
				.GetCourtBookingById(id);
			return new CourtBookingModel()
			{
				Id = booking.Id,
				User = new UserModel()
					{
					Id = booking.User.Id,
					Email = booking.User.Email,
					FirstName = booking.User.FirstName,
					LastName = booking.User.LastName
				},
				Court = new CourtModel()
				{
					Id = booking.Court.Id,
					Name = booking.Court.Name
				},
				StartDate = booking.StartDate,
				EndDate = booking.EndDate
			};
		}

		public void Update(long id, CourtBookingModel model)
		{
			_bookingRepository.Update(id, model.StartDate, model.EndDate);
		}

		public IReadOnlyList<CourtBookingModel> GetAllUserBooking(int userId)
		{
			return _userRepository.GetUserById(userId)
				.BookingCourts
				.Select(booking => new CourtBookingModel()
					{
					Id = booking.Id,
					User = new UserModel()
					{
						Id = booking.User.Id,
						Email = booking.User.Email,
						FirstName = booking.User.FirstName,
						LastName = booking.User.LastName
					},
					Court = new CourtModel()
					{
						Id = booking.Court.Id,
						Name = booking.Court.Name
					},
					StartDate = booking.StartDate,
					EndDate = booking.EndDate,
					IsActive = booking.StartDate.Date >= DateTimeOffset.Now.Date,
					DateInString = booking.StartDate.ToString("yyyy-M-d dddd")
					})
				.ToList();
		}
	}
}
