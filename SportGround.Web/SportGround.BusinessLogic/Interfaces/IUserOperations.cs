using System;
using System.Collections.Generic;
using SportGround.BusinessLogic.Models;
using SportGround.Data.entities;

namespace SportGround.BusinessLogic.Interfaces
{
	public interface IUserOperations
	{
		UserModelWithRole GetUserById(int id);
		List<UserModelWithRole> GetAll();
		List<UserEntity> Users();
		void Create(UserModelWithPassword model);
		void Update(int id, UserModel model);
		void Update(int id, UserModelWithRole model);
		void Update(int id, UserModelWithPassword model);
		void Delete(int id);
		string GetPasswordHashCode(string password, string salt);
		string GetDecodePassword(string password, string salt);
	}
}
