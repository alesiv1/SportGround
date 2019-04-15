using System;
using SportGround.Data.Context;
using SportGround.Data.entities;
using SportGround.Data.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using SportGround.Data.Enums;

namespace SportGround.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly DataContext _context;

		public UserRepository(DataContext context)
		{
			this._context = context;
		}

		public void Add(string firstName, string lastName, string email, UserRole role, string password, string salt)
		{
			UserEntity user = new UserEntity()
			{
				FirstName = firstName,
				LastName = lastName,
				Email = email,
				Role = role,
				Password = password,
				Salt = salt
			};

			_context.Users.Add(user);
			_context.SaveChanges();
		}

		public void Delete(int id)
		{
			var user = _context
				.Users
				.Include(booking => booking.BookingCourts)
				.FirstOrDefault(us => us.Id == id);
			_context.Users.Remove(user);
			_context.SaveChanges();
		}

		public void Update(int id, string firstName, string lastName, string email)
		{
			var user = _context.Users.Find(id);
			user.FirstName = firstName;
			user.LastName = lastName;
			user.Email = email;
			_context.SaveChanges();
		}

		public void Update(int id, UserRole role)
		{
			var user = _context.Users.Find(id);
			user.Role = role;
			_context.SaveChanges();
		}

		public void Update(int id, string password)
		{
			var user = _context.Users.Find(id);
			user.Password = password;
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
