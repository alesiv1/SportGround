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
			var allCourtModel = new List<CourtModel>();

			var courtEntity = _courtRepository
				.Include(x => x.WorkingHours);
			foreach (var court in courtEntity)
			{
				allCourtModel.Add(new CourtModel()
				{
					Id = court.Id,
					Name = court.Name,
					CanBooking = court.WorkingHours.Count > 0
				});
			}

			return allCourtModel;
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
			var courtEntity = _courtRepository.GetById(id);
			courtEntity.Name = model.Name;
			_courtRepository.Update(courtEntity);
		}

		public CourtEntity GetCourtEntityById(int id)
		{
			return _courtRepository.GetById(id);
		}
	}
}
