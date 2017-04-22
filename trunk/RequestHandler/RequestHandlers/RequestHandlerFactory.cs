using RequestHandlers.Handlers;
using System;

namespace RequestHandlers
{
	public static class RequestHandlerFactory
	{
		public static IRequestHandler Create(string url, string Login, string Password, string optional = null)
		{
			IRequestHandler result;
			if (url.Contains("citroen"))
			{
				result = new CitroenRequestHandler(Login, Password);
			}
			else if (url.Contains("peugeot"))
			{
				result = new PeugeotRequestHandler(Login, Password);
			}
			else if (url.Contains(CatalogApi.UrlConstants.Partslink24Root))
			{
				result = new PartsLink24RequestHandler(Login, Password);
			}
			else
			{
				if (!url.Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot))
				{
					throw new NotImplementedException(string.Format("Processor for url: '{0}' is not implemented", url));
				}
				if (!string.IsNullOrEmpty(optional))
				{
					result = new OpelRequestHandler(Login, Password);
				}
				else
				{
					result = new ChevroletRequestHandler(Login, Password);
				}
			}
			return result;
		}
	}
}
