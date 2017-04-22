using System;
using System.Net.Http;

namespace RequestHandlers.Requests
{
	internal static class RequestFactory
	{
		public static HttpRequestMessage CreateGetRequest(string url)
		{
			return new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri(url)
			};
		}
	}
}
