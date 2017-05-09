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
using System.Threading.Tasks;

namespace RelayServer.Portals
{
	public class CitroenPortal : BrandPortal
	{
		private static readonly object AutorizeLock;

		private static readonly CookieContainer CookieContainer;

		static CitroenPortal()
		{
			CitroenPortal.AutorizeLock = new object();
			CitroenPortal.CookieContainer = new CookieContainer();
		}

		public override void OpenSession(string url, bool forceSession)
		{
            if (GetValidateSession(url, forceSession, CitroenPortal.CookieContainer) == false) {
                string login = string.Empty
			        , password = string.Empty;

                using (AvtoritetEntities ae = new AvtoritetEntities())
                {
                    string sql = string.Format("SELECT TOP (1) dbo.ProviderAccount.Login, dbo.ProviderAccount.Password{0}"
                        + " FROM dbo.Provider{0}"
                        + " INNER JOIN dbo.ProviderAccount ON dbo.Provider.ProviderId = dbo.ProviderAccount.ProviderId{0}"
                        + " WHERE(dbo.Provider.Uri LIKE N'%citroen%') AND(dbo.ProviderAccount.Enable = 1)"
                        , "\r\n");

                    ProvAcc provider = ae.Database.SqlQuery<ProvAcc>(sql, new object[0]).FirstOrDefault<ProvAcc>();
                    login = provider.Login;
                    password = provider.Password;
                }
            
                this.m_requestHandler = RequestHandlerFactory.Create(url, login, password, null);
            } else
                ;

			HttpResponseMessage responseMessage = this.GetResponse(url, forceSession, this.m_requestHandler, CitroenPortal.CookieContainer);
			if (responseMessage != null)
			{
				this.m_requestHandler.GetSessionResultAsync(responseMessage);
			}
		}

        //public override void CloseSession()
        //{
        //    CloseSession(/*CatalogApi.UrlConstants.CitroenLogoutTo, m_requestHandler,*/ CookieContainer);
        //}

        public override void CloseSession(string url)
		{
            CloseSession(/*url, m_requestHandler,*/ CookieContainer);
		}

		public override string GetCookies(string url)
		{
            return BrandPortal.GetCookies(url, CookieContainer, false);
		}

		public override HttpResponseMessage GetResponse(string url, bool forceSession, IRequestHandler reqHandler, CookieContainer container)
		{
            HttpResponseMessage resHttpResponseMessage;
            string url_session = string.Empty;
            bool error_session = false;

            lock (CitroenPortal.AutorizeLock)
			{
				ConsoleHelper.Debug("CITROEN");

                url_session = url;

                if (reqHandler.NeedAuthorization(url_session, container) == true) {
                    resHttpResponseMessage = reqHandler.OpenSessionAsync(url_session, container);

                    error_session = this.SessionHasError(resHttpResponseMessage);

                    if (error_session == false) {
                        ConsoleHelper.Info(string.Format("Url Navigation to open session: {0}, RequestUri={1}, StatusCode={2}"
                            , url_session
                            , resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri
                            , resHttpResponseMessage.StatusCode));

                        base.OpenSession(url, forceSession);

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
                    // url для подтверждения сессии
                    url_session =
                        // "http://service.citroen.com/"
                        // url
                        "http://service.citroen.com/docpr/"
                        ;

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

                    CloseSession(CatalogApi.UrlConstants.CitroenLogoutTo);

                    // url для новой-повторной сессии - повторный код (см. выше, при отсутствии авторизации)
                    url_session = url;

                    resHttpResponseMessage = reqHandler.OpenSessionAsync(url_session, container);

                    error_session = this.SessionHasError(resHttpResponseMessage);

                    if (error_session == false) {
                        ConsoleHelper.Info(string.Format("Url Navigation to reopen-forced session: {0}, RequestUri={1}, StatusCode={2}"
                            , url_session
                            , resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri
                            , resHttpResponseMessage.StatusCode));

                        base.OpenSession(url, forceSession);

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
            return !(statusCode == HttpStatusCode.OK)
                || (responseMessage.RequestMessage.RequestUri.AbsoluteUri.Contains("index.jsp") == true);
		}
	}
}
