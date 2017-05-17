using Newtonsoft.Json;
//using RelayServer.DataContext;
using RelayServer.Helpers;
using RelayServer.Interfaces;
using RequestHandlers;
using RequestHandlers.Extensions;
using RequestHandlers.Handlers;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Policy;
using System.Threading.Tasks;

namespace RelayServer.Portals
{
	public class ChevroletPortal : BrandPortal
	{
        private static readonly object AutorizeLock;

        private static readonly CookieContainer CookieContainer;

        static ChevroletPortal()
		{
            ChevroletPortal.AutorizeLock = new object();
            ChevroletPortal.CookieContainer = new CookieContainer();
        }

		public override void OpenSession(string url, long providerId, bool forceSession)
		{
            string url_session = string.Empty;
            int validateSession = -1;

            //uri = new Uri(url);
            //urlSession = string.Format("{0}://{1}/", uri.Scheme, uri.Host);
            url_session = url;

            if (!((validateSession = GetValidateSession(url_session, forceSession, ChevroletPortal.CookieContainer)) == 0)) {
                string login = string.Empty,
		            password = string.Empty;

                if (validateSession == -1) {
                    using (AvtoritetEntities ae = new AvtoritetEntities()) {
                        string sql = 
                            //string.Format(
                            ////"SELECT        TOP (1) dbo.ProviderAccount.Login, dbo.ProviderAccount.Password\r\n                                            FROM dbo.Provider INNER JOIN\r\n                                            dbo.ProviderAccount ON dbo.Provider.ProviderId = dbo.ProviderAccount.ProviderId\r\n                                            WHERE(dbo.Provider.Uri LIKE N'%{0}%') AND(dbo.ProviderAccount.Enable = 1)"
                            ////, CatalogApi.UrlConstants.ChevroletOpelGroupRoot)
                            //"SELECT TOP (1) dbo.ProviderAccount.Login, dbo.ProviderAccount.Password{0}{1}"
                            //    + "FROM dbo.Provider{0}{1}"
                            //    + "INNER JOIN dbo.ProviderAccount ON dbo.Provider.ProviderId = dbo.ProviderAccount.ProviderId{0}{1}"
                            //    + "WHERE(dbo.Provider.Uri = N'{2}') AND (dbo.ProviderAccount.Enable = 1)"
                            //    , "\r\n", "                                            ", url_session)
                            GetQueryCreditionals(providerId)
                            ;

                        ProvAcc provider = ae.Database.SqlQuery<ProvAcc>(sql, new object[0]).FirstOrDefault<ProvAcc>();
                        if (provider != null) {
                            login = provider.Login;
                            password = provider.Password;
                        }
                    }

                    //uri = new Uri(url);
                    //urlSession = string.Format("{0}://{1}/", uri.Scheme, uri.Host);
                    url_session = url;

                    this.m_requestHandler = RequestHandlerFactory.Create(url_session, login, password, null);
                } else
                    ;
            } else
                ;
            HttpResponseMessage responseMessage = this.GetResponse(url_session, providerId, validateSession, this.m_requestHandler, ChevroletPortal.CookieContainer);
			if (responseMessage != null)
			{
				this.m_requestHandler.GetSessionResultAsync(responseMessage);
			}
		}

        //public override void CloseSession()
        //{
        //    CloseSession(/*CatalogApi.UrlConstants.ChevroletOpelGroupUserLogoutTo, m_requestHandler,*/ CookieContainer);
        //}

        public override void CloseSession()
        {
            CloseSession(/*url, m_requestHandler,*/ CookieContainer);
        }

        public override HttpResponseMessage GetResponse(string url, long providerId, int validateSession, IRequestHandler reqHandler, CookieContainer container)
		{
            HttpResponseMessage resHttpResponseMessage;
            string url_session = string.Empty;
            bool error_session = false;

            url_session = string.Format("{0}/", url);

            lock (ChevroletPortal.AutorizeLock) {
                ConsoleHelper.Debug("CHEVROLET");

                if (validateSession == 1)
                    CloseSession();
                else
                    ;

                if (!(validateSession == 0)) {
                    resHttpResponseMessage = reqHandler.OpenSessionAsync(url_session, container);

                    error_session = this.SessionHasError(resHttpResponseMessage);

                    if (error_session == false) {
                        ConsoleHelper.Info(string.Format("Url Navigation to open session: {0}, RequestUri={1}, StatusCode={2}"
                            , url_session
                            , resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri
                            , resHttpResponseMessage.StatusCode));

                        base.OpenSession(url_session, providerId, validateSession == 1);

                        return resHttpResponseMessage;
                    } else
                        ;

                    ConsoleHelper.Error(string.Format("Open session error: url={0}, StatusCode={1}"
                        , resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri
                        , resHttpResponseMessage.StatusCode));
                } else
                {

                // без разницы force или не force, но сессию возвращать надо!
                //if (forceSession == true) {
                    Task<HttpResponseMessage> session2 = reqHandler.GetSessionAsync(url_session, container);
                    session2.Wait();
                    resHttpResponseMessage = session2.Result;

                    error_session = this.SessionHasError(resHttpResponseMessage);

                    if (error_session == false) {
                        ConsoleHelper.Info(string.Format("Url Navigation to confirmed session: {0}, RequestUri={1}, StatusCode={2}"
                            , url_session
                            , resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri
                            , resHttpResponseMessage.StatusCode));

                        return resHttpResponseMessage;
                    } else
                        ;

                    ConsoleHelper.Warning(string.Format("Confirmed session error: url={0}, RequestUri={1}, StatusCode={2}"
                        , url_session
                        , resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri
                        , resHttpResponseMessage.StatusCode));

                    CloseSession();

                    // url для новой-повторной сессии - повторный код (см. выше, при отсутствии авторизации)
                    resHttpResponseMessage = reqHandler.OpenSessionAsync(url_session, container);

                    error_session = this.SessionHasError(resHttpResponseMessage);

                    if (error_session == false) {
                        ConsoleHelper.Info(string.Format("Url Navigation to reopen-forced session: {0}, RequestUri={1}, StatusCode={2}"
                            , url_session
                            , resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri
                            , resHttpResponseMessage.StatusCode));

                        base.OpenSession(url_session, providerId, false);

                        return resHttpResponseMessage;
                    } else
                        ;

                    ConsoleHelper.Error(string.Format("Reopen-forced session error: url={0}, StatusCode={1}"
                        , resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri
                        , resHttpResponseMessage.StatusCode));
                }
                //else
                //    ;

                ConsoleHelper.Error("Session obtained faulty");
            }

            return null;
        }

		protected override bool SessionHasError(HttpResponseMessage responseMessage)
		{
			HttpStatusCode statusCode = responseMessage.StatusCode;
			return statusCode != HttpStatusCode.OK || responseMessage.RequestMessage.RequestUri.AbsoluteUri.Contains("error403");
		}

        public override string GetCookies(string url)
        {
            return BrandPortal.GetCookies(url, CookieContainer, true);
        }
    }
}
