using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace RelayServer.Helpers
{
	public static class StringZip
	{
		public static string Zip(string text)
		{
			byte[] buffer = System.Text.Encoding.Unicode.GetBytes(text);
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
			{
				zip.Write(buffer, 0, buffer.Length);
			}
			ms.Position = 0L;
			System.IO.MemoryStream outStream = new System.IO.MemoryStream();
			byte[] compressed = new byte[ms.Length];
			ms.Read(compressed, 0, compressed.Length);
			byte[] gzBuffer = new byte[compressed.Length + 4];
			System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
			System.Buffer.BlockCopy(System.BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
			return System.Convert.ToBase64String(gzBuffer);
		}

		public static string UnZip(string compressedText)
		{
			byte[] gzBuffer = System.Convert.FromBase64String(compressedText);
			string @string;
			using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
			{
				int msgLength = System.BitConverter.ToInt32(gzBuffer, 0);
				ms.Write(gzBuffer, 4, gzBuffer.Length - 4);
				byte[] buffer = new byte[msgLength];
				ms.Position = 0L;
				using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
				{
					zip.Read(buffer, 0, buffer.Length);
				}
				@string = System.Text.Encoding.Unicode.GetString(buffer, 0, buffer.Length);
			}
			return @string;
		}
	}
}
