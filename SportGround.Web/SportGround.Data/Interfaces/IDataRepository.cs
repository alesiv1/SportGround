using System;
using System.Collections.Generic;

namespace SportGround.Data.Interfaces
{
	public interface IDataRepository<TEntity> where TEntity : class
	{
		TEntity GetById(object id);
		ICollection<TEntity> GetAll();
		void Insert(TEntity entity);
		void Update(TEntity entity);
		void Delete(TEntity entity);
		void DeleteById(object id);
	}
}
