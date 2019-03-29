using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.entities;
using SportGround.Data.Interfaces;
using System.IO;
using System.Security.Cryptography;
using SportGround.Data.Enums;

namespace SportGround.BusinessLogic.Operations
{
	public class UserOperations : IUserOperations
	{
		private readonly string ProjectKey = "SportGround";
		private IDataRepository<UserEntity> _userRepository;

		public UserOperations(IDataRepository<UserEntity> userRepository)
		{
			_userRepository = userRepository;
		}

		public void Create(UserModelWithPassword model)
		{
			if (UserAlreadyExist(model.Email))
			{
				throw new ArgumentException("User already exist with this email -> {0} ", model.Email);
			}

			var salt = CreateSaltForPasscode();
			var passcode = GetCodeForPassword(model.Password, salt, model.Id);

			UserEntity user = new UserEntity()
			{
				Id = model.Id,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Email = model.Email,
				Role = UserRole.User,
				Password = passcode,
				Salt = salt
			};

			_userRepository.Insert(user);
		}

		public void Delete(int id)
		{
			_userRepository.DeleteById(id);
		}

		public List<UserModelWithRole> GetAll()
		{
			var allUsers = new List<UserModelWithRole>();
			var query = _userRepository.GetAll();
			foreach (var user in query)
			{
				allUsers.Add(new UserModelWithRole()
				{
					Id = user.Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email,
					Role = user.Role,
				});
			}

			return allUsers;
		}

		public UserModelWithRole GetUserById(int id)
		{
			var userEntity = _userRepository.GetById(id);

			return new UserModelWithRole()
			{
				Id = userEntity.Id,
				FirstName = userEntity.FirstName,
				LastName = userEntity.LastName,
				Role = userEntity.Role,
				Email = userEntity.Email
			};
		}

		public void Update(int id, UserModel model)
		{
			var user = _userRepository.GetById(id);
			user.FirstName = model.FirstName;
			user.LastName = model.LastName;
			user.Email = model.Email;
			_userRepository.Update(user);
		}

		public void Update(int id, UserModelWithRole model)
		{
			var user = _userRepository.GetById(id);
			user.FirstName = model.FirstName;
			user.LastName = model.LastName;
			user.Email = model.Email;
			user.Role = user.Role;
			_userRepository.Update(user);
		}

		public void Update(int id, UserModelWithPassword model)
		{
			var user = _userRepository.GetById(id);
			user.FirstName = model.FirstName;
			user.LastName = model.LastName;
			user.Email = model.Email;
			user.Password = model.Password;
			_userRepository.Update(user);
		}

		public string GetPasswordHashCode(string password, string salt, int id)
		{
			return GetCodeForPassword(password, salt, id);
		}

		public string GetDecodePassword(string password, string salt, int id)
		{
			return GetPasswordByDecode(password, salt, id);
		}

		private bool UserAlreadyExist(string email)
		{
			return _userRepository.GetAll().Any(em => em.Email == email);
		}

		private string CreateSaltForPasscode()
		{
			var random = new RNGCryptoServiceProvider();
			byte[] salt = new byte[40];
			random.GetNonZeroBytes(salt);
			var s = Convert.ToBase64String(salt);
			return s;
		}

		private byte[] GetSaltForPasscode(string salt)
		{
			return Convert.FromBase64String(salt);
		}

		private string GetCodeForPassword(string password, string salt, int id)
		{
			string EncryptionKey = ProjectKey + id;
			byte[] clearBytes = Encoding.Unicode.GetBytes(password);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, GetSaltForPasscode(salt));
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
					password = null;
				}
			}

			return password;
		}

		private string GetPasswordByDecode(string password, string salt, int id)
		{
			string EncryptionKey = ProjectKey + id;
			password = password.Replace(" ", "+");
			byte[] cipherBytes = Convert.FromBase64String(password);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, GetSaltForPasscode(salt));
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

						password = Encoding.Unicode.GetString(ms.ToArray());
					}
				}
				catch
				{
					password = null;
				}
			}

			return password;
		}
	}
}

