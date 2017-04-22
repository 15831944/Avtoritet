using System;
using System.IO;
using System.ServiceModel;

namespace RelayServer.Interfaces
{
	[ServiceContract]
	internal interface IFileTransferProcessor
	{
		[OperationContract]
		System.IO.Stream DownloadSettings();
	}
}
