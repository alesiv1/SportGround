using SportGround.Data.Context;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SportGround.Data.Repositories
{
	public class CourtDataRepository : IDataRepository<CourtEntity>
	{
		private readonly DataContext _context;

		public CourtDataRepository(DataContext context)
		{
			this._context = context;
		}

		public CourtEntity GetById(object id)
		{
			try
			{
				return this._context.Courts.Find(id);
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("The court doesn't exist in the database!");
			}
		}

		public CourtEntity GetWithWorkingHoursBuId(int courtId)
		{
			try
			{
				return this._context.Courts.Include(wh => wh.WorkingHours).FirstOrDefault(id => id.Id == (int)courtId);
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("The court doesn't exist in the database!");
			}
		}

		public void Insert(CourtEntity entity)
		{
			if (entity == null)
			{
				throw new NullReferenceException("The court doesn't exist in the database!");
			}
			try
			{
				this._context.Courts.Add(entity);
				this._context.SaveChanges();
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}

		public void Update(CourtEntity entity)
		{
			if (entity == null)
			{
				throw new NullReferenceException("The court doesn't exist in the database!");
			}
			try
			{
				this._context.SaveChanges();
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}

		public void Delete(CourtEntity entity)
		{
			CourtEntity entityToDelete = this._context.Courts
				.Include(include => include.WorkingHours)
				.FirstOrDefault(en => en == entity);

			if (entityToDelete == null)
			{
				throw new NullReferenceException("The court doesn't exist in the database!");
			}
			try
			{				
				this._context.Courts.Remove(entityToDelete);
				this._context.SaveChanges();
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}

		public void DeleteById(object id)
		{
			try
			{
				CourtEntity entityToDelete = this._context.Courts
					.Include(include => include.WorkingHours)
					.FirstOrDefault(entity => entity.Id == (int)id);

				if (entityToDelete == null)
				{
					throw new NullReferenceException("The court doesn't exist in the database!");
				}
				this._context.Courts.Remove(entityToDelete);
				this._context.SaveChanges();
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}

		public ICollection<CourtEntity> GetAll()
		{
			return _context.Courts.ToList();
		}

		public IQueryable<CourtEntity> GetWithInclude(Expression<Func<CourtEntity, bool>> predicate, params Expression<Func<CourtEntity, object>>[] include)
		{
			try
			{
				IQueryable<CourtEntity> query = this._context.Courts;
			query = include.Aggregate(query, (current, inc) => current.Include(inc));
			return query.Where(predicate);
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}

		public IQueryable<CourtEntity> Include(params Expression<Func<CourtEntity, object>>[] include)
		{
			try
			{
				IQueryable<CourtEntity> query = this._context.Courts;
				return include.Aggregate(query, (current, inc) => current.Include(inc));
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}
	}
}
