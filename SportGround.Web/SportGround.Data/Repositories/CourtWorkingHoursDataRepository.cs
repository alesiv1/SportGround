using SportGround.Data.Context;
using SportGround.Data.Entities;
using SportGround.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportGround.Data.Repositories
{
	public class CourtWorkingHoursDataRepository : IDataRepository<CourtWorkingHoursEntity>
	{
		private readonly DataContext _context;

		public CourtWorkingHoursDataRepository(DataContext context)
		{
			this._context = context;
		}

		public CourtWorkingHoursEntity GetById(object id)
		{
			try
			{
				return this._context.CourtWorkingHours.Find(id);
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("The court working hours doesn't exist in the database!");
			}
		}

		public void Insert(CourtWorkingHoursEntity entity)
		{
			if (entity == null)
			{
				throw new NullReferenceException("The court working hours doesn't exist in the database!");
			}
			try
			{
				this._context.CourtWorkingHours.Add(entity);
				this._context.SaveChanges();
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			};
		}

		public void Update(CourtWorkingHoursEntity entity)
		{
			if (entity == null)
			{
				throw new NullReferenceException("The court working hours doesn't exist in the database!");
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

		public void Delete(CourtWorkingHoursEntity entity)
		{
			if (entity == null)
			{
				throw new NullReferenceException("The court working hours doesn't exist in the database!");
			}
			try
			{
				this._context.CourtWorkingHours.Remove(entity);
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
				CourtWorkingHoursEntity entityToDelete = this._context.CourtWorkingHours.Find(id);

				if (entityToDelete == null)
				{
					throw new NullReferenceException("The court working hurs doesn't exist in the database!");
				}

				this._context.CourtWorkingHours.Remove(entityToDelete);
				this._context.SaveChanges();
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("An error occurred while requesting!");
			}
		}

		public ICollection<CourtWorkingHoursEntity> GetAll()
		{
			return _context.CourtWorkingHours.ToList();
		}
	}
}
