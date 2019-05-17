using System;
using SportGround.BusinessLogic.Models;
using System.Collections.Generic;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface ICourtService
	{
		CourtModel GetCourtById(int id);
		IReadOnlyList<CourtModel> GetCourtList();
		void Create(CourtModel model);
		void Update(int id, CourtModel model);
		void DeleteCourt(int id);
		bool CourtExists(string name);
	}
}
