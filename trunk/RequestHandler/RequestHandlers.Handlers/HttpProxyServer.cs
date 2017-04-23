using RequestHandlers.Helpers;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RequestHandlers.Handlers
{
	public static class HttpProxyServer
	{
		public static async Task<HttpResponseMessage> SendRequest(HttpRequestMessage httpRequestMessage, CookieContainer cookieContainer)
		{
			int arg_54_0 = 0;
			//httpRequestMessage.RequestUri.AbsoluteUri.IndexOf(string.Format("{0}.com", CatalogApi.UrlConstants.Partslink24Root));

            HttpResponseMessage result;
			if (arg_54_0 != 0)
			{
				HttpProxyServer.ConfigureMessageHeaders(httpRequestMessage);
				using (HttpClient httpClient = new HttpClient(HttpProxyServer.CreateClientHandler2(cookieContainer), true))
				{
					httpClient.BaseAddress = httpRequestMessage.RequestUri;
					result = await httpClient.SendAsync(httpRequestMessage);

					return result;
				}
			}

			HttpProxyServer.ConfigureMessageHeaders(httpRequestMessage);
			using (HttpClient httpClient2 = new HttpClient(HttpProxyServer.CreateClientHandler(cookieContainer), true))
			{
				httpClient2.BaseAddress = httpRequestMessage.RequestUri;
				result = await httpClient2.SendAsync(httpRequestMessage);
			}

			return result;
		}

		private static void ConfigureMessageHeaders(HttpRequestMessage message)
		{
			message.Headers.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
			message.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
			message.Headers.Add("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4");
			message.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.154 Safari/537.36");
			message.Headers.Add("Connection", "keep-Alive");
		}

		private static HttpClientHandler CreateClientHandler2(CookieContainer cookieContainer)
		{
			ConsoleHelper.Info(string.Format("WebProxy - {0}", CatalogApi.UrlConstants.WEB_PROXY));
			return new HttpClientHandler
			{
				AllowAutoRedirect = true,
				ClientCertificateOptions = ClientCertificateOption.Automatic,
				UseCookies = true,
				CookieContainer = cookieContainer,
				Proxy = new WebProxy(CatalogApi.UrlConstants.WEB_PROXY, false),
				UseProxy = true
			};
		}

		private static HttpClientHandler CreateClientHandler(CookieContainer cookieContainer)
		{
			return new HttpClientHandler
			{
				AllowAutoRedirect = true,
				ClientCertificateOptions = ClientCertificateOption.Automatic,
				UseCookies = true,
				CookieContainer = cookieContainer
			};
		}
	}
}
