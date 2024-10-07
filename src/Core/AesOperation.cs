using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace _2fa
{
	public class AesOperation
	{
		// src: https://www.c-sharpcorner.com/article/encryption-and-decryption-using-a-symmetric-key-in-c-sharp/

		public static string EncryptString(string key, string plainText)
		{
			byte[] iv = new byte[16];
			byte[] array;

			using (Aes aes = Aes.Create())
			{
				//aes.Key = Encoding.UTF8.GetBytes(key);
				aes.Key = DeriveKeyFromPassword(key);
				aes.IV = iv;

				ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
						{
							streamWriter.Write(plainText);
						}

						array = memoryStream.ToArray();
					}
				}
			}

			return Convert.ToBase64String(array);
		}

		public static string DecryptString(string key, string cipherText)
		{
			byte[] iv = new byte[16];
			byte[] buffer = Convert.FromBase64String(cipherText);

			using (Aes aes = Aes.Create())
			{
				//aes.Key = Encoding.UTF8.GetBytes(key);
				aes.Key = DeriveKeyFromPassword(key);
				aes.IV = iv;
				ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

				using (MemoryStream memoryStream = new MemoryStream(buffer))
				{
					using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
						{
							return streamReader.ReadToEnd();
						}
					}
				}
			}
		}

		private static byte[] DeriveKeyFromPassword(string password)
		{
			// src: https://code-maze.com/csharp-string-encryption-decryption/
			var emptySalt = Array.Empty<byte>();
			var iterations = 1000;
			var desiredKeyLength = 16; // 16 bytes equal 128 bits.
			var hashMethod = HashAlgorithmName.SHA384;
			return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password), emptySalt, iterations, hashMethod, desiredKeyLength);
		}
	}
}
