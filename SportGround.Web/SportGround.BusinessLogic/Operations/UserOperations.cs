using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportGround.BusinessLogic.Enums;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.entities;
using SportGround.Data.Interfaces;
using System.IO;
using System.Reflection.Metadata;
using System.Security.Cryptography;

namespace SportGround.BusinessLogic.Operations
{
	public class UserOperations : IUserOperations
	{
		private IDataRepository<UserEntity> _userData;
		private readonly string ProjectKey = "SportGround";

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

		public string GetPasswordHashCode(string password, string encryptionKey = null)
		{
			string EncryptionKey = encryptionKey ?? ProjectKey;
			byte[] clearBytes = Encoding.Unicode.GetBytes(password);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				try
				{
					using (MemoryStream ms = new MemoryStream())
					{
						using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(),
							CryptoStreamMode.Write))
						{
							cs.Write(clearBytes, 0, clearBytes.Length);
							cs.Close();
						}

						password = Convert.ToBase64String(ms.ToArray());
					}
				}
				catch
				{
					password = "";
				}
			}
			return password;
		}

		public string GetDecodePassword(string passwordHashCode, string encryptionKey = null)
		{
			string EncryptionKey = encryptionKey ?? ProjectKey;
			passwordHashCode = passwordHashCode.Replace(" ", "+");
			byte[] cipherBytes = Convert.FromBase64String(passwordHashCode);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				try
				{
					using (MemoryStream ms = new MemoryStream())
					{
						using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(),
							CryptoStreamMode.Write))
						{
							cs.Write(cipherBytes, 0, cipherBytes.Length);
							cs.Close();
						}

						passwordHashCode = Encoding.Unicode.GetString(ms.ToArray());
					}
				}
				catch
				{
					passwordHashCode = "";
				}
			}
			return passwordHashCode;
		}
	}
}

