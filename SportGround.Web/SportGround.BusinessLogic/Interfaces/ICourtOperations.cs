using SportGround.BusinessLogic.Models;
using System;
using System.Collections.Generic;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface ICourtOperations
	{
		CourtModel GetCourtById(int id);
		List<CourtModel> GetAll();
		void Create(CourtModel model);
		void Update(int id, CourtModel model);
		void Delete(int id);
	}
}
