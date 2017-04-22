namespace NewLauncher.Helper
{
    using System;
    using System.IO;
    using System.IO.Compression;
    using System.Text;

    public static class StringZip
    {
        public static string UnZip(string compressedText)
        {
            byte[] buffer = Convert.FromBase64String(compressedText);
            using (MemoryStream stream = new MemoryStream())
            {
                int num = BitConverter.ToInt32(buffer, 0);
                stream.Write(buffer, 4, buffer.Length - 4);
                byte[] buffer2 = new byte[num];
                stream.Position = 0L;
                using (GZipStream stream2 = new GZipStream(stream, CompressionMode.Decompress))
                {
                    stream2.Read(buffer2, 0, buffer2.Length);
                }
                return Encoding.Unicode.GetString(buffer2, 0, buffer2.Length);
            }
        }

        public static string Zip(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);
            MemoryStream stream = new MemoryStream();
            using (GZipStream stream2 = new GZipStream(stream, CompressionMode.Compress, true))
            {
                stream2.Write(bytes, 0, bytes.Length);
            }
            stream.Position = 0L;
            MemoryStream stream3 = new MemoryStream();
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            byte[] dst = new byte[buffer.Length + 4];
            Buffer.BlockCopy(buffer, 0, dst, 4, buffer.Length);
            Buffer.BlockCopy(BitConverter.GetBytes(bytes.Length), 0, dst, 0, 4);
            return Convert.ToBase64String(dst);
        }
    }
}

