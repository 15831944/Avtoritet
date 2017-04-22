using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Ninject;
using PushNotification.Repository;

namespace PushNotification.Features.Hub
{
    public class DemoHub : Microsoft.AspNet.SignalR.Hub
    {
        [Inject]
        public IVersionRepository VersionRepository { get; set; }
        [Inject]
        public ISettingsRepository SettingRepository { get; set; }

        public override Task OnConnected()
        {
            var entity = VersionRepository.Versions.FirstOrDefault();
            var settingVersion = SettingRepository.GetSettingVersion();
            var message=(entity == null)? "1.0.0.0": entity.Value;
            message = message + @"/" + settingVersion;
            return Clients.All.hubMessage(message);
        }

        public override Task OnReconnected()
        {
            var entity = VersionRepository.Versions.FirstOrDefault();
            var settingVersion = SettingRepository.GetSettingVersion();
            var message = (entity == null) ? "1.0.0.0" : entity.Value;
            message = message + @"/" + settingVersion;
            return Clients.Caller.hubMessage(message);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            var entity = VersionRepository.Versions.FirstOrDefault();
            var settingVersion = SettingRepository.GetSettingVersion();
            var message = (entity == null) ? "1.0.0.0" : entity.Value;
            message = message + @"/" + settingVersion;
            return Clients.All.hubMessage(message);
        }

        public void SendToMe(string value)
        {
            Clients.Caller.hubMessage(value);
        }

        public void SendToConnectionId(string connectionId, string value)
        {
            Clients.Client(connectionId).hubMessage(value);
        }

        public void SendToAll(string value)
        {
            var entity = VersionRepository.Versions.FirstOrDefault();
            var settingVersion = SettingRepository.GetSettingVersion();
            var message = (entity == null) ? "1.0.0.0" : entity.Value;
            message = message + @"/" + settingVersion;
            Clients.All.hubMessage(message);
        }

        public void SendToGroup(string groupName, string value)
        {
            Clients.Group(groupName).hubMessage(value);
        }

        public void JoinGroup(string groupName, string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId))
            {
                connectionId = Context.ConnectionId;
            }

            Groups.Add(connectionId, groupName);
            Clients.All.hubMessage(connectionId + " joined group " + groupName);
        }

        public void LeaveGroup(string groupName, string connectionId)
        {
            if (string.IsNullOrEmpty(connectionId))
            {
                connectionId = Context.ConnectionId;
            }

            Groups.Remove(connectionId, groupName);
            Clients.All.hubMessage(connectionId + " left group " + groupName);
        }

        public void IncrementClientVariable()
        {
            Clients.Caller.counter = Clients.Caller.counter + 1;
            Clients.Caller.hubMessage("Incremented counter to " + Clients.Caller.counter);
        }

        public void ThrowOnVoidMethod()
        {
            throw new InvalidOperationException("ThrowOnVoidMethod");
        }

        public async Task ThrowOnTaskMethod()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            throw new InvalidOperationException("ThrowOnTaskMethod");
        }

        public void ThrowHubException()
        {
            throw new HubException("ThrowHubException",
                new {Detail = "I can provide additional error information here!"});
        }

        public void StartBackgroundThread()
        {
//   BackgroundThread.Enabled = true;
//   BackgroundThread.SendOnPersistentConnection();
//   BackgroundThread.SendOnHub();
        }

        public void StopBackgroundThread()
        {
//   BackgroundThread.Enabled = false;
        }
    }
}