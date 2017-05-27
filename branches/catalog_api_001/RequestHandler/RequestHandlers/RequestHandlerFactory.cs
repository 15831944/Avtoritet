using RequestHandlers.Handlers;
using System;
using RequestHandlers.Helpers;
using CatalogApi.Settings;

namespace RequestHandlers
{
	public static class RequestHandlerFactory
	{
		public static IRequestHandler Create(string url, string login, string password, string optional = null)
		{
		    ConsoleHelper.Trace(string.Format("RequestHandlerFactory::Create(url={0}, login={1}, password={2})", url, login, password));

            IRequestHandler result;
			if (url.Contains(ResourceManager.Urls[CatalogApi.UrlConstants.Key.CitroenRoot]))
			{
				result = new CitroenRequestHandler(login, password);
			}
			else if (url.Contains(ResourceManager.Urls[CatalogApi.UrlConstants.Key.PeugeotRoot]))
			{
				result = new PeugeotRequestHandler(login, password);
			}
			else if (url.Contains(ResourceManager.Urls[CatalogApi.UrlConstants.Key.Partslink24Root]))
			{
				result = new PartsLink24RequestHandler(login, password);
			}
			else if (url.Contains(ResourceManager.Urls[CatalogApi.UrlConstants.Key.ChevroletOpelGroupRoot]))
			{
			    result = new ChevroletRequestHandler(login, password);
			    
            } else if (string.IsNullOrEmpty(optional) == false)
			{
			    result = new OpelRequestHandler(login, password);
			} else
			{
			    throw new NotImplementedException(string.Format("Processor for url: '{0}' is not implemented", url));
            }

			return result;
		}
	}
}
