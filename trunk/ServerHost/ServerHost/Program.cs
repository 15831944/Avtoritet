using RelayServer.Processors;
using ServerHost.Helpers;
using ServerHost.ServiceReference;
using System;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ServerHost
{
	public static class Program
	{
		private const string Partslink24LogoutDo = "https://www.partslink24.com/partslink24/user/logout.do";

		private static readonly RequestProcessorClient RpClient = new RequestProcessorClient();

		[STAThread]
		private static void Main()
		{
			try
			{
				using (ServiceHost proxyHost = new ServiceHost(typeof(RequestProcessor), new Uri[]
				{
					new Uri(ConfigurationManager.AppSettings["ProxyAddress"])
				}))
				{
					using (ServiceHost storageHost = new ServiceHost(typeof(RequestProcessor), new Uri[]
					{
						new Uri(ConfigurationManager.AppSettings["StorageAddress"])
					}))
					{
						ServiceMetadataBehavior metadataBehavior = new ServiceMetadataBehavior
						{
							HttpGetEnabled = true,
							MetadataExporter = 
							{
								PolicyVersion = PolicyVersion.Policy15
							}
						};
						Program.SetupHost(proxyHost, metadataBehavior);
						Program.SetupHost(storageHost, metadataBehavior);
						ConsoleHelper.Info(string.Format("Proxy Address: {0}", new Uri(ConfigurationManager.AppSettings["ProxyAddress"]).Authority));
						while (true)
						{
							ConsoleKey pressedKey = Console.ReadKey(true).Key;
							ConsoleKey consoleKey = pressedKey;
							if (consoleKey != ConsoleKey.Escape)
							{
								ConsoleHelper.Warning("Press 'ESCAPE' to stop proxy server..");
							}
							else
							{
								Program.RpClient.CloseSession("https://www.partslink24.com/partslink24/user/logout.do");
								Program.StopHost(proxyHost);
								Program.StopHost(storageHost);
								ConsoleHelper.Warning("Proxy session was stopped. Now can close console window");
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ErrorLogHelper.AddErrorInLog("Main()", ex.Message + "|" + ex.StackTrace);
				using (FileStream fileStream = new FileStream("ServerHost_Exception.txt", FileMode.OpenOrCreate, FileAccess.Write))
				{
					using (StreamWriter streamWriter = new StreamWriter(fileStream))
					{
						streamWriter.Write("{0} / {1}", ex.Message, ex.StackTrace);
					}
				}
				Environment.Exit(0);
			}
		}

		private static void StopHost(ICommunicationObject proxyHost)
		{
			proxyHost.Close();
		}

		private static void SetupHost(ServiceHostBase hostBase, IServiceBehavior serviceBehavior)
		{
			hostBase.Description.Behaviors.Add(serviceBehavior);
			hostBase.Open();
		}
	}
}
