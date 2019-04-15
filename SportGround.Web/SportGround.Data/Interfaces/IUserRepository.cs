using System;
using SportGround.Data.entities;
using System.Collections.Generic;
using SportGround.Data.Enums;

namespace SportGround.Data.Interfaces
{
	public interface IUserRepository
	{
		void Add(string firstName, string lastName, string email, UserRole role, string password, string salt);
		void Delete(int id);
		void Update(int id, string firstName, string lastName, string email);
		void Update(int id, string password);
		void Update(int id, UserRole role);
		ICollection<UserEntity> GetUsers();
		UserEntity GetUserById(int id);
		bool UserExists(string email);
		bool UserExists(int id);
	}
}
