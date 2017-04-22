using System;
using System.Threading;

namespace RequestHandlers.Exceptions
{
	public static class ExceptionHelper
	{
		public static bool IsCriticalException(Exception ex)
		{
			return ex is StackOverflowException || ex is OutOfMemoryException || ex is ThreadAbortException;
		}
	}
}
