using RelayServer.Entities;
using System;
using System.CodeDom.Compiler;
using System.ServiceModel;

namespace ServerHost.ServiceReference
{
	[GeneratedCode("System.ServiceModel", "4.0.0.0"), ServiceContract(ConfigurationName = "ServiceReference.IRequestProcessor")]
	public interface IRequestProcessor
	{
		[OperationContract(Action = "http://tempuri.org/IRequestProcessor/OpenSession", ReplyAction = "http://tempuri.org/IRequestProcessor/OpenSessionResponse")]
		void OpenSession(string url, bool forceSession);

		[OperationContract(Action = "http://tempuri.org/IRequestProcessor/CloseSession", ReplyAction = "http://tempuri.org/IRequestProcessor/CloseSessionResponse")]
		void CloseSession(string url);

		[OperationContract(Action = "http://tempuri.org/IRequestProcessor/GetCookies", ReplyAction = "http://tempuri.org/IRequestProcessor/GetCookiesResponse")]
		string GetCookies(string url);

		[OperationContract(Action = "http://tempuri.org/IRequestProcessor/DownloadGuiSettigs", ReplyAction = "http://tempuri.org/IRequestProcessor/DownloadGuiSettigsResponse")]
		string DownloadGuiSettigs();

		[OperationContract(Action = "http://tempuri.org/IRequestProcessor/FreeOccupiedAccount", ReplyAction = "http://tempuri.org/IRequestProcessor/FreeOccupiedAccountResponse")]
		void FreeOccupiedAccount(string loginName);

		[OperationContract(Action = "http://tempuri.org/IRequestProcessor/GetUnoccupiedAccount", ReplyAction = "http://tempuri.org/IRequestProcessor/GetUnoccupiedAccountResponse")]
		AccountModel GetUnoccupiedAccount();

		[OperationContract(Action = "http://tempuri.org/IRequestProcessor/IsServiceAvailable", ReplyAction = "http://tempuri.org/IRequestProcessor/IsServiceAvailableResponse")]
		bool IsServiceAvailable();

		[OperationContract(Action = "http://tempuri.org/IRequestProcessor/LogConnection", ReplyAction = "http://tempuri.org/IRequestProcessor/LogConnectionResponse")]
		void LogConnection(string machineName, string launcherVersion);
	}
}
