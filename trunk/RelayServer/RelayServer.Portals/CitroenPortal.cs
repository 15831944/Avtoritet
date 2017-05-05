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
			string Login = string.Empty;
			string Password = string.Empty;
            using (AvtoritetEntities ae = new AvtoritetEntities())
            {
                string sql = "SELECT        TOP (1) dbo.ProviderAccount.Login, dbo.ProviderAccount.Password\r\n                                            FROM dbo.Provider INNER JOIN\r\n                                            dbo.ProviderAccount ON dbo.Provider.ProviderId = dbo.ProviderAccount.ProviderId\r\n                                            WHERE(dbo.Provider.Uri LIKE N'%citroen%') AND(dbo.ProviderAccount.Enable = 1)";
                ProvAcc provider = ae.Database.SqlQuery<ProvAcc>(sql, new object[0]).FirstOrDefault<ProvAcc>();
                Login = provider.Login;
                Password = provider.Password;
            }
            //Login = "AC27525217";
            //Password = "Citroenepc";

            this.m_requestHandler = RequestHandlerFactory.Create(url, Login, Password, null);
			HttpResponseMessage responseMessage = this.GetResponse(url, forceSession, this.m_requestHandler, CitroenPortal.CookieContainer);
			if (responseMessage != null)
			{
				this.m_requestHandler.GetSessionResultAsync(responseMessage);
			}
		}

		public override void CloseSession(string url)
		{
            BrandPortal.CloseSession(url, m_requestHandler, CookieContainer);
		}

		public override string GetCookies(string url)
		{
            return BrandPortal.GetCookies(url, CookieContainer, false);
		}

		public override HttpResponseMessage GetResponse(string url, bool forceSession, IRequestHandler reqHandler, CookieContainer container)
		{
			HttpResponseMessage result;
			lock (CitroenPortal.AutorizeLock)
			{
				ConsoleHelper.Debug("CITROEN");
				if (reqHandler.NeedAuthorization(url, container))
				{
					HttpResponseMessage session = reqHandler.OpenSessionAsync(url, container);
					ConsoleHelper.Info(string.Format("Open session status: {0}", session.StatusCode));
					if (!this.SessionHasError(session))
					{
						result = session;
						return result;
					}
					ConsoleHelper.Error(string.Format("Open session error: {0}", url));
				}
				if (forceSession)
				{
					Task<HttpResponseMessage> session2 = reqHandler.GetSessionAsync("http://service.citroen.com/docpr/", container);
					session2.Wait();
					HttpResponseMessage responseMessage = session2.Result;
					ConsoleHelper.Info(string.Format("Url Navigation: {0}", responseMessage.RequestMessage.RequestUri.AbsoluteUri));
					HttpResponseMessage forcedSession = reqHandler.OpenSessionAsync(url, container);
					ConsoleHelper.Info(string.Format("Force session status: {0}", forcedSession.StatusCode));
					if (!this.SessionHasError(forcedSession))
					{
						result = forcedSession;
						return result;
					}
					ConsoleHelper.Error(string.Format("Force session error: {0}", url));
				}
				ConsoleHelper.Info("Session obtained successfully");
				result = null;
			}
			return result;
		}

		protected override bool SessionHasError(HttpResponseMessage responseMessage)
		{
			HttpStatusCode statusCode = responseMessage.StatusCode;
			return statusCode != HttpStatusCode.OK || responseMessage.RequestMessage.RequestUri.AbsoluteUri.Contains("index.jsp");
		}
	}
}
