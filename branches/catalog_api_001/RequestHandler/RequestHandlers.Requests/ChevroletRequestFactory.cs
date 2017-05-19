using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RequestHandlers.Requests
{
	public class ChevroletRequestFactory
	{
		public static HttpRequestMessage CreateLoginRequest(string url, string login, string password)
		{
			return new HttpRequestMessage
			{
				Content = ChevroletRequestFactory.FormUrlEncodedContentForLogin(login, password),
				Method = HttpMethod.Post,
				RequestUri = new Uri(string.Format("{0}/users/login.html", url))
			};
		}

		public static HttpRequestMessage CreateLogoutRequest()
		{
			return new HttpRequestMessage
			{
				Content = ChevroletRequestFactory.FormUrlEncodedContentForLogout(),
				Method = HttpMethod.Post,
				RequestUri = new Uri(CatalogApi.UrlConstants.ChevroletOpelGroupUserLogoutTo)
			};
		}

		private static FormUrlEncodedContent FormUrlEncodedContentForLogin(string login, string password)
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
