using System;
using System.Collections.Generic;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;

namespace SportGround.BusinessLogic.Operations
{
	public class CourtOperations : ICourtService
	{
		private ICourtRepository _courtRepository;

		public CourtOperations(ICourtRepository courtRepository)
		{
			_courtRepository = courtRepository;
		}

		public void Create(CourtModel model)
		{
			if (CourtExists(model.Name))
			{
				throw new ArgumentException("Court with name {0}  already exists!", model.Name);
			}
			CourtEntity court = new CourtEntity()
			{
				Id = model.Id,
				Name = model.Name
			};
			_courtRepository.Add(court);
		}

		public void Delete(int id)
		{
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
			return court != null ? 
				new CourtModel()
				{
					Id = court.Id,
					Name = court.Name
				} : new CourtModel();
		}

		public void Update(int id, CourtModel model)
		{
			var court = _courtRepository.GetCourtById(id);
			if (court == null)
			{
				throw new ArgumentException("Court doesn't exists!");
			}
			court.Name = model.Name;
			_courtRepository.Update(court);
		}

		public bool CourtExists(string name)
		{
			return _courtRepository.CourtExists(name);
		}
	}
}
