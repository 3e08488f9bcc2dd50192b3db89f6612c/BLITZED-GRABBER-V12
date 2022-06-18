using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;

// Token: 0x02000001 RID: 1
internal class <Module>
{
	// Token: 0x06000001 RID: 1 RVA: 0x000026A8 File Offset: 0x000008A8
	static <Module>()
	{
		<Module>.SetupResources();
	}

	// Token: 0x06000002 RID: 2 RVA: 0x000026AF File Offset: 0x000008AF
	public static void SetupResources()
	{
		AppDomain.CurrentDomain.AssemblyResolve += <Module>.CurrentDomain_AssemblyResolve;
	}

	// Token: 0x06000003 RID: 3 RVA: 0x0000294C File Offset: 0x00000B4C
	private static Assembly CurrentDomain_AssemblyResolve(object A_0, ResolveEventArgs A_1)
	{
		Assembly result;
		try
		{
			string text = A_1.Name.Contains(',') ? A_1.Name.Substring(0, A_1.Name.IndexOf(',')) : A_1.Name.Replace(".dll", "");
			bool flag = text.EndsWith("_resources");
			if (flag)
			{
				result = null;
			}
			else
			{
				using (Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(text))
				{
					byte[] array = new byte[manifestResourceStream.Length];
					manifestResourceStream.Read(array, 0, array.Length);
					result = Assembly.Load(<Module>.Decompress(array));
				}
			}
		}
		catch
		{
			result = null;
		}
		return result;
	}

	// Token: 0x06000004 RID: 4 RVA: 0x00002A78 File Offset: 0x00000C78
	public static byte[] Decompress(byte[] A_0)
	{
		MemoryStream stream = new MemoryStream(A_0);
		MemoryStream memoryStream = new MemoryStream();
		using (DeflateStream deflateStream = new DeflateStream(stream, CompressionMode.Decompress))
		{
			deflateStream.CopyTo(memoryStream);
		}
		return memoryStream.ToArray();
	}
}
