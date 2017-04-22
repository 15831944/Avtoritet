using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RequestHandlers.Requests
{
	public class ChevroletRequestFactory
	{
		public static HttpRequestMessage CreateLoginRequest(string Login, string Password)
		{
			return new HttpRequestMessage
			{
				Content = ChevroletRequestFactory.FormUrlEncodedContent(Login, Password),
				Method = HttpMethod.Post,
				RequestUri = new Uri(CatalogApi.UrlConstants.ChevroletOpelGroupUserLoginDo)
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

		private static FormUrlEncodedContent FormUrlEncodedContent(string Login, string Password)
		{
			List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("logon", Login),
				new KeyValuePair<string, string>("password", Password)
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
