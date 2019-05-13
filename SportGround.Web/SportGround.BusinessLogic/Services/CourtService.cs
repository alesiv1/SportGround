using System;
using System.Collections.Generic;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.Enums;
using SportGround.Data.Interfaces;

namespace SportGround.BusinessLogic.Operations
{
	public class CourtService : ICourtService, ICourtWorkingDaysService
	{
		private ICourtRepository _courtRepository;
		private ICourtBookingRepository _bookingRepository;
		private ICourtWorkingDaysRepository _courtWorkingDaysRepository;

		public CourtService(ICourtRepository courtRepository, ICourtBookingRepository bookingRepository, ICourtWorkingDaysRepository courtWorkingDays)
		{
			_courtRepository = courtRepository;
			_bookingRepository = bookingRepository;
			_courtWorkingDaysRepository = courtWorkingDays;
		}

		// Methods for courts
		public void Create(CourtModel model)
		{
			_courtRepository.Add(model.Name);
		}

		public void DeleteCourt(int id)
		{
			_bookingRepository.DeleteRangeByCourtId(id);
			_courtWorkingDaysRepository.DeleteRangeByCourtId(id);
			_courtRepository.Delete(id);
		}

		public List<CourtModel> GetCourtList()
		{
			var courtsList = new List<CourtModel>();
			var courts = _courtRepository.GetCourts();
			foreach (var court in courts)
			{
				courtsList.Add(new CourtModel()
				{
					Id = court.Id,
					Name = court.Name,
					CanBooking = court.WorkingDays.Count > 0
				});
			}
			return courtsList;
		}

		public CourtModel GetCourtById(int id)
		{
			var court = _courtRepository.GetCourtById(id);
			return new CourtModel()
			{
				Id = court.Id,
				Name = court.Name
			};
		}

		public void Update(int id, CourtModel model)
		{
			_courtRepository.Update(id, model.Name);
		}

		public bool CourtExists(string name)
		{
			return _courtRepository.CourtExists(name);
		}

		// Methods for working days
		public void Create(int courtId, CourtWorkingDaysModel model)
		{
			_courtWorkingDaysRepository.Add(model.Day, model.StartTime, model.EndTime, courtId);
		}

		public void DeleteWorkingDays(int id)
		{
			_courtWorkingDaysRepository.Delete(id);
		}

		public List<CourtWorkingDaysModel> GetWorkingDaysList()
		{
			var workingDaysList = new List<CourtWorkingDaysModel>();
			var workingDays = _courtWorkingDaysRepository.GetWorkingDays();
			foreach (var workingDay in workingDays)
			{
				workingDaysList.Add(new CourtWorkingDaysModel()
				{
					Id = workingDay.Id,
					Day = workingDay.Day,
					StartTime = workingDay.StartTimeOfDay,
					EndTime = workingDay.EndTimeOfDay
				});
			}
			return workingDaysList;
		}

		public CourtWorkingDaysModel GetWorkingDay(int id)
		{
			var workingDay = _courtWorkingDaysRepository.GetWorkingDayById(id);
			return new CourtWorkingDaysModel()
			{
				Id = workingDay.Id,
				Court = new CourtModel()
				{
					Id = workingDay.Court.Id,
					Name = workingDay.Court.Name
				},
				Day = workingDay.Day,
				StartTime = workingDay.StartTimeOfDay,
				EndTime = workingDay.EndTimeOfDay
			};
		}

		public void Update(int id, CourtWorkingDaysModel model)
		{
			_courtWorkingDaysRepository.Update(id, (DaysOfTheWeek) model.StartTime.DayOfWeek, model.StartTime, model.EndTime);
		}

		public List<CourtWorkingDaysModel> GetWorkingDaysForCourt(int courtId)
		{
			var workingDays = new List<CourtWorkingDaysModel>();
			var courtWorkingDays = _courtRepository.GetCourtById(courtId).WorkingDays;
			foreach (var workingDay in courtWorkingDays)
			{
				workingDays.Add(new CourtWorkingDaysModel()
				{
					Id = workingDay.Id,
					Court = new CourtModel()
					{
						Id = workingDay.Court.Id,
						Name = workingDay.Court.Name
					},
					Day = workingDay.Day,
					StartTime = workingDay.StartTimeOfDay,
					EndTime = workingDay.EndTimeOfDay
				});
			}
			return workingDays;
		}
	}
}
