using System;
using System.Collections.Generic;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;

namespace SportGround.BusinessLogic.Operations
{
	public class CourtOperations : ICourtOperations
	{
		private IDataRepository<CourtEntity> _courtRepository;

		public CourtOperations(IDataRepository<CourtEntity> courtRepository)
		{
			_courtRepository = courtRepository;
		}

		public void Create(CourtModel model)
		{
			CourtEntity court = new CourtEntity()
			{
				Id = model.Id,
				Name = model.Name,
			};
			_courtRepository.Insert(court);
		}

		public void Delete(int id)
		{
			_courtRepository.DeleteById(id);
		}

		public List<CourtModel> GetAll()
		{
			var allCourt = new List<CourtModel>();

			var courtWithWorkingHours = _courtRepository
				.Include(x => x.WorkingHours);
			foreach (var court in courtWithWorkingHours)
			{
				allCourt.Add(new CourtModel()
				{
					Id = court.Id,
					Name = court.Name,
					CanBooking = court.WorkingHours.Count > 0
				});
			}

			return allCourt;
		}

		public CourtModel GetCourtById(int id)
		{
			var courtEntity = _courtRepository.GetById(id);
			return courtEntity != null ? 
				new CourtModel()
				{
					Id = courtEntity.Id,
					Name = courtEntity.Name
				} : null;
		}

		public void Update(int id, CourtModel model)
		{
			var court = _courtRepository.GetById(id);
			court.Name = model.Name;
			_courtRepository.Update(court);
		}

		public CourtEntity GetCourtEntityById(int id)
		{
			return _courtRepository.GetById(id);
		}
	}
}
