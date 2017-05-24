using RequestHandlers.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RequestHandlers.Requests
{
	internal static class PartsLink24RequestFactory
	{
		public static HttpRequestMessage CreateLoginRequest(string login, string password)
		{
			return new HttpRequestMessage
			{
				Content = PartsLink24RequestFactory.FormUrlEncodedContentForLogin(login, password),
				Method = HttpMethod.Post,
				RequestUri = new Uri(CatalogApi.UrlConstants.Partslink24ComPartslink24AjaxLoginAction)

            };
		}

		public static HttpRequestMessage CreateLogoutRequest()
		{
			return new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri(CatalogApi.UrlConstants.Partslink24ComPartslink24UserLogoutTo)
			};
		}

		private static FormUrlEncodedContent FormUrlEncodedContentForLogin(string id_login, string password)
		{
			string[] array_auth = id_login.Split(new char[] { '/' });
			string id = array_auth[0];
			string login = array_auth[1];
			ConsoleHelper.Info("Partslink ID: " + id);
			ConsoleHelper.Info("Partslink Login: " + login);
			ConsoleHelper.Info("Partslink Password: " + password);
			List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>(CatalogApi.UrlConstants.PartsLink24FormRequestKeyToken
                    , CatalogApi.UrlConstants.PartsLink24FormRequestValueToken),
				new KeyValuePair<string, string>("loginAction", ""),
				new KeyValuePair<string, string>("loginBean.accountLogin", id),
				new KeyValuePair<string, string>("loginBean.userLogin", login),
				new KeyValuePair<string, string>("loginBean.password", password)
			};
			return new FormUrlEncodedContent(postData);
		}
	}
}
