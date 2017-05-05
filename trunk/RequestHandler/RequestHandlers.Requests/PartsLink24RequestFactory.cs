using RequestHandlers.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace RequestHandlers.Requests
{
	internal static class PartsLink24RequestFactory
	{
		private const string Partslink24ComPartslink24UserLoginDo = "https://www.partslink24.com/partslink24/login-ajax!login.action";

		private const string Partslink24ComPartslink24UserLogoutDo = "https://www.partslink24.com/partslink24/user/logout.do";

		public static HttpRequestMessage CreateLoginRequest(string login, string password)
		{
			return new HttpRequestMessage
			{
				Content = PartsLink24RequestFactory.FormUrlEncodedContent(login, password),
				Method = HttpMethod.Post,
				RequestUri = new Uri("https://www.partslink24.com/partslink24/login-ajax!login.action")
			};
		}

		public static HttpRequestMessage CreateLogoutRequest()
		{
			return new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri("https://www.partslink24.com/partslink24/user/logout.do")
			};
		}

		private static FormUrlEncodedContent FormUrlEncodedContent(string id_login, string password)
		{
			string[] ar = id_login.Split(new char[]
			{
				'/'
			});
			string id = ar[0];
			string login = ar[1];
			ConsoleHelper.Info("Partslink ID: " + id);
			ConsoleHelper.Info("Partslink Login: " + login);
			ConsoleHelper.Info("Partslink Password: " + password);
			List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("org.apache.struts.taglib.html.TOKEN", "7bfe4fc414c2621c309b7960a92d012e"),
				new KeyValuePair<string, string>("loginAction", ""),
				new KeyValuePair<string, string>("loginBean.accountLogin", id),
				new KeyValuePair<string, string>("loginBean.userLogin", login),
				new KeyValuePair<string, string>("loginBean.password", password)
			};
			return new FormUrlEncodedContent(postData);
		}
	}
}
