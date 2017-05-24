using CatalogApi.Settings;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RequestHandlers.Requests
{
	public class PeugeotRequestFactory
	{
		public static HttpRequestMessage CreateLoginRequest(string login, string password)
		{
			return new HttpRequestMessage
			{
				Content = PeugeotRequestFactory.FormUrlEncodedContentForLogin(login, password),
				Method = HttpMethod.Post,
				RequestUri = new Uri(ResourceManager.Urls[CatalogApi.UrlConstants.Key.PeugeotLoginDo])
			};
		}

		public static HttpRequestMessage CreateLogoutRequest()
		{
			return new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri(ResourceManager.Urls[CatalogApi.UrlConstants.Key.PeugeotLogoutTo])
			};
		}

		private static FormUrlEncodedContent FormUrlEncodedContentForLogin(string login, string password)
		{
			List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("userid", login),
				new KeyValuePair<string, string>("password", password),
				new KeyValuePair<string, string>("window", "jbnvzV6B522C53oL3E")
			};
			return new FormUrlEncodedContent(postData);
		}
	}
}
