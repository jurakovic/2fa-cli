using System;
using System.IO;
using System.Security.Cryptography;

namespace _2fa
{
	public class Aes
	{
		public static string EncryptString(string password, string text)
		{
			byte[] key, iv;
			using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 16, 10000, HashAlgorithmName.SHA256))
			{
				key = rfc2898DeriveBytes.GetBytes(32);
				iv = rfc2898DeriveBytes.Salt;
			}

			string encryptedText = EncryptString(text, key, iv);

			return $"{Convert.ToBase64String(iv)}:{encryptedText}";
		}

		private static string EncryptString(string text, byte[] key, byte[] iv)
		{
			using (var aes = System.Security.Cryptography.Aes.Create())
			{
				aes.Key = key;
				aes.IV = iv;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;

				using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
				using (var ms = new MemoryStream())
				using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
				using (var writer = new StreamWriter(cs))
				{
					writer.Write(text);
					writer.Flush();
					cs.FlushFinalBlock();
					return Convert.ToBase64String(ms.ToArray());
				}
			}
		}

		public static string DecryptString(string password, string text)
		{
			string[] parts = text.Split(':');
			byte[] iv = Convert.FromBase64String(parts[0]);
			string encryptedText = parts[1];

			byte[] key;
			using (var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, iv, 10000, HashAlgorithmName.SHA256))
			{
				key = rfc2898DeriveBytes.GetBytes(32);
			}

			return DecryptString(encryptedText, key, iv);
		}

		public static string DecryptString(string encryptedText, byte[] key, byte[] iv)
		{
			using (var aes = System.Security.Cryptography.Aes.Create())
			{
				aes.Key = key;
				aes.IV = iv;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;

				using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
				using (var ms = new MemoryStream(Convert.FromBase64String(encryptedText)))
				using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
				using (var reader = new StreamReader(cs))
				{
					return reader.ReadToEnd();
				}
			}
		}
	}
}
