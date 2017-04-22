using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RequestHandlers.Requests
{
	public class CitroenRequestFactory
	{
		private const string CitroenLoginDo = "http://service.citroen.com/do/login";

		private const string CitroenLogoutDo = "http://service.citroen.com/do/logout";

		public static HttpRequestMessage CreateLoginRequest(string Login, string Password)
		{
			return new HttpRequestMessage
			{
				Content = CitroenRequestFactory.FormUrlEncodedContent(Login, Password),
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

		private static FormUrlEncodedContent FormUrlEncodedContent(string Login, string Password)
		{
			List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("userid", Login),
				new KeyValuePair<string, string>("password", Password),
				new KeyValuePair<string, string>("window", "jbnvzV6B522C53oL3E")
			};
			return new FormUrlEncodedContent(postData);
		}
	}
}
