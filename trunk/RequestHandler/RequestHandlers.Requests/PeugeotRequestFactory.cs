using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RequestHandlers.Requests
{
	public class PeugeotRequestFactory
	{
		private const string CitroenLoginDo = "http://public.servicebox.peugeot.com/do/login";

		private const string CitroenLogoutDo = "http://public.servicebox.peugeot.com/do/logout";

		public static HttpRequestMessage CreateLoginRequest(string login, string password)
		{
			return new HttpRequestMessage
			{
				Content = PeugeotRequestFactory.FormUrlEncodedContent(login, password),
				Method = HttpMethod.Post,
				RequestUri = new Uri(CitroenLoginDo)
			};
		}

		public static HttpRequestMessage CreateLogoutRequest()
		{
			return new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri(CitroenLogoutDo)
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
