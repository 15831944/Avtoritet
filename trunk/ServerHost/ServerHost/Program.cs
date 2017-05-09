using RelayServer.Processors;
using RequestHandlers.Helpers;
using ServerHost.Helpers;
using ServerHost.ServiceReference;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace ServerHost
{
	public static class Program
	{
		[STAThread]
		private static void Main()
		{
            int running = 0;

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
                        EventHandler fHostClosed = (object obj, EventArgs ev) => {
                            Uri[] uriAddresses = new Uri[(obj as ServiceHost).BaseAddresses.Count];
                            (obj as ServiceHost).BaseAddresses.CopyTo(uriAddresses, 0);

                            ConsoleHelper.Trace(string.Format("Service to uri addresses: {0} has closed...", uriAddresses[0].OriginalString));

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
                        ConsoleHelper.Info(string.Format("Proxy Address: {0}", new Uri(ConfigurationManager.AppSettings["ProxyAddress"]).Authority));
                        running ++;

                        Program.SetupHost(storageHost, metadataBehavior, fHostClosed);
                        ConsoleHelper.Info(string.Format("Storage Address: {0}", new Uri(ConfigurationManager.AppSettings["StorageAddress"]).Authority));
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
                                    //ConsoleHelper.Warning("Proxy session was stopped. Now can close console window...");
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
				using (FileStream fileStream = new FileStream("ServerHost_Exception.txt", FileMode.Append, FileAccess.Write))
				{
					using (StreamWriter streamWriter = new StreamWriter(fileStream))
					{
						streamWriter.Write("[{0}]{1}{2} | {3}"
                            , DateTime.UtcNow, Environment.NewLine
                            , ex.Message, ex.StackTrace);
					}
				}
				Environment.Exit(0);
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
