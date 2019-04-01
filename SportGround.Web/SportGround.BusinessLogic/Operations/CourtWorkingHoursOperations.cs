using System;
using System.Collections.Generic;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;

namespace SportGround.BusinessLogic.Operations
{
	public class CourtWorkingHoursOperations : ICourtWorkingHoursOperations
	{
		private IDataRepository<CourtWorkingHoursEntity> _courtWorkingHoursRepository;

		public CourtWorkingHoursOperations(IDataRepository<CourtWorkingHoursEntity> courtWorkingHours)
		{
			_courtWorkingHoursRepository = courtWorkingHours;
		}

		public void Create(CourtWorkingHoursModel model)
		{
			CourtWorkingHoursEntity workingHours = new CourtWorkingHoursEntity()
			{
				Id = model.Id,
				CourtId = model.CourtId,
				Day = model.Day,
				StartTime = model.StartTime,
				EndTime = model.EndTime
			};
			_courtWorkingHoursRepository.Insert(workingHours);
		}

		public void Delete(int id)
		{
			_courtWorkingHoursRepository.DeleteById(id);
		}

		public List<CourtWorkingHoursModel> GetAll()
		{
			var allCourt = new List<CourtWorkingHoursModel>();

			var query = _courtWorkingHoursRepository.GetAll();
			foreach (var workingHours in query)
			{
				allCourt.Add(new CourtWorkingHoursModel()
				{
					Id = workingHours.Id,
					CourtId = workingHours.CourtId,
					Day = workingHours.Day,
					StartTime = workingHours.StartTime,
					EndTime = workingHours.EndTime
				});
			}

			return allCourt;
		}

		public CourtWorkingHoursModel GetCourtById(int id)
		{
			var workingHours = _courtWorkingHoursRepository.GetById(id);

			return new CourtWorkingHoursModel()
			{
				Id = workingHours.Id,
				CourtId = workingHours.CourtId,
				Day = workingHours.Day,
				StartTime = workingHours.StartTime,
				EndTime = workingHours.EndTime
			};
		}

		public void Update(int id, CourtWorkingHoursModel model)
		{
			var workingHours = _courtWorkingHoursRepository.GetById(id);
			workingHours.StartTime = model.StartTime;
			workingHours.EndTime = model.EndTime;
			_courtWorkingHoursRepository.Update(workingHours);
		}
	}
}
