using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RequestHandlers.Requests
{
	public class OpelRequestFactory
	{
		private const string OpelLoginDo = "https://imtportal.gm.com/users/login.html";

		private const string OpelLogoutDo = "https://imtportal.gm.com/users/logout.html";

		public static HttpRequestMessage CreateLoginRequest(string login, string password)
		{
			return new HttpRequestMessage
			{
				Content = OpelRequestFactory.FormUrlEncodedContent(login, password),
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://imtportal.gm.com/users/login.html")
			};
		}

		public static HttpRequestMessage CreateLogoutRequest()
		{
			return new HttpRequestMessage
			{
				Content = OpelRequestFactory.FormUrlEncodedContentForLogout(),
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://imtportal.gm.com/users/logout.html")
			};
		}

		private static FormUrlEncodedContent FormUrlEncodedContent(string login, string password)
		{
			List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("logon", login),
				new KeyValuePair<string, string>("password", password)
			};
			return new FormUrlEncodedContent(postData);
		}

		private static FormUrlEncodedContent FormUrlEncodedContentForLogout()
		{
			List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("check", "1")
			};
			return new FormUrlEncodedContent(postData);
		}
	}
}
