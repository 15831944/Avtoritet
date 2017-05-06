using Newtonsoft.Json;
using RelayServer.DataContext;
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
			string login = string.Empty;
			string password = string.Empty;
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
			this.m_requestHandler = RequestHandlerFactory.Create(url, login, password, null);
			HttpResponseMessage responseMessage = this.GetResponse(url, forceSession, this.m_requestHandler, PartslinkPortal.CookieContainer);
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
			lock (PartslinkPortal.AutorizeLock)
			{
				if (PartslinkPortal.partslinkIsBlocked)
				{
					result = null;
				}
				else
				{
					ConsoleHelper.Debug("PARTSLINK");
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
						this.CloseSession(CatalogApi.UrlConstants.Partslink24ComPartslink24UserLogoutTo);
						PartslinkPortal.partslinkIsBlocked = true;
					}
					if (forceSession)
					{
						Task<HttpResponseMessage> session2 = reqHandler.GetSessionAsync(CatalogApi.UrlConstants.Partslink24ComPartslink24UserLoginDo, container);
						session2.Wait();
						HttpResponseMessage responseMessage = session2.Result;
						ConsoleHelper.Info(string.Format("Url Navigation: {0}", responseMessage.RequestMessage.RequestUri.AbsoluteUri));
						if (responseMessage.RequestMessage.RequestUri.AbsoluteUri.Contains("login.do") || reqHandler.NeedAuthorization(url, container))
						{
							HttpResponseMessage forcedSession = reqHandler.OpenSessionAsync(url, container);
							ConsoleHelper.Info(string.Format("Force session status: {0}", forcedSession.StatusCode));
							if (!this.SessionHasError(forcedSession))
							{
								result = forcedSession;
								return result;
							}
							ConsoleHelper.Error(string.Format("Force session error: {0}", url));
							this.CloseSession(CatalogApi.UrlConstants.Partslink24ComPartslink24UserLogoutTo);
							PartslinkPortal.partslinkIsBlocked = true;
						}
					}
					ConsoleHelper.Info("Session obtained successfully");
					result = null;
				}
			}
			return result;
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
