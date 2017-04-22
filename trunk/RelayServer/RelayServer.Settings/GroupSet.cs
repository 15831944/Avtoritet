using System;
using System.Collections.Generic;

namespace RelayServer.Settings
{
	public class GroupSet
	{
		public string Name
		{
			get;
			set;
		}

		public System.Collections.Generic.IList<GroupBox> GroupBoxs
		{
			get;
			private set;
		}

		public GroupSet()
		{
			this.GroupBoxs = new System.Collections.Generic.List<GroupBox>();
		}
	}
}
