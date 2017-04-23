using RequestHandlers.Handlers;
using System;

namespace RequestHandlers
{
	public static class RequestHandlerFactory
	{
		public static IRequestHandler Create(string url, string login, string password, string optional = null)
		{
			IRequestHandler result;
			if (url.Contains("citroen"))
			{
				result = new CitroenRequestHandler(login, password);
			}
			else if (url.Contains("peugeot"))
			{
				result = new PeugeotRequestHandler(login, password);
			}
			else if (url.Contains(CatalogApi.UrlConstants.Partslink24Root))
			{
				result = new PartsLink24RequestHandler(login, password);
			}
			else
			{
				if (!url.Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot))
				{
					throw new NotImplementedException(string.Format("Processor for url: '{0}' is not implemented", url));
				}
				if (!string.IsNullOrEmpty(optional))
				{
					result = new OpelRequestHandler(login, password);
				}
				else
				{
					result = new ChevroletRequestHandler(login, password);
				}
			}
			return result;
		}
	}
}
