using System;
using System.Collections.Generic;
using SportGround.BusinessLogic.Models;
using SportGround.Data.entities;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface IUserService
	{
		UserModelWithRole GetUserById(int id);
		UserEntity GetUserByEmail(string email);
		UserEntity GetUserEntity(int id);
		List<UserModelWithRole> GetUserList();
		void Create(UserModelWithPassword model);
		void Update(int id, UserModel model);
		void Update(int id, UserModelWithRole model);
		void Update(int id, UserModelWithPassword model);
		void Delete(int id);
		bool UserExists(string email);
		bool UserExists(int id);
		string GetPasswordHashCode(string password, string salt);
		string GetDecodePassword(string password, string salt);
	}
}
