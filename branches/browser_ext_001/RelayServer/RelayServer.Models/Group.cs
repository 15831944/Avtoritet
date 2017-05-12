using System;
using System.Collections.Generic;

namespace RelayServer.Models
{
	public class Group
	{
		public string Name
		{
			get;
			set;
		}

		public int Width
		{
			get;
			set;
		}

		public int Height
		{
			get;
			set;
		}

		public System.Collections.Generic.List<GroupBox> GroupBoxs
		{
			get;
			set;
		}

		public int Order
		{
			get;
			set;
		}
	}
}
