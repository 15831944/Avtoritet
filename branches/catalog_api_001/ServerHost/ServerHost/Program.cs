using CatalogApi;
using RelayServer.Processors;
using RequestHandlers.Helpers;
using ServerHost.Helpers;
using ServerHost.ServiceReference;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ServerHost
{
	public static class Program
	{
        // TODO: требуется закрыть все сессии
        //private static readonly RequestProcessorClient RpClient = new RequestProcessorClient();

        [STAThread]
		private static void Main()
		{
            int running = 0;

            string message = string.Empty;

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
                        message = string.Format("Start proxy-application...{0}{1}"
                            , Environment.NewLine
                            , string.Concat(Enumerable.Repeat("*---", 16)));
                                Logging.Info(message);

                        EventHandler fHostClosed = (object obj, EventArgs ev) => {
                            Uri[] uriAddresses = new Uri[(obj as ServiceHost).BaseAddresses.Count];
                            (obj as ServiceHost).BaseAddresses.CopyTo(uriAddresses, 0);

                            message = string.Format("Service to uri addresses: {0} has closed...", uriAddresses[0].OriginalString);
                            ConsoleHelper.Trace(message);
                            Logging.Info(message);

                            running--;
                        };

                        ServiceMetadataBehavior metadataBehavior = new ServiceMetadataBehavior
						{
							HttpGetEnabled = true,
							MetadataExporter = 
							{
								PolicyVersion = PolicyVersion.Policy15
							}
						};

						Program.SetupHost(proxyHost, metadataBehavior, fHostClosed);
                        message = string.Format("Proxy Address: {0}", new Uri(ConfigurationManager.AppSettings["ProxyAddress"]).Authority);
                        ConsoleHelper.Info(message);
                        Logging.Info(message);
                        running ++;

                        Program.SetupHost(storageHost, metadataBehavior, fHostClosed);
                        message = string.Format("Storage Address: {0}", new Uri(ConfigurationManager.AppSettings["StorageAddress"]).Authority);
                        ConsoleHelper.Info(message);
                        Logging.Info(message);
                        running++;

                        while (running > 0)
						{
							ConsoleKey pressedKey =
                                Console.ReadKey(true).Key
                                //char.ConvertFromUtf32(Console.Read())
                                ;
							ConsoleKey consoleKey = pressedKey;
							if (consoleKey != ConsoleKey.Escape)
							{
								ConsoleHelper.Warning("Press 'ESCAPE' to stop proxy server..");
							}
							else
							{
                                if (running == 2) {
                                    // TODO: требуется закрыть все сессии
                                    RelayServer.Portals.BrandPortal.Close();

                                    Program.StopHost(proxyHost);
                                    Program.StopHost(storageHost);
                                } else
                                    ;
                            }
						}

                        ConsoleHelper.Info("Press any key to shutdown service...");
                        Console.ReadKey(true);
                    }
				}
			}
			catch (Exception ex)
			{
				ErrorLogHelper.AddErrorInLog("Main()", string.Format("{0} | {1}", ex.Message , ex.StackTrace));
                CatalogApi.Logging.Exception(ex, true);
				Environment.Exit(0);
			} finally {
                message = string.Format("Exit from proxy-application...{0}{1}"
                    , Environment.NewLine
                    , string.Concat(Enumerable.Repeat("*---", 16)));
                Logging.Info(message);
            }
		}

		private static void StopHost(ICommunicationObject host)
		{
            ConsoleHelper.Warning(string.Format("Service has closing..."));

            host.Close();
		}

		private static void SetupHost(ServiceHostBase hostBase, IServiceBehavior serviceBehavior, EventHandler delegateHostClosed)
		{
			hostBase.Description.Behaviors.Add(serviceBehavior);
			hostBase.Open();
            hostBase.Closed += new EventHandler (delegateHostClosed);
        }

        private static void HostBase_Closed(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
