using RequestHandlers.Helpers;
using RequestHandlers.Requests;
using System;
using System.Collections;
using System.Configuration;
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

        private int MAX_SEC_REOPEN_SESSION = int.Parse(ConfigurationManager.AppSettings["MaxSecReopenSession"]); // 1 час

        public virtual bool NeedAuthorization(string url, CookieContainer cookieContainer)
        {
            bool bRes = false;

            bool bExpires = false;
            int secRemaind = -1;

            CookieCollection cookies = cookieContainer.GetCookies(new Uri(url));
            if (cookies.Count > 0) {
                PrintCookies(cookies);

                bExpires = (cookies[0].Expires - DateTime.MinValue).TotalSeconds > 0;

                if (bExpires == true)
                    bRes = !((secRemaind = (int)(DateTime.Now - cookies[0].Expires).TotalSeconds) > 0);
                else if (MAX_SEC_REOPEN_SESSION > 0)
                    bRes = !((secRemaind = (MAX_SEC_REOPEN_SESSION - (int)(DateTime.Now - cookies[0].TimeStamp).TotalSeconds)) > 0);
                else
                    ;
            } else
                ;

            ConsoleHelper.Info(string.Format("::NeedAuthorization (url={0}) - рез-т={1} [Expires={2}, остаток={3} сек]..."
                , url, bRes
                , bExpires, secRemaind));

            return bRes;
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
