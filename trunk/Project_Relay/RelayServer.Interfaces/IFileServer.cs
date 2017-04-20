using System;
using System.IO;
using System.ServiceModel;

namespace RelayServer.Interfaces
{
	[ServiceContract]
	internal interface IFileServer
	{
		[OperationContract]
		System.IO.Stream DownloadUpdate();

		[OperationContract]
		System.IO.Stream DownloadSettings();
	}
}
