using System;
using System.Collections.Generic;
using System.Linq;
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
			if (String.IsNullOrEmpty(model.Name))
			{
				throw new ArgumentException("Field Name can't be null!");
			}
			if (CourtExists(model.Name))
			{
				throw new ArgumentException("Court with name {0}  already exists!", model.Name);
			}
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
			if (court == null)
			{
				throw new ArgumentException("This court doesn't exists in database!");
			}
			return new CourtModel()
			{
				Id = court.Id,
				Name = court.Name
			};
		}

		public void Update(int id, CourtModel model)
		{
			if (String.IsNullOrEmpty(model.Name))
			{
				throw new ArgumentException("Court name can't be empty!");
			}
			_courtRepository.Update(id, model.Name);
		}

		public bool CourtExists(string name)
		{
			return _courtRepository.CourtExists(name);
		}

		// Methods for working days
		public void Create(int courtId, CourtWorkingDaysModel model)
		{
			if (model.StartTime >= model.EndTime)
			{
				throw new Exception("Start time can't be less then end time");
			}
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
			var workingDay = _courtWorkingDaysRepository.GetCourtWorkingDayById(id);
			if (workingDay == null)
			{
				throw new ArgumentException("Court doesn't exists in database!");
			}
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
			_courtWorkingDaysRepository.Update(id, model.Day, model.StartTime, model.EndTime);
		}

		public List<CourtWorkingDaysModel> GetWorkingDaysForCourt(int courtId)
		{
			var workingDays = new List<CourtWorkingDaysModel>();
			var court = _courtRepository.GetCourtById(courtId);
			foreach (var workingDay in court.WorkingDays)
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

		public List<DaysOfTheWeek> GetAllAvailableDays(int courtId)
		{
			var useDays = GetWorkingDaysForCourt(courtId)
				.Select(x => x.Day);
			var daysOfTheWeeks = new List<DaysOfTheWeek>()
			{
				DaysOfTheWeek.Sunday,
				DaysOfTheWeek.Monday,
				DaysOfTheWeek.Tuesday,
				DaysOfTheWeek.Wednesday,
				DaysOfTheWeek.Thursday,
				DaysOfTheWeek.Friday,
				DaysOfTheWeek.Saturday
			};
			return daysOfTheWeeks
				.FindAll(x => !useDays.Contains(x));
		}
	}
}
