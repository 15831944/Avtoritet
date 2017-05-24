using CatalogApi.Settings;
using RequestHandlers.Helpers;
using RequestHandlers.Requests;
using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RequestHandlers.Handlers
{
	public class ChevroletRequestHandler : BrandRequestHandler
	{
        // вар№0 "https://imtportal.gm.com"
        // вар№1 CatalogApi.UrlConstants.ChevroletOpelGroup
        // вар№2 "https://gme-infotech.com"

        public ChevroletRequestHandler(string login, string password)
            : base (login, password)
		{
		}

        protected override HttpRequestMessage createLoginRequest()
        {
            return ChevroletRequestFactory.CreateLoginRequest(
                string.Format("{0}", ResourceManager.Urls[CatalogApi.UrlConstants.Key.ChevroletOpelGroup])
                , m_login
                , m_password);
        }

        protected override HttpRequestMessage createLogoutRequest()
        {
            return ChevroletRequestFactory.CreateLogoutRequest();
        }
    }
}
