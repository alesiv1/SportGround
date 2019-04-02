using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;
using SportGround.Data.Repositories;

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
			var court = _courtRepository.Include(include => include.WorkingHours).FirstOrDefault(id => id.Id == courtId);
			
			var workingHours = new CourtWorkingHoursEntity()
			{
				Day = model.Day,
				StartTime = model.StartTime,
				EndTime = model.EndTime
			};
			court.WorkingHours.Add(workingHours);
			_courtRepository.Update(court);
		}

		public void Delete(int id)
		{
			_courtWorkingHoursRepository.DeleteById(id);
		}

		public List<CourtWorkingHoursModel> GetAll()
		{
			var allHours = new List<CourtWorkingHoursModel>();

			var query = _courtWorkingHoursRepository.GetAll();
			foreach (var workingHours in query)
			{
				allHours.Add(new CourtWorkingHoursModel()
				{
					Id = workingHours.Id,
					Day = workingHours.Day,
					StartTime = workingHours.StartTime,
					EndTime = workingHours.EndTime
				});
			}

			return allHours;
		}

		public CourtWorkingHoursModel GetById(int id)
		{
			var workingHours = _courtWorkingHoursRepository.Include(court => court.CourtId)
				.FirstOrDefault(i => i.Id == id);
			return new CourtWorkingHoursModel()
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
			};
		}

		public void Update(int id, CourtWorkingHoursModel model)
		{
			var workingHours = _courtWorkingHoursRepository.GetById(id);
			workingHours.Day = model.Day;
			workingHours.StartTime = model.StartTime;
			workingHours.EndTime = model.EndTime;
			_courtWorkingHoursRepository.Update(workingHours);
		}

		public List<CourtWorkingHoursModel> GetAllForCourt(int courtId)
		{
			var allHours = new List<CourtWorkingHoursModel>();

			var query = _courtWorkingHoursRepository.Include(court => court.CourtId).Where(id => id.CourtId.Id == courtId);
			foreach (var workingHours in query)
			{
				allHours.Add(new CourtWorkingHoursModel()
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

			return allHours;
		}
	}
}
