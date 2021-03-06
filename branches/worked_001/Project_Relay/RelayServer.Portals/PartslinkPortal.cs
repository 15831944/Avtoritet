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
	public class PartslinkPortal : ISessionHandler
	{
		private static readonly object AutorizeLock;

		private static readonly CookieContainer CookieContainer;

		private static bool partslinkIsBlocked;

		private IRequestHandler requestHandler;

		static PartslinkPortal()
		{
			PartslinkPortal.AutorizeLock = new object();
			PartslinkPortal.CookieContainer = new CookieContainer();
		}

		public void OpenSession(string url, bool forceSession)
		{
			string Login = string.Empty;
			string Password = string.Empty;
			using (AvtoritetEntities ae = new AvtoritetEntities())
			{
				string sql = "SELECT          dbo.ProviderAccount.Login, dbo.ProviderAccount.Password\r\n                                            FROM dbo.Provider INNER JOIN\r\n                                            dbo.ProviderAccount ON dbo.Provider.ProviderId = dbo.ProviderAccount.ProviderId\r\n                                            WHERE(dbo.Provider.Uri LIKE N'%partslink%') AND(dbo.ProviderAccount.Enable = 1)";
				System.Collections.Generic.List<ProvAcc> provider = ae.Database.SqlQuery<ProvAcc>(sql, new object[0]).ToList<ProvAcc>();
				if (provider.Count > 0)
				{
					System.Random random = new System.Random();
					int randomValue = (provider.Count > 1) ? random.Next(provider.Count - 1) : 0;
					Login = provider[randomValue].Login;
					Password = provider[randomValue].Password;
				}
			}
			this.requestHandler = RequestHandlerFactory.Create(url, Login, Password, null);
			HttpResponseMessage responseMessage = this.GetResponse(url, forceSession, this.requestHandler, PartslinkPortal.CookieContainer);
			if (responseMessage != null)
			{
				this.requestHandler.GetSessionResultAsync(responseMessage);
			}
		}

		public void CloseSession(string url)
		{
			if (this.requestHandler != null)
			{
				this.requestHandler.Close(PartslinkPortal.CookieContainer).Wait();
			}
			else
			{
				IRequestHandler request = RequestHandlerFactory.Create(url, string.Empty, string.Empty, null);
				if (request == null)
				{
					return;
				}
				request.Close(PartslinkPortal.CookieContainer).Wait();
			}
			ConsoleHelper.Info("Session was closed");
		}

		public string GetCookies(string url)
		{
			string json = JsonConvert.SerializeObject(PartslinkPortal.CookieContainer.GetCookies(new Uri(url)).Cast<Cookie>().ToList<Cookie>());
			ConsoleHelper.Debug("Cookies obtained succesfully");
			return json;
		}

		public HttpResponseMessage GetResponse(string url, bool forceSession, IRequestHandler reqHandler, CookieContainer container)
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
						this.CloseSession("https://www.partslink24.com/partslink24/user/logout.do");
						PartslinkPortal.partslinkIsBlocked = true;
					}
					if (forceSession)
					{
						Task<HttpResponseMessage> session2 = reqHandler.GetSessionAsync("https://www.partslink24.com/partslink24/user/login.do", container);
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
							this.CloseSession("https://www.partslink24.com/partslink24/user/logout.do");
							PartslinkPortal.partslinkIsBlocked = true;
						}
					}
					ConsoleHelper.Info("Session obtained successfully");
					result = null;
				}
			}
			return result;
		}

		private bool SessionHasError(HttpResponseMessage responseMessage)
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
