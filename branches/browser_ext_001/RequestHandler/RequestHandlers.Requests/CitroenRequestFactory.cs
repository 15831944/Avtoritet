using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RequestHandlers.Requests
{
	public class CitroenRequestFactory
	{
		public static HttpRequestMessage CreateLoginRequest(string login, string password)
		{
			return new HttpRequestMessage
			{
				Content = CitroenRequestFactory.FormUrlEncodedContentForLogin(login, password),
				Method = HttpMethod.Post,
				RequestUri = new Uri(CatalogApi.UrlConstants.CitroenLoginDo)
			};
		}

		public static HttpRequestMessage CreateLogoutRequest()
		{
			return new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri(CatalogApi.UrlConstants.CitroenLogoutTo)
			};
		}

		private static FormUrlEncodedContent FormUrlEncodedContentForLogin(string login, string password)
		{
			List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("userid", login),
				new KeyValuePair<string, string>("password", password),
				new KeyValuePair<string, string>("window", "jbnvzV6B522C53oL3E") // "jbnCHH4pmdaz8BFqrp"
			};
			return new FormUrlEncodedContent(postData);
		}
	}
}
