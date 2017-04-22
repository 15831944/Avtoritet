using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace NewLauncher.Extension.Features.Hub
{
	public class HubClient
	{
		private IHubProxy hubProxy;

		public async Task RunAsync(string url, System.Action<string> refreshAction)
		{
			HubConnection hubConnection = new HubConnection(url);
			this.hubProxy = hubConnection.CreateHubProxy("DemoHub");
			this.hubProxy.On("hubMessage", refreshAction);
			await hubConnection.Start();
		}
	}
}
