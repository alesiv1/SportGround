using System;
using System.Collections.Generic;
using System.Linq;
using SportGround.BusinessLogic.Interfaces;
using SportGround.BusinessLogic.Models;
using SportGround.Data.Interfaces;
using SportGround.Data.entities;
using SportGround.Data.Enums;
using SportGround.Core;

namespace SportGround.BusinessLogic.Operations
{
	public class UserService : IUserService
	{
		private IUserRepository _userRepository;
		private ICourtBookingRepository _bookingRepository;
		private PasswordHashingHelper PasswordHashHelper = new PasswordHashingHelper();

		public UserService(IUserRepository userRepository, ICourtBookingRepository bookingRepository)
		{
			_userRepository = userRepository;
			_bookingRepository = bookingRepository;
		}

		public void Create(UserModelWithPassword model)
		{
			var salt = PasswordHashHelper.CreateSaltForPasscode(); 
			var passcode = PasswordHashHelper.GetCodeForPassword(model.Password, salt);
			_userRepository.Add(model.FirstName, model.LastName, model.Email, UserRole.User, passcode, salt);
		}

		public void CreateDefaultUser()
		{
			var salt = PasswordHashHelper.CreateSaltForPasscode();
			var passcode = PasswordHashHelper.GetCodeForPassword("admin1", salt);
			_userRepository.Add("Admin", "Admin", "admin@admin.com", UserRole.Admin, passcode, salt);
		}

		public void Delete(int id)
		{
			_bookingRepository.DeleteRangeByUserId(id);
			_userRepository.Delete(id);
		}

		public IReadOnlyList<UserModelWithRole> GetUserList()
		{
			return _userRepository.GetUsers()
				.Select(user => new UserModelWithRole()
					{
						Id = user.Id,
						FirstName = user.FirstName,
						LastName = user.LastName,
						Email = user.Email,
						Role = user.Role,
					})
				.ToList();
		}

		public UserModelWithRole GetUserById(int id)
		{
			var user = _userRepository.GetUserById(id);
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
			var user = _userRepository.GetUserByEmail(email);
			return user;
		}

		public UserModelWithPassword GetUserWithPassword(int id)
		{
			var user = _userRepository.GetUserById(id);
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

		public void Update(int id, UserModel model)
		{
			_userRepository.Update(id, model.FirstName, model.LastName, model.Email);
		}

		public void Update(int id, UserModelWithRole model)
		{
			_userRepository.Update(id, model.Role);
		}

		public void Update(int id, UserModelWithPassword model)
		{
			var salt = _userRepository.GetUserById(id).Salt;
			var password = PasswordHashHelper.GetCodeForPassword(model.Password, salt);
			_userRepository.Update(id, password);
		}

		public string GetPasswordHashCode(string password, string salt)
		{
			return PasswordHashHelper.GetCodeForPassword(password, salt);
		}

		private string GetDecodePassword(string password, string salt)
		{
			return PasswordHashHelper.GetPasswordByDecode(password, salt);
		}
	}
}

