using RequestHandlers.Helpers;
using RequestHandlers.Requests;
using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RequestHandlers.Handlers
{
	internal class OpelRequestHandler : BrandRequestHandler
	{
		public OpelRequestHandler(string login, string password)
            : base (login, password)
		{
		}

        protected override HttpRequestMessage createLoginRequest()
        {
            return OpelRequestFactory.CreateLoginRequest(m_login, m_password);
        }

        protected override HttpRequestMessage createLogoutRequest()
        {
            return OpelRequestFactory.CreateLogoutRequest();
        }
    }
}
