using RequestHandlers.Helpers;
using RequestHandlers.Requests;
using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RequestHandlers.Handlers
{
	public abstract class BrandRequestHandler : IRequestHandler
	{
		protected string m_login;

        protected string m_password;

		public BrandRequestHandler(string login, string password)
		{
			this.m_login = login;
			this.m_password = password;
		}

        protected abstract HttpRequestMessage createLoginRequest();

        protected abstract HttpRequestMessage createLogoutRequest();

        public bool SessionHasEnded(string url, CookieContainer cookieContainer)
        {
            return this.NeedAuthorization(url, cookieContainer);
        }

        public virtual bool NeedAuthorization(string url, CookieContainer cookieContainer)
        {
            CookieCollection cookies = cookieContainer.GetCookies(new Uri(url));
            if (cookies.Count > 0) {
                PrintCookies(cookies);
            }
            return cookies.Count == 0;
        }

        public async Task Close(CookieContainer cookieContainer)
        {
            await HttpProxyServer.SendRequest(createLogoutRequest(), cookieContainer);
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
            return await HttpProxyServer.SendRequest(createLoginRequest(), cookieContainer);
        }

        private static void PrintCookies(IEnumerable cookies)
        {
            foreach (Cookie cookie in cookies) {
                ConsoleHelper.Trace(string.Format("Cookie {0}={1}", cookie.Name, cookie.Value));
            }
        }
    }
}
