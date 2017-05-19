using RequestHandlers.Handlers;
using System;
using System.Net;
using System.Net.Http;

namespace RelayServer.Interfaces
{
	public interface ISessionHandler
	{
		void OpenSession(string url, long providerId, bool forceSession);

        // KhryapinAN 09.05.2017
        //void CloseSession();

        void CloseSession(string url);

		string GetCookies(string url);

		HttpResponseMessage GetResponse(string url, long providerId, int validateSession, IRequestHandler reqHandler, CookieContainer container);
	}
}
