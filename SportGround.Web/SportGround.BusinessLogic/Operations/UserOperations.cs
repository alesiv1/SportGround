using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.entities;
using SportGround.Data.Interfaces;

namespace SportGround.BusinessLogic.Operations
{
	public class UserOperations : IUserOperations
	{
		private IDataRepository<UserEntity> _userData;

		public UserOperations(IDataRepository<UserEntity> courtRepository)
		{
			_userData = courtRepository;
		}

		public void Create(UserModel model)
		{
			UserEntity user = new UserEntity()
			{
				Id = model.Id,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Role = model.Role
			};
			_userData.Insert(user);
		}

		public void Delete(int id)
		{
			_userData.DeleteById(id);
		}

		public List<UserModel> GetAll()
		{
			var allUsers = new List<UserModel>();
			try
			{
				var query = _userData.Get.ToList();
				foreach (var user in query)
				{
					allUsers.Add(new UserModel()
					{
						Id = user.Id,
						FirstName = user.FirstName,
						LastName = user.LastName,
						Role = user.Role
					});
				}
			}
			catch (Exception ex)
			{
				throw;
			}
			return allUsers;
		}

		public UserModel GetUserById(int id)
		{
			var userEntity = _userData.GetById(id);
			return new UserModel()
			{
				Id = userEntity.Id,
				FirstName = userEntity.FirstName,
				LastName = userEntity.LastName,
				Role = userEntity.Role
			};
		}

		public void Update(int id, UserModel model)
		{
			var user = _userData.GetById(id);
			user.FirstName = model.FirstName;
			user.LastName = model.LastName;
			user.Role = model.Role;
			_userData.Update(user);
		}
	}
}
