using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.entities;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;

namespace SportGround.BusinessLogic.Operations
{
	public class CourtBookingOperations : ICourtBookingOperations
	{
		private IDataRepository<CourtBookingEntity> _bookingRepository;
		private IDataRepository<UserEntity> _userRepository;
		private IDataRepository<CourtEntity> _courtRepository;

		public CourtBookingOperations(IDataRepository<CourtBookingEntity> bookingRepository, IDataRepository<UserEntity> userRepository, IDataRepository<CourtEntity> courtRepository)
		{
			_bookingRepository = bookingRepository;
			_userRepository = userRepository;
			_courtRepository = courtRepository;
		}

		public void Create(CourtBookingModel model)
		{
			var user = _userRepository
				.Include(include => include.BookingCourt)
				.FirstOrDefault(id => id.Id == model.User.Id);
			if (user == null)
			{
				throw new Exception();
			}
			CourtBookingEntity booking = new CourtBookingEntity()
			{
				Id = model.Id,
				CourtId = model.CourtId,
				Date =  model.Date
			};
			user.BookingCourt.Add(booking);
			_userRepository.Update(user);
		}

		public void Delete(int id)
		{
			_bookingRepository.DeleteById(id);
		}

		public List<CourtBookingModel> GetAll()
		{
			var allCourt = new List<CourtBookingModel>();
			var courtBookingEntity = _bookingRepository
				.Include(include => include.User);
			foreach (var field in courtBookingEntity)
			{
				allCourt.Add(new CourtBookingModel()
				{
					Id = field.Id,
					User = new UserModel()
					{
						Id = field.User.Id,
						Email = field.User.Email,
						FirstName = field.User.FirstName,
						LastName = field.User.LastName
					},
					CourtId = field.CourtId,
					CourtName = _courtRepository.GetById(field.CourtId).Name,
					Date = field.Date
				});
			}
			return allCourt;
		}

		public List<DateTimeOffset> GetAllAvailableDataTime(int courtId)
		{
			var dateNow = DateTimeOffset.Now;
			List<DateTimeOffset> allAvailableDataTime = new List<DateTimeOffset>();
			List<CourtWorkingHoursEntity> courtsWithWorkingHours = _courtRepository
				.Include(wh => wh.WorkingHours)
				.FirstOrDefault(id => id.Id == courtId)
				.WorkingHours;
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
				.GetAll()
				.Where(ci => ci.CourtId == courtId)
				.Select(x => x.Date.Date)
				.ToList();
			allAvailableDataTime = allAvailableDataTime
				.FindAll(x => !bookedCourtDate.Contains(x.Date) && x.Date >= dateNow.Date)
				.OrderBy(date => date.Date)
				.ToList();
			return allAvailableDataTime;
		}

		public List<CourtBookingModel> GetAllForUser(int userId)
		{
			var allBookedCourt = new List<CourtBookingModel>();
			var userWithBooking = _userRepository
				.Include(x => x.BookingCourt)
				.FirstOrDefault(id => id.Id == userId);
			if (userWithBooking != null)
			{
				foreach (var booking in userWithBooking.BookingCourt)
				{
					allBookedCourt.Add(new CourtBookingModel()
					{
						Id = booking.Id,
						User = new UserModel()
						{
							Id = booking.User.Id,
							Email = booking.User.Email,
							FirstName = booking.User.FirstName,
							LastName = booking.User.LastName
						},
						CourtId = booking.CourtId,
						CourtName = _courtRepository.GetById(booking.CourtId).Name,
						Date = booking.Date
					});
				}
			}
			return allBookedCourt;
		}

		public CourtBookingModel GetById(int id)
		{
			var booking = _bookingRepository
				.Include(user => user.User)
				.FirstOrDefault(i => i.Id == id);
			return booking != null
				? new CourtBookingModel()
				{
					Id = booking.Id,
					User = new UserModel()
					{
						Id = booking.User.Id,
						Email = booking.User.Email,
						FirstName = booking.User.FirstName,
						LastName = booking.User.LastName
					},
					CourtId = booking.CourtId,
					CourtName = _courtRepository.GetById(booking.CourtId).Name,
					Date = booking.Date
				}
				: null;
		}

		public void Update(int id, CourtBookingModel model)
		{
			var booking = _bookingRepository.GetById(id);
			booking.Date = model.Date;
			_bookingRepository.Update(booking);
		}
	}
}
