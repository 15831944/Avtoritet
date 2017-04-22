using System;
using System.Collections.Generic;

namespace RelayServer.Models
{
	public class Provider
	{
		public string Uri
		{
			get;
			set;
		}

		public string IconPath
		{
			get;
			set;
		}

		public string Title
		{
			get;
			set;
		}

		public string Commands
		{
			get;
			set;
		}

		public int Order
		{
			get;
			set;
		}

		public string Login
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public System.Collections.Generic.List<CommandFile> CommandFiles
		{
			get;
			set;
		}

		public System.Collections.Generic.List<ProviderFile> ProviderFiles
		{
			get;
			set;
		}
	}
}
