using System;
using System.CodeDom.Compiler;
using System.IO;
using System.ServiceModel;

namespace ServerHost.ServiceReference
{
	[GeneratedCode("System.ServiceModel", "4.0.0.0"), ServiceContract(ConfigurationName = "ServiceReference.IFileServer")]
	public interface IFileServer
	{
		[OperationContract(Action = "http://tempuri.org/IFileServer/DownloadUpdate", ReplyAction = "http://tempuri.org/IFileServer/DownloadUpdateResponse")]
		Stream DownloadUpdate();

		[OperationContract(Action = "http://tempuri.org/IFileServer/DownloadSettings", ReplyAction = "http://tempuri.org/IFileServer/DownloadSettingsResponse")]
		Stream DownloadSettings();
	}
}
