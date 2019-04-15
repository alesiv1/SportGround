using System;
using SportGround.Data.Entities;
using System.Collections.Generic;

namespace SportGround.Data.Interfaces
{
	public interface ICourtRepository
	{
		void Add(string name);
		void Delete(int id);
		ICollection<CourtEntity> GetCourts();
		CourtEntity GetCourtById(int id);
		void Update(int id, string name);
		bool CourtExists(string name);
	}
}
