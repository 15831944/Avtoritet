using RequestHandlers.Handlers;
using System;
using System.Net;
using System.Net.Http;
using RequestHandlers.Helpers;

namespace RequestHandlers.Extensions
{
	public static class RequestsExtension
	{
		public static HttpResponseMessage GetSessionAsync(this IRequestHandler requestHandler, string url, CookieContainer container)
		{
		    ConsoleHelper.Trace(string.Format(@"::GetSessionAsync (url={0})", url));

            return requestHandler.GetSessionAsync(url, container).Result;
		}

		public static HttpResponseMessage OpenSessionAsync(this IRequestHandler requestHandler, string url, CookieContainer container)
		{
            ConsoleHelper.Trace(string.Format(@"::OpenSessionAsync (url={0})", url));

			return requestHandler.OpenSessionAsync(container).Result;
		}
	}
}
