using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RequestHandlers.Requests
{
	public class CitroenRequestFactory
	{
		private const string CitroenLoginDo = "http://service.citroen.com/do/login";

		private const string CitroenLogoutDo = "http://service.citroen.com/do/logout";

		public static HttpRequestMessage CreateLoginRequest(string login, string password)
		{
			return new HttpRequestMessage
			{
				Content = CitroenRequestFactory.FormUrlEncodedContent(login, password),
				Method = HttpMethod.Post,
				RequestUri = new Uri("http://service.citroen.com/do/login")
			};
		}

		public static HttpRequestMessage CreateLogoutRequest()
		{
			return new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("http://service.citroen.com/do/logout")
			};
		}

		private static FormUrlEncodedContent FormUrlEncodedContent(string login, string password)
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
