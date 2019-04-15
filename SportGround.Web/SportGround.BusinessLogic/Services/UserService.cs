using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.Interfaces;
using System.IO;
using System.Security.Cryptography;
using SportGround.Data.entities;
using SportGround.Data.Enums;

namespace SportGround.BusinessLogic.Operations
{
	public class UserService : IUserService
	{
		private readonly string ProjectKey = "SportGround";
		private IUserRepository _userRepository;

		public UserService(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		public void Create(UserModelWithPassword model)
		{
			if (UserExists(model.Email))
			{
				throw new ArgumentException("User already exist with this email {0} .", model.Email);
			}
			if(string.IsNullOrEmpty(model.Password))
			{
				throw new ArgumentException("User email can't be empty.");
			}
			var salt = CreateSaltForPasscode(); 
			var passcode = GetCodeForPassword(model.Password, salt);
			_userRepository.Add(model.FirstName, model.LastName, model.Email, UserRole.User, passcode, salt);
		}

		public void Delete(int id)
		{
			if (!UserExists(id))
			{
				throw new ArgumentException("User doesn't exist!");
			}
			_userRepository.Delete(id);
		}

		public List<UserModelWithRole> GetUserList()
		{
			var userList = new List<UserModelWithRole>();
			var users = _userRepository.GetUsers();
			foreach (var user in users)
			{
				userList.Add(new UserModelWithRole()
				{
					Id = user.Id,
					FirstName = user.FirstName,
					LastName = user.LastName,
					Email = user.Email,
					Role = user.Role,
				});
			}
			return userList;
		}

		public UserModelWithRole GetUserById(int id)
		{
			var user = _userRepository.GetUserById(id);
			if (user == null)
			{
				throw new ArgumentException("User doesn't exist!");
			}
			return new UserModelWithRole()
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Role = user.Role,
				Email = user.Email
			};
		}

		public UserEntity GetUserByEmail(string email)
		{
			var user = _userRepository
				.GetUsers()
				.FirstOrDefault(us => us.Email == email);
			if (user == null)
			{
				throw new ArgumentException("User doesn't exist!");
			}

			return user;
		}

		public UserModelWithPassword GetUserWithPassword(int id)
		{
			var user = _userRepository.GetUserById(id);
			if (user == null)
			{
				throw new ArgumentException("User doesn't exist!");
			}
			return new UserModelWithPassword()
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Email = user.Email,
				Password = GetDecodePassword(user.Password, user.Salt),
				ConfirmPassword = ""
			};
		}

		public bool UserExists(string email)
		{
			return _userRepository.UserExists(email);
		}

		public bool UserExists(int id)
		{
			return _userRepository.UserExists(id);
		}

		public void Update(int id, UserModel model)
		{
			if (!UserExists(id))
			{
				throw new ArgumentException("User doesn't exist!");
			}
			_userRepository.Update(id, model.FirstName, model.LastName, model.Email);
		}

		public void Update(int id, UserModelWithRole model)
		{
			if (!UserExists(id))
			{
				throw new ArgumentException("User doesn't exist!");
			}
			_userRepository.Update(id, model.Role);
		}

		public void Update(int id, UserModelWithPassword model)
		{
			if (!UserExists(id))
			{
				throw new ArgumentException("User doesn't exist!");
			}
			_userRepository.Update(id, model.Password);
		}

		public string GetPasswordHashCode(string password, string salt)
		{
			return GetCodeForPassword(password, salt);
		}

		public string GetDecodePassword(string password, string salt)
		{
			return GetPasswordByDecode(password, salt);
		}

		private string CreateSaltForPasscode()
		{
			var random = new RNGCryptoServiceProvider();
			byte[] salt = new byte[40];
			random.GetNonZeroBytes(salt);
			var saltInString = Convert.ToBase64String(salt);
			return saltInString;
		}

		private byte[] GetSaltForPasscode(string salt)
		{
			return Convert.FromBase64String(salt);
		}

		private string GetCodeForPassword(string password, string salt)
		{
			string EncryptionKey = ProjectKey;
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

		private string GetPasswordByDecode(string password, string salt)
		{
			string EncryptionKey = ProjectKey;
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

