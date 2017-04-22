using System;
using System.Net.Http;

namespace RequestHandlers.Exceptions
{
	public class AuthorizationException : Exception
	{
		public HttpResponseMessage ResponseMessage
		{
			get;
			private set;
		}

		public AuthorizationException(HttpResponseMessage responseMessage)
		{
			this.ResponseMessage = responseMessage;
		}

		public AuthorizationException(string message) : base(message)
		{
		}
	}
}
