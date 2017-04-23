using RequestHandlers.Helpers;
using RequestHandlers.Requests;
using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RequestHandlers.Handlers
{
	public class ChevroletRequestHandler : IRequestHandler
	{
		public const string Pl24Sessionid = "PL24SESSIONID";

		public const string Pl24LoggedInTrail = "pl24LoggedInTrail";

		private string login;

		private string password;

		public ChevroletRequestHandler(string login, string password)
		{
			this.login = login;
			this.password = password;
		}

		public bool SessionHasEnded(string url, CookieContainer cookieContainer)
		{
			return this.NeedAuthorization(url, cookieContainer);
		}

		public bool NeedAuthorization(string url, CookieContainer cookieContainer)
		{
			CookieCollection cookies = cookieContainer.GetCookies(new Uri(url));
			if (cookies.Count > 0)
			{
				ChevroletRequestHandler.PrintCookies(cookies);
			}
			return cookies.Count == 0;
		}

		public async Task Close(CookieContainer cookieContainer)
		{
			await HttpProxyServer.SendRequest(ChevroletRequestFactory.CreateLogoutRequest(), cookieContainer);
		}

		public async Task<string> GetSessionResultAsync(HttpResponseMessage responseMessage)
		{
			return await responseMessage.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
		}

		public async Task<HttpResponseMessage> GetSessionAsync(string url, CookieContainer cookieContainer)
		{
			return await HttpProxyServer.SendRequest(RequestFactory.CreateGetRequest(url), cookieContainer);
		}

		public async Task<HttpResponseMessage> OpenSessionAsync(CookieContainer cookieContainer)
		{
			return await HttpProxyServer.SendRequest(ChevroletRequestFactory.CreateLoginRequest(
			    // вар№0"https://imtportal.gm.com"
                CatalogApi.UrlConstants.ChevroletOpelGroup // вар№1 
                // вар№2 "https://gme-infotech.com"
                , this.login
                , this.password)
                , cookieContainer);
		}

		private static void PrintCookies(IEnumerable cookies)
		{
			foreach (Cookie cookie in cookies)
			{
				ConsoleHelper.Trace(string.Format("Cookie {0}={1}", cookie.Name, cookie.Value));
			}
		}
	}
}
