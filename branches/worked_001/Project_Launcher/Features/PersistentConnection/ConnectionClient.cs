using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace NewLauncher.Features.PersistentConnection
{
	public class ConnectionClient
	{
		private Connection connection;

		public async Task RunAsync(string url, System.Action<string> showMessage)
		{
			this.connection = new Connection(url);
			this.connection.Received += delegate(string message)
			{
				showMessage(message);
			};
			await this.connection.Start();
			await this.connection.Send(new
			{
				Type = "sendToMe",
				Content = "Hello World!"
			});
		}
	}
}
