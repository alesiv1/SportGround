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
			CourtBookingEntity booking = new CourtBookingEntity()
			{
				Id = model.Id,
				User = _userRepository.GetById(model.User.Id),
				Court = _courtRepository.GetById(model.Court.Id),
				Date =  model.Date
			};
			_bookingRepository.Insert(booking);
		}

		public void Delete(int id)
		{
			_bookingRepository.DeleteById(id);
		}

		public List<CourtBookingModel> GetAll()
		{
			var allCourt = new List<CourtBookingModel>();

			var query = _bookingRepository.GetAll();
			foreach (var field in query)
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
					Court = new CourtModel()
					{
						Id = field.Court.Id,
						Name = field.Court.Name
					},
					Date = field.Date
				});
			}

			return allCourt;
		}

		public List<DateTimeOffset> GetAllAvailableDataTime(int courtId)
		{
			List<DateTimeOffset> allAvailableDataTime = new List<DateTimeOffset>();
			List<CourtWorkingHoursEntity> workingHours = _courtRepository.Include(wh => wh.WorkingHours).FirstOrDefault(id => id.Id == courtId).WorkingHours;
			if (workingHours != null)
			{
				foreach (var data in workingHours)
				{
					var date = data.StartTime.Date;
					allAvailableDataTime.Add(date);
					for (int i = 1; i < 4; i++)
					{
						allAvailableDataTime.Add(date.AddDays(7*i));
					}
				}
			}

			var bookedCourtDateTime = _bookingRepository
				.Include(court => court.Court)
				.Where(ci => ci.Court.Id == courtId)
				.Select(x => x.Date).ToList();

			allAvailableDataTime = allAvailableDataTime.FindAll(x => !bookedCourtDateTime.Contains(x));
			return allAvailableDataTime;
		}

		public List<CourtBookingModel> GetAllForUser(int userId)
		{
			var allCourt = new List<CourtBookingModel>();

			var query = _userRepository.Include(x => x.BookingCourt).FirstOrDefault(id => id.Id == userId);
			if (query != null)
			{
				foreach (var booking in query.BookingCourt)
				{
					allCourt.Add(new CourtBookingModel()
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
						Date = booking.Date
					});
				}
			}
			return allCourt;
		}

		public CourtBookingModel GetById(int id)
		{
			var booking = _bookingRepository.Include(court => court.Court, user => user.User)
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
					Court = new CourtModel()
					{
						Id = booking.Court.Id,
						Name = booking.Court.Name
					},
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
