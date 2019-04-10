using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.entities;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;
using SportGround.Data.Migrations;

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
			var userEntity = _userRepository
				.Include(include => include.BookingCourt)
				.FirstOrDefault(id => id.Id == model.User.Id);
			if (userEntity == null)
			{
				throw new Exception();
			}
			CourtBookingEntity booking = new CourtBookingEntity()
			{
				Id = model.Id,
				CourtId = model.CourtId,
				Date =  model.Date
			};
			userEntity.BookingCourt.Add(booking);
			_userRepository.Update(userEntity);
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
			var dateNow = DateTimeOffset.UtcNow;
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

		public List<CourtBookingModel> GetAllUserBooking(string email)
		{
			var allBookingCourts = new List<CourtBookingModel>();
			var userId = _userRepository
				.GetAll()
				.FirstOrDefault(em => em.Email == email)?.Id;
			if (userId == null)
			{
				throw new NullReferenceException("This email doesn't exist in database!");
			}
			var courtBookingEntity = GetAll()
				.Where(user => user.User.Id == userId)
				.ToList();
			if (courtBookingEntity.Count < 1)
			{
				throw new NullReferenceException("This user doesn't have any booking court!");
			}
			foreach (var field in courtBookingEntity)
			{
				allBookingCourts.Add(new CourtBookingModel()
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
					Date = field.Date,
					IsActive = field.Date.Date >= DateTimeOffset.Now.Date,
					DateInString = field.Date.ToString("yyyy-M-d dddd")
				});
			}
			return allBookingCourts.OrderBy(date => date.Date).ToList();
		}
	}
}
