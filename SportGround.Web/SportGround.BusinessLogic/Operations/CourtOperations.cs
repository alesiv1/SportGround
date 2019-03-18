using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;

namespace SportGround.BusinessLogic.Operations
{
	public class CourtOperations : ICourtOperations
	{
		private List<CourtModel> data = new List<CourtModel>()
		{
			new CourtModel(){ Id = 1, Name = "First" },
			new CourtModel(){ Id = 2, Name = "Two" },
			new CourtModel(){ Id = 3, Name = "Three" },
			new CourtModel(){ Id = 4, Name = "Four" },
		};

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
