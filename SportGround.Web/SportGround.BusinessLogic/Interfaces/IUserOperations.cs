using System;
using System.Collections.Generic;
using System.Security.Policy;
using SportGround.BusinessLogic.Models;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface IUserOperations
	{
		UserModel GetUserById(int id);
		List<UserModel> GetAll();
		void Create(UserModel model);
		void Update(int id, UserModel model);
		void Delete(int id);
		string GetPasswordHashCode(string password, string encryptionKey = null);
		string GetDecodePassword(string passwordHashCode, string encryptionKey = null);
	}
}
