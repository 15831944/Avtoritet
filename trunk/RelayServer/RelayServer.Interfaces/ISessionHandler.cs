using RequestHandlers.Handlers;
using System;
using System.Net;
using System.Net.Http;

namespace RelayServer.Interfaces
{
	public interface ISessionHandler
	{
		void OpenSession(string url, bool forceSession);

        void CloseSession();

        void CloseSession(string url);

		string GetCookies(string url);

		HttpResponseMessage GetResponse(string url, bool forceSession, IRequestHandler reqHandler, CookieContainer container);
	}
}
