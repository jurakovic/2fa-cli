using OtpNet;

namespace _2fa
{
	internal static class EntryHelper
	{
		public static bool IsValidSecret(string secretKey)
		{
			if (!IsValidBase32(secretKey))
				return false;

			if (secretKey.Length < 16)
				return false;

			try
			{
				byte[] keyBytes = Base32Encoding.ToBytes(secretKey);
				var totp = new Totp(keyBytes);
				totp.ComputeTotp();
				return true;
			}
			catch
			{
				return false;
			}
		}

		static bool IsValidBase32(string base32String)
		{
			try
			{
				_ = Base32Encoding.ToBytes(base32String);
				return true;
			}
			catch
			{
				return false;
			}
		}
	}
}
