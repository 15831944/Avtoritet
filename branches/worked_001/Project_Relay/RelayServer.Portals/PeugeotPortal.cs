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
	public class PeugeotPortal : ISessionHandler
	{
		private static readonly object AutorizeLock;

		private static readonly CookieContainer CookieContainer;

		private IRequestHandler requestHandler;

		static PeugeotPortal()
		{
			PeugeotPortal.AutorizeLock = new object();
			PeugeotPortal.CookieContainer = new CookieContainer();
		}

		public void OpenSession(string url, bool forceSession)
		{
			string Login = string.Empty;
			string Password = string.Empty;
            using (AvtoritetEntities ae = new AvtoritetEntities())
            {
                string sql = "SELECT        TOP (1) dbo.ProviderAccount.Login, dbo.ProviderAccount.Password\r\n                                            FROM dbo.Provider INNER JOIN\r\n                                            dbo.ProviderAccount ON dbo.Provider.ProviderId = dbo.ProviderAccount.ProviderId\r\n                                            WHERE(dbo.Provider.Uri LIKE N'%peugeot%') AND(dbo.ProviderAccount.Enable = 1)";
                ProvAcc provider = ae.Database.SqlQuery<ProvAcc>(sql, new object[0]).FirstOrDefault<ProvAcc>();
                Login = provider.Login;
                Password = provider.Password;
            }

            this.requestHandler = RequestHandlerFactory.Create(url, Login, Password, null);
			HttpResponseMessage responseMessage = this.GetResponse(url, forceSession, this.requestHandler, PeugeotPortal.CookieContainer);
			if (responseMessage != null)
			{
				this.requestHandler.GetSessionResultAsync(responseMessage);
			}
		}

		public void CloseSession(string url)
		{
			if (this.requestHandler != null)
			{
				this.requestHandler.Close(PeugeotPortal.CookieContainer).Wait();
			}
			else
			{
				IRequestHandler request = RequestHandlerFactory.Create(url, string.Empty, string.Empty, null);
				if (request != null)
				{
					request.Close(PeugeotPortal.CookieContainer).Wait();
				}
			}
			ConsoleHelper.Info("Session was closed");
		}

		public string GetCookies(string url)
		{
			string json = JsonConvert.SerializeObject(PeugeotPortal.CookieContainer.GetCookies(new Uri(url)).Cast<Cookie>().ToList<Cookie>());
			ConsoleHelper.Debug("Cookies obtained succesfully");
			return json;
		}

		public HttpResponseMessage GetResponse(string url, bool forceSession, IRequestHandler reqHandler, CookieContainer container)
		{
			HttpResponseMessage result;
			lock (PeugeotPortal.AutorizeLock)
			{
				Uri request = new Uri(url);
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
					Task<HttpResponseMessage> session2 = reqHandler.GetSessionAsync("http://public.servicebox.peugeot.com/docpr/", container);
					session2.Wait();
					HttpResponseMessage responseMessage = session2.Result;
					ConsoleHelper.Info(string.Format("Url Navigation: {0}", responseMessage.RequestMessage.RequestUri.AbsoluteUri));
					if (responseMessage.RequestMessage.RequestUri.AbsoluteUri.Contains("index.jsp") || reqHandler.NeedAuthorization(url, container))
					{
						HttpResponseMessage forcedSession = reqHandler.OpenSessionAsync(url, container);
						ConsoleHelper.Info(string.Format("Force session status: {0}", forcedSession.StatusCode));
						if (!this.SessionHasError(forcedSession))
						{
							result = forcedSession;
							return result;
						}
						ConsoleHelper.Error(string.Format("Force session error: {0}", url));
					}
				}
				ConsoleHelper.Info("Session obtained successfully");
				result = null;
			}
			return result;
		}

		private bool SessionHasError(HttpResponseMessage responseMessage)
		{
			HttpStatusCode statusCode = responseMessage.StatusCode;
			return statusCode != HttpStatusCode.OK || responseMessage.RequestMessage.RequestUri.AbsoluteUri.Contains("index.jsp");
		}
	}
}
