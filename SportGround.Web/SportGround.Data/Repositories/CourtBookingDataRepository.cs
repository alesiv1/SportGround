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
	public class CourtBookingDataRepository : IDataRepository<CourtBookingEntity>
	{
		private readonly DataContext _context;

		public CourtBookingDataRepository(DataContext context)
		{
			this._context = context;
		}

		public void Delete(CourtBookingEntity entity)
		{
			if (entity == null)
			{
				throw new NullReferenceException("The model isn't correct!");
			}
			try
			{
				this._context.BookingCourt.Remove(entity);
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
				CourtBookingEntity entityToDelete = this._context.BookingCourt.Find(id);

				if (entityToDelete == null)
				{
					throw new NullReferenceException("The model isn't valid!");
				}
				this._context.BookingCourt.Remove(entityToDelete);
				this._context.SaveChanges();
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}

		public ICollection<CourtBookingEntity> GetAll()
		{
			return _context.BookingCourt.ToList();
		}

		public CourtBookingEntity GetById(object id)
		{
			try
			{
				return this._context.BookingCourt.Find(id);
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("The model isn't valid!");
			}
		}

		public void Insert(CourtBookingEntity entity)
		{
			if (entity == null)
			{
				throw new NullReferenceException("The model isn't valid!");
			}
			try
			{
				this._context.BookingCourt.Add(entity);
				this._context.SaveChanges();
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}

		public void Update(CourtBookingEntity entity)
		{
			if (entity == null)
			{
				throw new NullReferenceException("The model isn't valid!");
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

		public IQueryable<CourtBookingEntity> GetWithInclude(Expression<Func<CourtBookingEntity, bool>> predicate, params Expression<Func<CourtBookingEntity, object>>[] include)
		{
			try
			{
				IQueryable<CourtBookingEntity> query = this._context.BookingCourt;
				query = include.Aggregate(query, (current, inc) => current.Include(inc));
				return query.Where(predicate);
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}

		public IQueryable<CourtBookingEntity> Include(params Expression<Func<CourtBookingEntity, object>>[] include)
		{
			try
			{
				IQueryable<CourtBookingEntity> query = this._context.BookingCourt;
				return include.Aggregate(query, (current, inc) => current.Include(inc));
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}
	}
}
