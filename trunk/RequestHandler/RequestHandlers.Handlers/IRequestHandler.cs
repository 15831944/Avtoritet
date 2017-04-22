using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RequestHandlers.Handlers
{
	public interface IRequestHandler
	{
		Task Close(CookieContainer cookieContainer);

		bool SessionHasEnded(string url, CookieContainer cookieContainer);

		bool NeedAuthorization(string url, CookieContainer cookieContainer);

		Task<string> GetSessionResultAsync(HttpResponseMessage responseMessage);

		Task<HttpResponseMessage> GetSessionAsync(string url, CookieContainer cookieContainer);

		Task<HttpResponseMessage> OpenSessionAsync(CookieContainer cookieContainer);
	}
}
