using System;
using System.Linq;
using System.Linq.Expressions;

namespace SportGround.Data.Interfaces
{
	public interface IDataRepository<TEntity> where TEntity : class
	{
		TEntity GetById(object id);
		void Insert(TEntity entity);
		void Update(TEntity entity);
		void Delete(TEntity entity);
		void DeleteById(object id);
		IQueryable<TEntity> Get { get; }

		IQueryable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate,
			params Expression<Func<TEntity, object>>[] include);

		IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] include);
	}
}
