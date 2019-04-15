using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;

namespace SportGround.BusinessLogic.Operations
{
	public class CourtBookingService : ICourtBookingService
	{
		private ICourtBookingRepository _bookingRepository;
		private IUserRepository _userRepository;
		private ICourtRepository _courtRepository;

		public CourtBookingService(ICourtBookingRepository bookingRepository, IUserRepository userRepository, ICourtRepository courtRepository)
		{
			_bookingRepository = bookingRepository;
			_userRepository = userRepository;
			_courtRepository = courtRepository;
		}

		public void Create(CourtBookingModel model)
		{
			_bookingRepository.Add(model.Date, model.Court.Id, model.User.Id);
		}

		public void Delete(int id)
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
					Date = booking.BookingDate
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
				.Select(x => x.BookingDate.Date)
				.ToList();
			allAvailableDataTime = allAvailableDataTime
				.FindAll(x => !bookedCourtDate.Contains(x.Date) && x.Date >= dateNow.Date)
				.OrderBy(date => date.Date)
				.ToList();
			return allAvailableDataTime;
		}

		public CourtBookingModel GetCourtBookingById(int id)
		{
			var booking = _bookingRepository
				.GetCourtBookingById(id);
			if (booking == null)
			{
				throw new ArgumentException("Booking doesn't exists in database");
			}
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
				Date = booking.BookingDate
			};
		}

		public void Update(int id, CourtBookingModel model)
		{
			_bookingRepository.Update(id, model.Date);
		}

		public List<CourtBookingModel> GetAllUserBooking(string email)
		{
			var userId = _userRepository
				.GetUsers()
				.FirstOrDefault(em => em.Email == email)?.Id;
			if (userId == null)
			{
				throw new NullReferenceException("This email doesn't exist in database!");
			}
			var bookingsForUser = GetBookingList()
				.Where(user => user.User.Id == userId)
				.ToList();
			if (bookingsForUser.Count < 1)
			{
				throw new NullReferenceException("This user doesn't have any booking court!");
			}
			bookingsForUser.ForEach(x => x.IsActive = x.Date.Date >= DateTimeOffset.Now.Date);
			bookingsForUser.ForEach(x => x.DateInString = x.Date.ToString("yyyy-M-d dddd"));		
			return bookingsForUser.OrderBy(date => date.Date).ToList();
		}
	}
}
