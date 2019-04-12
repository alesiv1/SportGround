using System;
using SportGround.Data.Entities;
using System.Collections.Generic;

namespace SportGround.Data.Interfaces
{
	public interface ICourtRepository
	{
		void Add(CourtEntity court);
		void Delete(int id);
		void Delete(CourtEntity court);
		ICollection<CourtEntity> GetCourts();
		CourtEntity GetCourtWithWorkingDays(int id);
		CourtEntity GetCourtById(int id);
		void Update(CourtEntity entity);
		bool CourtExists(string name);
	}
}
