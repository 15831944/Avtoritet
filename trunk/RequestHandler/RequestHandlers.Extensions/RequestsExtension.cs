using RequestHandlers.Handlers;
using System;
using System.Net;
using System.Net.Http;

namespace RequestHandlers.Extensions
{
	public static class RequestsExtension
	{
		public static HttpResponseMessage GetSessionAsync(this IRequestHandler requestHandler, string url, CookieContainer container)
		{
			return requestHandler.GetSessionAsync(url, container).Result;
		}

		public static HttpResponseMessage OpenSessionAsync(this IRequestHandler requestHandler, string url, CookieContainer container)
		{
			return requestHandler.OpenSessionAsync(container).Result;
		}
	}
}
