using System;
using System.CodeDom.Compiler;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace ServerHost.ServiceReference
{
	[GeneratedCode("System.ServiceModel", "4.0.0.0")]
	public interface IFileServerChannel : IFileServer, IClientChannel, IContextChannel, IChannel, ICommunicationObject, IExtensibleObject<IContextChannel>, IDisposable
	{
	}
}
