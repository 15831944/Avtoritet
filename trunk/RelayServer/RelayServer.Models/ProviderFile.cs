using System;

namespace RelayServer.Models
{
	public class ProviderFile
	{
		public string FileName
		{
			get;
			set;
		}

		public string FileExt
		{
			get;
			set;
		}

		public long FileSize
		{
			get;
			set;
		}

		public byte[] FileContent
		{
			get;
			set;
		}
	}
}
