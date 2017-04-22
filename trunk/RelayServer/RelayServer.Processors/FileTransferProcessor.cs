using RelayServer.Interfaces;
using System;
using System.IO;
using System.ServiceModel;

namespace RelayServer.Processors
{
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, AddressFilterMode = AddressFilterMode.Any)]
	public class FileTransferProcessor : IFileTransferProcessor
	{
		public System.IO.Stream DownloadSettings()
		{
			return null;
		}
	}
}
