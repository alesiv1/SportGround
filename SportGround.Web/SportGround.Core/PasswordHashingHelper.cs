﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SportGround.Core
{
	public class PasswordHashingHelper
	{
		private const string ProjectKey = "SportGround";

		public string CreateSaltForPasscode()
		{
			var random = new RNGCryptoServiceProvider();
			var salt = new byte[40];
			random.GetNonZeroBytes(salt);
			var saltInString = Convert.ToBase64String(salt);
			return saltInString;
		}

		public byte[] GetSaltForPasscode(string salt)
		{
			return Convert.FromBase64String(salt);
		}

		public string GetCodeForPassword(string password, string salt)
		{
			var EncryptionKey = ProjectKey;
			var clearBytes = Encoding.Unicode.GetBytes(password);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, GetSaltForPasscode(salt));
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					password = Convert.ToBase64String(ms.ToArray());
				}
			}
			return password;
		}

		public string GetPasswordByDecode(string hash, string salt)
		{
			var EncryptionKey = ProjectKey;
			var password = hash.Replace(" ", "+");
			var cipherBytes = Convert.FromBase64String(password);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, GetSaltForPasscode(salt));
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cipherBytes, 0, cipherBytes.Length);
						cs.Close();
					}
					password = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return password;
		}
	}
}
