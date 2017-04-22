using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ServerHost.ServiceReference
{
	[GeneratedCode("System.ServiceModel", "4.0.0.0"), DebuggerStepThrough]
	public class FileServerClient : ClientBase<IFileServer>, IFileServer
	{
		public FileServerClient()
		{
		}

		public FileServerClient(string endpointConfigurationName) : base(endpointConfigurationName)
		{
		}

		public FileServerClient(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
		{
		}

		public FileServerClient(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
		{
		}

		public FileServerClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
		{
		}

		public Stream DownloadUpdate()
		{
			return base.Channel.DownloadUpdate();
		}

		public Stream DownloadSettings()
		{
			return base.Channel.DownloadSettings();
		}
	}
}
