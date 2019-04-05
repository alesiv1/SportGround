using SportGround.Data.Context;
using SportGround.Data.entities;
using SportGround.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SportGround.Data.Repositories
{
	public class UserDataRepository : IDataRepository<UserEntity>
	{
		private readonly DataContext _context;

		public UserDataRepository(DataContext context)
		{
			this._context = context;
		}

		public UserEntity GetById(object id)
		{
			try
			{
				return this._context.Users.Find(id);
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("The user doesn't exist in the database!");
			}
		}

		public void Insert(UserEntity entity)
		{
			if (entity == null)
			{
				throw new NullReferenceException("The user doesn't exist in the database!");
			}
			try
			{
				this._context.Users.Add(entity);
				this._context.SaveChanges();
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}

		public void Update(UserEntity entity)
		{
			if (entity == null)
			{
				throw new NullReferenceException("The user doesn't exist in the database!");
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

		public void Delete(UserEntity entity)
		{
			UserEntity entityToDelete = this._context.Users
				.Include(include => include.BookingCourt)
				.FirstOrDefault(en => en == entity);

			if (entityToDelete == null)
			{
				throw new NullReferenceException("The user doesn't exist in the database!");
			}
			try
			{
				this._context.Users.Remove(entityToDelete);
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
				UserEntity entityToDelete = this._context.Users
					.Include(include => include.BookingCourt)
					.FirstOrDefault(entity => entity.Id == (int)id);

				if (entityToDelete == null)
				{
					throw new NullReferenceException("The user doesn't exist in the database!");
				}

				this._context.Users.Remove(entityToDelete);
				this._context.SaveChanges();
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}

		public ICollection<UserEntity> GetAll()
		{
			return _context.Users.ToList();
		}

		public IQueryable<UserEntity> GetWithInclude(Expression<Func<UserEntity, bool>> predicate, params Expression<Func<UserEntity, object>>[] include)
		{
			try
			{
				IQueryable<UserEntity> query = this._context.Users;
				query = include.Aggregate(query, (current, inc) => current.Include(inc));
				return query.Where(predicate);
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}

		public IQueryable<UserEntity> Include(params Expression<Func<UserEntity, object>>[] include)
		{
			try
			{
				IQueryable<UserEntity> query = this._context.Users;
				return include.Aggregate(query, (current, inc) => current.Include(inc));
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}
	}
}
