using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace NewLauncher.Builder
{
	public static class HttpBuilder
	{
		public static async Task<T> GetJsonAsync<T>(this string requestUrl)
		{
			HttpResponseMessage httpResponseMessage = await new HttpClient().GetAsync(new System.Uri(requestUrl));
			Task<string> task = httpResponseMessage.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(task.Result);
		}
	}
}
