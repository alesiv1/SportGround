using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using SportGround.BusinessLogic.Enums;
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
				Role = model.Role.ToString(),
				Password = model.Password,
				Email = model.Email
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
						Role = user.Role == "Admin" ? UserRole.Admin : UserRole.User,
						Password = user.Password,
						Email = user.Email
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
				Role = userEntity.Role == "Admin" ? UserRole.Admin : UserRole.User,
				Password = userEntity.Password,
				Email = userEntity.Email
			};
		}

		public void Update(int id, UserModel model)
		{
			var user = _userData.GetById(id);
			user.FirstName = model.FirstName ?? user.FirstName;
			user.LastName = model.LastName ?? user.LastName;
			user.Email = model.Email ?? user.Email;
			user.Role = String.IsNullOrEmpty(model.Role.ToString()) ? user.Role : model.Role.ToString();
			user.Password = model.Password ?? user.Password;
			_userData.Update(user);
		}

		public string GetPasswordHashCode(int id, string password)
		{
			var hash = "";
			foreach (var symbol in password)
			{
				hash += ((int)symbol + (id * id)).ToString() + ".";
			}

			hash += id;
			return hash;
		}

		public string GetDecodePassword(string passwordHashCode)
		{
			if (!passwordHashCode.Contains("."))
			{
				return passwordHashCode;
			}

			var allsymbols = passwordHashCode.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
			var id = Int16.Parse(allsymbols[allsymbols.Length - 1]);
			var password = "";
			for (int i = 0; i < allsymbols.Length - 1; i++)
			{
				password += ((char)(Int16.Parse(allsymbols[i]) - (id * id))).ToString();
			}
			return password;
		}
	}
}

