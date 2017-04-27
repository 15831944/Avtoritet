using Newtonsoft.Json;
using RelayServer.DataContext;
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
	public class ChevroletPortal : ISessionHandler
	{
		private static readonly object AutorizeLock;

		private static readonly CookieContainer CookieContainer;

		private IRequestHandler requestHandler;

		static ChevroletPortal()
		{
			ChevroletPortal.AutorizeLock = new object();
			ChevroletPortal.CookieContainer = new CookieContainer();
		}

		public void OpenSession(string url, bool forceSession)
		{
		    //Uri uri;
		    string
                urlSession = string.Empty,
		        login = string.Empty,
		        password = string.Empty;

            using (AvtoritetEntities ae = new AvtoritetEntities())
            {
                string sql = string.Format(
                    //"SELECT        TOP (1) dbo.ProviderAccount.Login, dbo.ProviderAccount.Password\r\n                                            FROM dbo.Provider INNER JOIN\r\n                                            dbo.ProviderAccount ON dbo.Provider.ProviderId = dbo.ProviderAccount.ProviderId\r\n                                            WHERE(dbo.Provider.Uri LIKE N'%{0}%') AND(dbo.ProviderAccount.Enable = 1)"
                    //, CatalogApi.UrlConstants.ChevroletOpelGroupRoot)
                    "SELECT TOP (1) dbo.ProviderAccount.Login, dbo.ProviderAccount.Password{0}{1}"
                    + "FROM dbo.Provider{0}{1}"
                    + "INNER JOIN dbo.ProviderAccount ON dbo.Provider.ProviderId = dbo.ProviderAccount.ProviderId{0}{1}"
                    + "WHERE(dbo.Provider.Uri = N'{2}') AND (dbo.ProviderAccount.Enable = 1)"
                    , "\r\n", "                                            ", url)
                    ;
                ProvAcc provider = ae.Database.SqlQuery<ProvAcc>(sql, new object[0]).FirstOrDefault<ProvAcc>();
                if (provider != null)
                {
                    login = provider.Login;
                    password = provider.Password;
                }
            }

		    //uri = new Uri(url);
		    //urlSession = string.Format("{0}://{1}/", uri.Scheme, uri.Host);
		    urlSession = url;

            this.requestHandler = RequestHandlerFactory.Create(urlSession, login, password, null);
			HttpResponseMessage responseMessage = this.GetResponse(urlSession, forceSession, this.requestHandler, ChevroletPortal.CookieContainer);
			if (responseMessage != null)
			{
				this.requestHandler.GetSessionResultAsync(responseMessage);
			}
		}

		public void CloseSession(string url)
		{
			if (this.requestHandler != null)
			{
				this.requestHandler.Close(ChevroletPortal.CookieContainer).Wait();
			}
			else
			{
				IRequestHandler request = RequestHandlerFactory.Create(url, string.Empty, string.Empty, null);
				if (request != null)
				{
					request.Close(ChevroletPortal.CookieContainer).Wait();
				}
			}
			ConsoleHelper.Info("Session was closed");
		}

		public string GetCookies(string url)
		{
			string json = JsonConvert.SerializeObject(ChevroletPortal.CookieContainer.GetCookies(new Uri(string.Format("{0}/", url))).Cast<Cookie>().ToList<Cookie>());
			ConsoleHelper.Debug(String.Format("Cookies obtained successfully: {0}"
                , json.Length > 0 ? json : "отсутствует"));
			return json;
		}

		public HttpResponseMessage GetResponse(string url, bool forceSession, IRequestHandler reqHandler, CookieContainer container)
		{
            HttpResponseMessage result;

			lock (ChevroletPortal.AutorizeLock)
			{
                ConsoleHelper.Debug("CHEVROLET");
				if (reqHandler.NeedAuthorization(string.Format("{0}/",url), container))
				{
					HttpResponseMessage session = reqHandler.OpenSessionAsync(string.Format("{0}/", url), container); // /users/login.html
                    ConsoleHelper.Info(string.Format("Open session status: {0}", session.StatusCode));
					if (!this.SessionHasError(session))
					{// Success
						result = session;
						return result;
					}
					ConsoleHelper.Error(string.Format("Open session error: {0}", url));
				}
				Task<HttpResponseMessage> session2 = reqHandler.GetSessionAsync(string.Format("{0}/", url), container); // /subscriptions.html
                session2.Wait();
				HttpResponseMessage responseMessage = session2.Result;
				ConsoleHelper.Info(string.Format("Url Navigation: {0}", responseMessage.RequestMessage.RequestUri.AbsoluteUri));
				HttpResponseMessage forcedSession = reqHandler.OpenSessionAsync(url, container);
				ConsoleHelper.Info(string.Format("Force session status: {0}", forcedSession.StatusCode));
				if (!this.SessionHasError(forcedSession))
				{
					result = forcedSession;
				}
				else
				{
					ConsoleHelper.Error(string.Format("Force session error: {0}", url));
					ConsoleHelper.Info("Session obtained successfully");
					result = null;
				}
			}
			return result;
		}

		private bool SessionHasError(HttpResponseMessage responseMessage)
		{
			HttpStatusCode statusCode = responseMessage.StatusCode;
			return statusCode != HttpStatusCode.OK || responseMessage.RequestMessage.RequestUri.AbsoluteUri.Contains("error403");
		}
	}
}
