using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;

namespace SportGround.BusinessLogic.Operations
{
	public class CourtOperations : ICourtOperations
	{
		private List<CourtModel> data = new List<CourtModel>();

		public void Create(CourtModel model)
		{
			data.Add(model);
		}

		public void Delete(int id)
		{
			data.Remove(data.FirstOrDefault(s => s.Id == id));
		}

		public List<CourtModel> GetAll()
		{
			return data;
		}

		public CourtModel GetCourtById(int id)
		{
			return data.FirstOrDefault(s => s.Id == id);
		}

		public void Update(int id, CourtModel model)
		{
			var record = data.FirstOrDefault(s => s.Id == id);
			if (record != null)
			{
				record.Id = model.Id;
				record.Name = model.Name;
			}
		}
	}
}
