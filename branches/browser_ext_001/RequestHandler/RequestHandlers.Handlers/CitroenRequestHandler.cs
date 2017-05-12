using RequestHandlers.Helpers;
using RequestHandlers.Requests;
using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RequestHandlers.Handlers
{
	public class CitroenRequestHandler : BrandRequestHandler
	{
		public CitroenRequestHandler(string login, string password)
            : base (login, password)
		{
		}

        protected override HttpRequestMessage createLoginRequest()
        {
            return CitroenRequestFactory.CreateLoginRequest(m_login, m_password);
        }

        protected override HttpRequestMessage createLogoutRequest()
        {
            return CitroenRequestFactory.CreateLogoutRequest();
        }
    }
}
