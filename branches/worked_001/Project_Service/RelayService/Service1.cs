using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;

namespace RelayService
{
	public class Service1 : ServiceBase
	{
		private StreamWriter file;

		private IContainer components = null;

		public Service1()
		{
			this.InitializeComponent();
		}

		protected override void OnStart(string[] args)
		{
			Service1.KillProcessIfExist("ServerHost");
			Thread.Sleep(2000);
			string baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string LogFile = Path.Combine(baseDir, "RelayService.log");
			this.file = new StreamWriter(new FileStream(LogFile, FileMode.Append));
			this.file.WriteLine("RelayService стартовал " + DateTime.Now.ToString());
			this.file.Flush();
			string url = Path.Combine(baseDir, "ServerHost.exe");
			this.file.WriteLine(baseDir);
			this.file.Flush();
			this.file.WriteLine(url);
			this.file.Flush();
			using (Process process = new Process
			{
				StartInfo = 
				{
					UseShellExecute = false,
					FileName = url,
					CreateNoWindow = true,
					Verb = url,
					WorkingDirectory = baseDir
				}
			})
			{
				process.Start();
			}
		}

		protected override void OnStop()
		{
			Service1.KillProcessIfExist("ServerHost");
			this.file.WriteLine("RelayService остановлен " + DateTime.Now.ToString());
			this.file.Flush();
			this.file.Close();
		}

		private static void KillProcessIfExist(string processName)
		{
			Process[] processesByName = Process.GetProcessesByName(processName);
			for (int i = 0; i < processesByName.Length; i++)
			{
				Process process = processesByName[i];
				process.Kill();
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			base.ServiceName = "ServiceRelayServer";
		}
	}
}
