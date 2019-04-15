using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.Enums;
using SportGround.Data.Interfaces;

namespace SportGround.BusinessLogic.Operations
{
	public class CourtWorkingDaysService : ICourtWorkingDaysService
	{
		private ICourtWorkingDaysRepository _courtWorkingDaysRepository;
		private ICourtRepository _courtRepository;

		public CourtWorkingDaysService(ICourtWorkingDaysRepository courtWorkingDays, ICourtRepository courtRepository)
		{
			_courtWorkingDaysRepository = courtWorkingDays;
			_courtRepository = courtRepository;
		}

		public void Create(int courtId, CourtWorkingDaysModel model)
		{
			if (model.StartTime >= model.EndTime)
			{
				throw new Exception("Start time can't be less then end time");
			}
			_courtWorkingDaysRepository.Add(model.Day, model.StartTime, model.EndTime, courtId);
		}

		public void Delete(int id)
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
