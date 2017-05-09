using RequestHandlers.Helpers;
using RequestHandlers.Requests;
using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RequestHandlers.Handlers
{
	public class PartsLink24RequestHandler : BrandRequestHandler
	{
		public const string Pl24Sessionid = "PL24SESSIONID";

		public const string Pl24LoggedInTrail = "pl24LoggedInTrail";

		public PartsLink24RequestHandler(string login, string password)
            : base (login, password)
		{
		}

		public override bool NeedAuthorization(string url, CookieContainer cookieContainer)
		{
            bool bRes = false;

            bRes = base.NeedAuthorization(url, cookieContainer);
            CookieCollection cookies = cookieContainer.GetCookies(new Uri(url));

            return bRes || cookies[Pl24LoggedInTrail] == null;
		}

        protected override HttpRequestMessage createLoginRequest()
        {
            return PartsLink24RequestFactory.CreateLoginRequest(m_login, m_password);
        }

        protected override HttpRequestMessage createLogoutRequest()
        {
            return PartsLink24RequestFactory.CreateLogoutRequest();
        }
    }
}
