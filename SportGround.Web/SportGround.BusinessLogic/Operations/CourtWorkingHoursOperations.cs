using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.Entities;
using SportGround.Data.Enums;
using SportGround.Data.Interfaces;

namespace SportGround.BusinessLogic.Operations
{
	public class CourtWorkingHoursOperations : ICourtWorkingHoursOperations
	{
		private IDataRepository<CourtWorkingHoursEntity> _courtWorkingHoursRepository;
		private IDataRepository<CourtEntity> _courtRepository;

		public CourtWorkingHoursOperations(IDataRepository<CourtWorkingHoursEntity> courtWorkingHours, IDataRepository<CourtEntity> courtRepository)
		{
			_courtWorkingHoursRepository = courtWorkingHours;
			_courtRepository = courtRepository;
		}

		public void Create(int courtId, CourtWorkingHoursModel model)
		{
			var courtEntity = _courtRepository
				.Include(include => include.WorkingHours)
				.FirstOrDefault(id => id.Id == courtId);
			if (courtEntity == null)
			{
				throw new ArgumentNullException("This court doesn't exist in database!");
			}
			if (model.StartTime >= model.EndTime)
			{
				throw new Exception("Start time can't be less then end time");
			}
			var workingHours = new CourtWorkingHoursEntity()
			{
				Day = model.Day,
				StartTime = model.StartTime,
				EndTime = model.EndTime
			};
			courtEntity.WorkingHours.Add(workingHours);
			_courtRepository.Update(courtEntity);
		}

		public void Delete(int id)
		{
			_courtWorkingHoursRepository.DeleteById(id);
		}

		public List<CourtWorkingHoursModel> GetAll()
		{
			var allHoursModel = new List<CourtWorkingHoursModel>();
			var allWorkingHoursEntity = _courtWorkingHoursRepository.GetAll();
			foreach (var workingHours in allWorkingHoursEntity)
			{
				allHoursModel.Add(new CourtWorkingHoursModel()
				{
					Id = workingHours.Id,
					Day = workingHours.Day,
					StartTime = workingHours.StartTime,
					EndTime = workingHours.EndTime
				});
			}
			return allHoursModel;
		}

		public CourtWorkingHoursModel GetById(int id)
		{
			var workingHoursEntity = _courtWorkingHoursRepository
				.Include(court => court.CourtId)
				.FirstOrDefault(i => i.Id == id);
			return workingHoursEntity != null ?
				new CourtWorkingHoursModel()
				{
					Id = workingHoursEntity.Id,
					Court = new CourtModel()
					{
						Id = workingHoursEntity.CourtId.Id,
						Name = workingHoursEntity.CourtId.Name
					},
					Day = workingHoursEntity.Day,
					StartTime = workingHoursEntity.StartTime,
					EndTime = workingHoursEntity.EndTime
				} : null;
		}

		public void Update(int id, CourtWorkingHoursModel model)
		{
			var workingHoursEntity = _courtWorkingHoursRepository.GetById(id);
			workingHoursEntity.Day = model.Day;
			workingHoursEntity.StartTime = model.StartTime;
			workingHoursEntity.EndTime = model.EndTime;
			_courtWorkingHoursRepository.Update(workingHoursEntity);
		}

		public List<CourtWorkingHoursModel> GetAllForCourt(int courtId)
		{
			var allWorkingHoursModel = new List<CourtWorkingHoursModel>();
			var courtWorkingHoursEntity = _courtWorkingHoursRepository
				.Include(court => court.CourtId)
				.Where(id => id.CourtId.Id == courtId);
			foreach (var workingHours in courtWorkingHoursEntity)
			{
				allWorkingHoursModel.Add(new CourtWorkingHoursModel()
				{
					Id = workingHours.Id,
					Court = new CourtModel()
						{
							Id = workingHours.CourtId.Id,
							Name = workingHours.CourtId.Name
						},
					Day = workingHours.Day,
					StartTime = workingHours.StartTime,
					EndTime = workingHours.EndTime
				});
			}
			return allWorkingHoursModel;
		}

		public List<DaysOfTheWeek> GetAllAvailableDays(int courtId)
		{
			var useDays = GetAllForCourt(courtId)
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
