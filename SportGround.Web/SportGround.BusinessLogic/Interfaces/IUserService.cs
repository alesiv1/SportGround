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
		UserModelWithPassword GetUserWithPassword(int id);
		IReadOnlyList<UserModelWithRole> GetUserList();
		void Create(UserModelWithPassword model);
		void CreateDefaultUser();
		void Update(int id, UserModel model);
		void Update(int id, UserModelWithRole model);
		void Update(int id, UserModelWithPassword model);
		void Delete(int id);
		bool UserExists(string email);
		string GetPasswordHashCode(string password, string salt);
	}
}
