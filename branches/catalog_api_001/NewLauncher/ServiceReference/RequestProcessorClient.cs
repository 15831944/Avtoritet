namespace NewLauncher.ServiceReference
{
    using System;
    using System.CodeDom.Compiler;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    [GeneratedCode("System.ServiceModel", "4.0.0.0"), DebuggerStepThrough]
    public class RequestProcessorClient : ClientBase<IRequestProcessor>, IRequestProcessor
    {
        public RequestProcessorClient()
        {
        }

        public RequestProcessorClient(string endpointConfigurationName) : base(endpointConfigurationName)
        {
        }

        public RequestProcessorClient(Binding binding, EndpointAddress remoteAddress) : base(binding, remoteAddress)
        {
        }

        public RequestProcessorClient(string endpointConfigurationName, EndpointAddress remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public RequestProcessorClient(string endpointConfigurationName, string remoteAddress) : base(endpointConfigurationName, remoteAddress)
        {
        }

        public void CloseSession(string url)
        {
            base.Channel.CloseSession(url);
        }

        public string DownloadGuiSettigs()
        {
            return base.Channel.DownloadGuiSettigs();
        }

        public void FreeOccupiedAccount(string loginName)
        {
            base.Channel.FreeOccupiedAccount(loginName);
        }

        public string GetCookies(string url)
        {
            return base.Channel.GetCookies(url);
        }

        public AccountModel GetUnoccupiedAccount()
        {
            return base.Channel.GetUnoccupiedAccount();
        }

        public bool IsServiceAvailable(string serviceUri)
        {
            return base.Channel.IsServiceAvailable(serviceUri);
        }

        public void LogConnection(string machineName, string launcherVersion)
        {
            base.Channel.LogConnection(machineName, launcherVersion);
        }

        public void OpenSession(string url, long providerId, bool forceSession)
        {
            base.Channel.OpenSession(url, providerId, forceSession);
        }
    }
}

