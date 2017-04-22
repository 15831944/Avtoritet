using RelayServer.Entities;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ServerHost.ServiceReference
{
	[GeneratedCode("System.ServiceModel", "4.0.0.0"), DebuggerStepThrough]
	public class RequestProcessorClient : ClientBase<IRequestProcessor>, IRequestProcessor
	{
		public RequestProcessorClient()
		{
		}

		public RequestProcessorClient(string endpointConfigurationName) : base(endpointConfigurationName)
		{
		}

		public RequestProcessorClient(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
		{
		}

		public RequestProcessorClient(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
		{
		}

		public RequestProcessorClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
		{
		}

		public void OpenSession(string url, bool forceSession)
		{
			base.Channel.OpenSession(url, forceSession);
		}

		public void CloseSession(string url)
		{
			base.Channel.CloseSession(url);
		}

		public string GetCookies(string url)
		{
			return base.Channel.GetCookies(url);
		}

		public string DownloadGuiSettigs()
		{
			return base.Channel.DownloadGuiSettigs();
		}

		public void FreeOccupiedAccount(string loginName)
		{
			base.Channel.FreeOccupiedAccount(loginName);
		}

		public AccountModel GetUnoccupiedAccount()
		{
			return base.Channel.GetUnoccupiedAccount();
		}

		public bool IsServiceAvailable()
		{
			return base.Channel.IsServiceAvailable();
		}

		public void LogConnection(string machineName, string launcherVersion)
		{
			base.Channel.LogConnection(machineName, launcherVersion);
		}
	}
}
