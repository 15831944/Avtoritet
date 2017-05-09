using Newtonsoft.Json;
//using RelayServer.DataContext;
using RelayServer.Helpers;
using RelayServer.Interfaces;
using RequestHandlers;
using RequestHandlers.Extensions;
using RequestHandlers.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RelayServer.Portals
{
	public class PartslinkPortal : BrandPortal
	{
        private static readonly object AutorizeLock;

        private static readonly CookieContainer CookieContainer;

        private static bool partslinkIsBlocked;

		static PartslinkPortal()
		{
            PartslinkPortal.AutorizeLock = new object();
            PartslinkPortal.CookieContainer = new CookieContainer();
        }

		public override void OpenSession(string url, bool forceSession)
		{
            string url_session = url;

            if (GetValidateSession(url_session, forceSession, PartslinkPortal.CookieContainer) == false) {
                base.OpenSession(url_session, forceSession);

                string login = string.Empty
			        , password = string.Empty;

			    using (AvtoritetEntities ae = new AvtoritetEntities())
			    {
				    string sql = string.Format("SELECT dbo.ProviderAccount.Login, dbo.ProviderAccount.Password{1}{0}{0}{0}{0}"
                        + "FROM dbo.Provider{1}{0}{0}{0}{0}"
                        + "INNER JOIN dbo.ProviderAccount ON dbo.Provider.ProviderId = dbo.ProviderAccount.ProviderId{1}{0}{0}{0}{0}"
                        + "WHERE(dbo.Provider.Uri LIKE N'%partslink%') AND (dbo.ProviderAccount.Enable = 1)"
                        , "          "
                        , "\r\n");

				    System.Collections.Generic.List<ProvAcc> provider = ae.Database.SqlQuery<ProvAcc>(sql, new object[0]).ToList<ProvAcc>();
				    if (provider.Count > 0)
				    {
					    System.Random random = new System.Random();
					    int randomValue = (provider.Count > 1) ? random.Next(provider.Count - 1) : 0;
					    login = provider[randomValue].Login;
					    password = provider[randomValue].Password;
				    }
			    }

                this.m_requestHandler = RequestHandlerFactory.Create(url_session, login, password, null);
            } else
                ;

            HttpResponseMessage responseMessage = this.GetResponse(url_session, forceSession, this.m_requestHandler, PartslinkPortal.CookieContainer);
            if (responseMessage != null) {
                this.m_requestHandler.GetSessionResultAsync(responseMessage);
            } else
                ;
		}

        public override void CloseSession()
        {
            CloseSession(CatalogApi.UrlConstants.Partslink24ComPartslink24UserLogoutTo, /*m_requestHandler,*/ CookieContainer);
        }

        public override void CloseSession(string url)
		{
            CloseSession(url, /*m_requestHandler,*/ CookieContainer);
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

            lock (PartslinkPortal.AutorizeLock)
			{
				if (PartslinkPortal.partslinkIsBlocked == true)
				{
                    resHttpResponseMessage = null;
				}
				else
				{
					ConsoleHelper.Debug("PARTSLINK");

                    url_session = url;

                    if (reqHandler.NeedAuthorization(url_session, container) == true) {
                        resHttpResponseMessage = reqHandler.OpenSessionAsync(url_session, container);

                        error_session = this.SessionHasError(resHttpResponseMessage);

                        if (error_session == false) {
                            ConsoleHelper.Info(string.Format("Url Navigation to open session: {0}, RequestUri={1}, StatusCode={2}"
                                , url_session
                                , resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri
                                , resHttpResponseMessage.StatusCode));

                            return resHttpResponseMessage;
                        } else
                            ;

                        ConsoleHelper.Error(string.Format("Open session error: url={0}, StatusCode={1}"
                            , resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri
                            , resHttpResponseMessage.StatusCode));

                        this.CloseSession(CatalogApi.UrlConstants.Partslink24ComPartslink24UserLogoutTo);
                        PartslinkPortal.partslinkIsBlocked = true;
                    } else
                    {

                    // без разницы force или не force, но сессию возвращать надо!
                    //if (forceSession == true) {
                        // url для подтверждения сессии
                        url_session = CatalogApi.UrlConstants.Partslink24ComPartslink24UserLoginDo;

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

                        //CloseSession(CatalogApi.UrlConstants.Partslink24ComPartslink24UserLogoutTo);

                        //// url для новой-повторной сессии - повторный код (см. выше, при отсутствии авторизации)
                        if (resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri.Contains("login.do")
                            || (reqHandler.NeedAuthorization(url_session, container) == true)) {
                            resHttpResponseMessage = reqHandler.OpenSessionAsync(url_session, container);

                            error_session = this.SessionHasError(resHttpResponseMessage);

                            if (error_session == false) {
                                ConsoleHelper.Info(string.Format("Url Navigation to reopen-forced session: {0}, RequestUri={1}, StatusCode={2}"
                                    , url_session
                                    , resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri
                                    , resHttpResponseMessage.StatusCode));

                                return resHttpResponseMessage;
                            } else
                                ;

                            ConsoleHelper.Error(string.Format("Reopen-forced session error: url={0}, StatusCode={1}"
                                , resHttpResponseMessage.RequestMessage.RequestUri.AbsoluteUri
                                , resHttpResponseMessage.StatusCode));

                            this.CloseSession(CatalogApi.UrlConstants.Partslink24ComPartslink24UserLogoutTo);
                            PartslinkPortal.partslinkIsBlocked = true;
                        } else
                            ;
                    }
                    //else
                    //    ;

                    ConsoleHelper.Error("Session obtained faulty");
				}
			}

			return null;
		}

		protected override bool SessionHasError(HttpResponseMessage responseMessage)
		{
			HttpStatusCode statusCode = responseMessage.StatusCode;
			bool result;
			if (statusCode != HttpStatusCode.OK)
			{
				result = true;
			}
			else
			{
				ConsoleHelper.Trace(responseMessage.RequestMessage.RequestUri.AbsoluteUri);
				result = responseMessage.RequestMessage.RequestUri.AbsoluteUri.Contains("login.do");
			}
			return result;
		}
	}
}
