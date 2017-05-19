using System;
using System.ServiceProcess;

namespace RelayService
{
	internal static class Program
	{
		private static void Main()
		{
			ServiceBase[] ServicesToRun = new ServiceBase[]
			{
				new Service1()
			};
			ServiceBase.Run(ServicesToRun);
		}
	}
}
