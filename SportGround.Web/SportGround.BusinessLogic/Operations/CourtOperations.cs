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
	public class CourtOperations : ICourtOperations
	{
		private IDataRepository<CourtEntity> _courtData;

		public CourtOperations(IDataRepository<CourtEntity> courtRepository)
		{
			_courtData = courtRepository;
		}

		public void Create(CourtModel model)
		{
			CourtEntity court = new CourtEntity()
			{
				Id = model.Id,
				Name = model.Name
			};
			_courtData.Insert(court);
		}

		public void Delete(int id)
		{
			_courtData.DeleteById(id);
		}

		public List<CourtModel> GetAll()
		{
			var allCourt = new List<CourtModel>();
			var query = _courtData.Get;
			foreach (var court in query)
			{
				allCourt.Add(new CourtModel()
				{
					Id = court.Id,
					Name = court.Name
				});
			}
			return allCourt;
		}

		public CourtModel GetCourtById(int id)
		{
			var courtEntity = _courtData.GetById(id);
			return new CourtModel()
			{
				Id = courtEntity.Id,
				Name = courtEntity.Name
			};
		}

		public void Update(int id, CourtModel model)
		{
			var court = _courtData.GetById(id);
			court.Name = model.Name;
			_courtData.Update(court);
		}
	}
}
