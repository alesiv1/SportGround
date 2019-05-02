using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.Entities;
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

		public List<CourtBookingModel> GetBookingList()
		{
			var bookingList = new List<CourtBookingModel>();
			var bookings = _bookingRepository.GetCourtBookings();
			foreach (var booking in bookings)
			{
				bookingList.Add(new CourtBookingModel()
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
				});
			}
			return bookingList;
		}

		public List<DateTimeOffset> GetAllAvailableDataTime(int courtId)
		{
			var dateNow = DateTimeOffset.UtcNow;
			List<DateTimeOffset> allAvailableDataTime = new List<DateTimeOffset>();
			List<CourtWorkingDaysEntity> courtsWithWorkingHours = _courtRepository
				.GetCourtById(courtId).WorkingDays;
			if (courtsWithWorkingHours.Count > 0)
			{
				foreach (var data in courtsWithWorkingHours)
				{
					var add_Days = (int) data.Day < (int) dateNow.Day
						? 7 - ((int) dateNow.Day - (int) data.Day)
						: (int) data.Day - (int) dateNow.Day;
					var date = DateTimeOffset.Now.AddDays(add_Days).Date;
					allAvailableDataTime.Add(date);
					for (int i = 1; i < 4; i++)
					{
						allAvailableDataTime.Add(date.AddDays(7 * i));
					}
				}
			}
			else return allAvailableDataTime;

			var bookedCourtDate = _bookingRepository
				.GetCourtBookings()
				.Select(x => x.StartDate)
				.ToList();
			allAvailableDataTime = allAvailableDataTime
				.FindAll(x => !bookedCourtDate.Contains(x.Date) && x.Date >= dateNow.Date)
				.OrderBy(date => date.Date)
				.ToList();
			return allAvailableDataTime;
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

		public List<CourtBookingModel> GetAllUserBooking(int userId)
		{
			var bookingList = new List<CourtBookingModel>();
			var bookings = _userRepository.GetUserById(userId).BookingCourts;
			foreach (var booking in bookings)
			{
				bookingList.Add(new CourtBookingModel()
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
				});
			}
			return bookingList;
		}
	}
}
