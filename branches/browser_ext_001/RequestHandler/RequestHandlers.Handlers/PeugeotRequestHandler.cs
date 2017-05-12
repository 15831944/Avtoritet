using RequestHandlers.Helpers;
using RequestHandlers.Requests;
using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RequestHandlers.Handlers
{
	public class PeugeotRequestHandler : BrandRequestHandler
	{
		public PeugeotRequestHandler(string login, string password)
            : base (login, password)
		{
		}

        protected override HttpRequestMessage createLoginRequest()
        {
            return PeugeotRequestFactory.CreateLoginRequest(m_login, m_password);
        }

        protected override HttpRequestMessage createLogoutRequest()
        {
            return PeugeotRequestFactory.CreateLogoutRequest();
        }
    }
}
