using RequestHandlers.Handlers;
using System;
using RequestHandlers.Helpers;

namespace RequestHandlers
{
	public static class RequestHandlerFactory
	{
		public static IRequestHandler Create(string url, string login, string password, string optional = null)
		{
		    ConsoleHelper.Trace(string.Format("RequestHandlerFactory::Create(url={0}, login={1}, password={2})", url, login, password));

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
			else if (url.Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot))
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
