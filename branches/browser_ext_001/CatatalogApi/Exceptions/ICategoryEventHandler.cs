using System;

namespace CatalogApi.Exceptions
{
	public interface ICategoryEventHandler
	{
		void ProcessException(Exception ex);
	}
}