using System;
using SportGround.BusinessLogic.Models;
using System.Collections.Generic;
using SportGround.Data.Entities;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface ICourtOperations
	{
		CourtModel GetCourtById(int id);
		List<CourtModel> GetAll();
		void Create(CourtModel model);
		void Update(int id, CourtModel model);
		void Delete(int id);
		CourtEntity GetCourtEntityById(int id);
	}
}
