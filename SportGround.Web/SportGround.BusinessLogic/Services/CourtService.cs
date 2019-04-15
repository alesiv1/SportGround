using System;
using System.Collections.Generic;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
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
	}
}
