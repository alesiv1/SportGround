using System;
using SportGround.Data.Context;
using SportGround.Data.entities;
using SportGround.Data.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SportGround.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext _context;

		public UserRepository(DataContext context)
		{
			this._context = context;
		}

		public void Add(UserEntity user)
		{
			_context.Users.Add(user);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var user = _context
				.Users
				.Include(booking => booking.BookingCourts)
				.FirstOrDefault(x => x.Id  == id);
			_context.Users.Remove(user);
			_context.SaveChanges();
		}

		public void Delete(UserEntity user)
		{			
			_context.Users.Remove(user);
			_context.SaveChanges();
		}

		public void Update(UserEntity user)
		{
			_context.SaveChanges();
		}

		public ICollection<UserEntity> GetUsers()
		{
			return _context.Users.ToList();

		}

		public UserEntity GetUserById(int id)
		{
			return _context.Users.Find(id);
		}

		public bool UserExists(string email)
		{
			return _context.Users.Any(user => user.Email.ToLower() == email.ToLower());
		}

		public bool UserExists(int id)
		{
			return _context.Users.Any(user => user.Id == id);
		}
	}
}
