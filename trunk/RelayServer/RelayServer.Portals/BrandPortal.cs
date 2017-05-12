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

        protected void CloseSession(/*string url,*/ /*IRequestHandler reqHandler,*/ CookieContainer cookieContainer)
		{
            int result_close = 1; // по умолчанию - ничего не делаем

            try {
			    if (m_requestHandler != null)
			    {
                    m_requestHandler.Close(cookieContainer).Wait();

                    result_close = 0;
			    }
			    else
			    {
                    //result_close = 1;

				    //IRequestHandler request = RequestHandlerFactory.Create(url, string.Empty, string.Empty, null);
        //            if (request != null) {
        //                request.Close(cookieContainer).Wait();
        //            } else
        //                ;
			    }
            } catch (Exception e) {
                result_close = -1;

                string error_message = string.Empty;

                error_message = string.Format("BrandPortal::CloseSession (Portal={0}, cookies.Count={1})", this.GetType().FullName, cookieContainer.Count);
                ErrorLogHelper.AddErrorInLog(error_message
                    , string.Format("{0} | {1}" , e.Message, e.StackTrace));

                ConsoleHelper.Warning(error_message);
            }
            finally {
                ConsoleHelper.Warning(string.Format("Session [Portal={0}, result={1}] was closed"
                    , this.GetType().FullName //url
                    , result_close == -1 ? "Error" : result_close == 0 ? "Success" : result_close == 1 ? "Passed" : "Unknown"));

                _session.Remove(this);
            }
		}

        public static void Close ()
        {
            if (!(_session == null)) {
                if (_session.Count > 0)
                    while (_session.Count > 0) {
                        // url сформируется автоматически методом HttpRequestMessage RequestHandlers.Handlers.XXXHandler::createLogoutRequest
                        (_session[0] as BrandPortal).CloseSession();
                    }
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
                , strRes.Length > 0 ? string.Format("кол-во={0}", cookieContainer.Count) : "отсутствует"));

			return strRes;
		}

        protected int GetValidateSession(string url, bool forceSession, CookieContainer cookies)
        {
            int iRes = 0;

            if ((!(m_requestHandler == null))
                && (!(cookies == null))
                && (cookies.Count > 1))
                iRes = m_requestHandler.SessionHasEnded(url, cookies) == true ? 1 : 0;
            else
                iRes = -1;

            ConsoleHelper.Info(String.Format("BrandPortal::GetValidateSession: uri={0}, force={1}, validate={2}"
                , url, forceSession, iRes == -1 ? "не создана" : iRes == 1 ? "завершена" : "в работе"));

            return iRes;
        }

        public abstract HttpResponseMessage GetResponse(string url, int validateSession, IRequestHandler reqHandler, CookieContainer container);

        protected abstract bool SessionHasError(HttpResponseMessage responseMessage);

        //public abstract void CloseSession();

        public virtual void CloseSession(string url)
        {
            throw new NotImplementedException();
        }

        public abstract void CloseSession();

        public abstract string GetCookies(string url);
    }
}
