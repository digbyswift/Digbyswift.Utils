using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Digbyswift.Utils.Extensions
{
	public static class FileExtensions
	{

		[DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
		private extern static UInt32 FindMimeFromData(
			UInt32 pBc,
			[MarshalAs(UnmanagedType.LPStr)] String pwzUrl,
			[MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
			UInt32 cbSize,
			[MarshalAs(UnmanagedType.LPStr)] String pwzMimeProposed,
			UInt32 dwMimeFlags,
			out UInt32 ppwzMimeOut,
			UInt32 dwReserverd
		);

		public static string GetMimeFromFile(this FileInfo value)
		{
			if (!value.Exists)
				throw new FileNotFoundException(value.FullName + " not found");

			byte[] buffer = new byte[256];
			using (FileStream fs = value.OpenRead())
			{
				if (fs.Length >= 256)
					fs.Read(buffer, 0, 256);
				else
					fs.Read(buffer, 0, (int)fs.Length);
			}

			try
			{
				UInt32 mimetype;
				FindMimeFromData(0, null, buffer, 256, null, 0, out mimetype, 0);
				IntPtr mimeTypePtr = new IntPtr(mimetype);
				string mime = Marshal.PtrToStringUni(mimeTypePtr);
				Marshal.FreeCoTaskMem(mimeTypePtr);
				return mime;
			}
			catch
			{
				return "unknown/unknown";
			}
		}

	}
}