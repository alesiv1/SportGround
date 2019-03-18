using SportGround.Data.Context;
using SportGround.Data.Interfaces;
using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;

namespace SportGround.Data.Repositories
{
	public class DataRepository<TEntity> : IDataRepository<TEntity> where TEntity : class
	{
		private readonly DataContext _context;
		private readonly IDbSet<TEntity> _entities;
		private string _errorMessage = string.Empty;

		public DataRepository(DataContext context)
		{
			this._context = context;
			this._entities = context.Set<TEntity>();
		}

		public TEntity GetById(object id)
		{
			return this._entities.Find(id);
		}

		public void Insert(TEntity entity)
		{
			try
			{
				if (entity == null)
				{
					throw new ArgumentNullException("entity");
				}
				this._entities.Add(entity);
				this._context.SaveChanges();
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						_errorMessage += string.Format("Property: {0} Error: {1}",
						validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
					}
				}
				throw new Exception(_errorMessage, dbEx);
			}
		}

		public void Update(TEntity entity)
		{
			try
			{
				if (entity == null)
				{
					throw new ArgumentNullException("entity");
				}
				this._context.SaveChanges();
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
						validationError.PropertyName, validationError.ErrorMessage);
					}
				}

				throw new Exception(_errorMessage, dbEx);
			}
		}

		public void Delete(TEntity entity)
		{
			try
			{
				if (entity == null)
				{
					throw new ArgumentNullException("entity");
				}

				this._entities.Remove(entity);
				this._context.SaveChanges();
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
						validationError.PropertyName, validationError.ErrorMessage);
					}
				}
				throw new Exception(_errorMessage, dbEx);
			}
		}

		public void DeleteById(object id)
		{
			try
			{
				TEntity entityToDelete = this._entities.Find(id);

				if (entityToDelete == null)
				{
					throw new ArgumentNullException("entity");
				}

				this._entities.Remove(entityToDelete);
				this._context.SaveChanges();
			}
			catch (DbEntityValidationException dbEx)
			{
				foreach (var validationErrors in dbEx.EntityValidationErrors)
				{
					foreach (var validationError in validationErrors.ValidationErrors)
					{
						_errorMessage += Environment.NewLine + string.Format("Property: {0} Error: {1}",
						validationError.PropertyName, validationError.ErrorMessage);
					}
				}
			}
		}

		public IQueryable<TEntity> Get
		{
			get
			{
				return this._entities;
			}
		}

		public IQueryable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] include)
		{
			IQueryable<TEntity> query = this._entities;
			query = include.Aggregate(query, (current, inc) => current.Include(inc));
			return query.Where(predicate);
		}

		public IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] include)
		{
			IQueryable<TEntity> query = this._entities;
			return include.Aggregate(query, (current, inc) => current.Include(inc));
		}
	}
}
