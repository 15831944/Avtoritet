using System;
using System.Collections.Generic;

namespace RelayServer.Models
{
	public class Catalog
	{
		public int Rows
		{
			get;
			set;
		}

		public System.Collections.Generic.List<Group> Groups
		{
			get;
			set;
		}
	}
}
