using System;
using SportGround.Data.entities;
using System.Collections.Generic;

namespace SportGround.Data.Interfaces
{
	public interface IUserRepository
	{
		void Add(UserEntity user);
		void Delete(int id);
		void Delete(UserEntity user);
		void Update(UserEntity user);
		ICollection<UserEntity> GetUsers();
		UserEntity GetUserById(int id);
		bool UserExists(string email);
		bool UserExists(int id);
	}
}
