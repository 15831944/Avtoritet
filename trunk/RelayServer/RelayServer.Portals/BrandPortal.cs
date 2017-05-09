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
using System.Security.Policy;
using System.Threading.Tasks;

namespace RelayServer.Portals
{
	public abstract class BrandPortal : ISessionHandler
	{
		protected IRequestHandler m_requestHandler;

        private static List<ISessionHandler> _session;

		static BrandPortal()
		{
		}

        public virtual void OpenSession(string url, bool forceSession)
        {
            ConsoleHelper.Info(String.Format("BrandPortal::OpenSession: uri={0}, force={1}"
                , url, forceSession));

            if (_session == null)
                _session = new List<ISessionHandler>();
            else
                ;

            if (_session.IndexOf(this) < 0)
                _session.Add(this);
            else
                ;
        }

        protected void CloseSession(string url, /*IRequestHandler reqHandler,*/ CookieContainer cookieContainer)
		{
            bool error_close = false;

            try {
			    if (m_requestHandler != null)
			    {
                    m_requestHandler.Close(cookieContainer).Wait();
			    }
			    else
			    {
				    IRequestHandler request = RequestHandlerFactory.Create(url, string.Empty, string.Empty, null);
				    if (request != null)
				    {
					    request.Close(cookieContainer).Wait();
				    }
			    }
            } catch (Exception e) {
                error_close = true;

                string error_message = string.Empty;

                error_message = string.Format("BrandPortal::CloseSession (url={0}, cookies.Count={1})", url, cookieContainer.Count);
                ErrorLogHelper.AddErrorInLog(error_message
                    , string.Format("{0} | {1}" , e.Message, e.StackTrace));

                ConsoleHelper.Warning(error_message);
            }
            finally {
                _session.Remove(this);
            }

			ConsoleHelper.Warning(string.Format("Session [url={0}, error={1}] was closed"
                , url
                , error_close));
		}

        public static void Close ()
        {
            if (!(_session == null)) {
                if (_session.Count > 0)
                    while (_session.Count > 0) { _session[0].CloseSession(); }
                else
                    ConsoleHelper.Warning(string.Format("BrandPortal::Close () - все сессии закрыты ранее..."));
            } else
                ConsoleHelper.Warning(string.Format("BrandPortal::Close () - ни одной сессии открыто не было..."));
        }

		public static string GetCookies(string url, CookieContainer cookieContainer, bool bRoot)
		{
            string strRes = string.Empty;

            if (!(cookieContainer == null))
                strRes = JsonConvert.SerializeObject(cookieContainer.GetCookies(new Uri(string.Format("{0}{1}", url, bRoot == true ? "/" : string.Empty)))
                    .Cast<Cookie>().ToList<Cookie>());
            else
                ;

			ConsoleHelper.Debug(String.Format("Cookies obtained successfully: {0}"
                , strRes.Length > 0 ? strRes : "отсутствует"));

			return strRes;
		}

        protected bool GetValidateSession(string url, bool forceSession, CookieContainer cookies)
        {
            bool bRes = false;

            bRes = (!(m_requestHandler == null))
                && ((!(cookies == null))
                    && (cookies.Count > 1))
                && (m_requestHandler.SessionHasEnded(url, cookies) == false);

            ConsoleHelper.Info(String.Format("BrandPortal::GetValidateSession: uri={0}, force={1}, validate={2}"
                , url, forceSession, bRes));

            return bRes;
        }

        public abstract HttpResponseMessage GetResponse(string url, bool forceSession, IRequestHandler reqHandler, CookieContainer container);

        protected abstract bool SessionHasError(HttpResponseMessage responseMessage);

        public abstract void CloseSession();

        public abstract void CloseSession(string url);

        public abstract string GetCookies(string url);
    }
}
