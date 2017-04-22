using RelayServer.Entities;
using System;
using System.ServiceModel;

namespace RelayServer.Interfaces
{
	[ServiceContract]
	public interface IRequestProcessor
	{
		[OperationContract]
		void OpenSession(string url, bool forceSession);

		[OperationContract]
		void CloseSession(string url);

		[OperationContract]
		string GetCookies(string url);

		[OperationContract]
		string DownloadGuiSettigs();

		[OperationContract]
		void FreeOccupiedAccount(string loginName);

		[OperationContract]
		AccountModel GetUnoccupiedAccount();

		[OperationContract]
		bool IsServiceAvailable();

		[OperationContract]
		void LogConnection(string machineName, string launcherVersion);
	}
}
