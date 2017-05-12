using System;
using System.Collections.Generic;
using System.Linq;

namespace RelayServer.Settings
{
	public class LauncherSettings
	{
		public const int ColumnsNumber = 3;

		public System.Collections.Generic.IList<GroupSet> Groups
		{
			get;
			private set;
		}

		public int Rows
		{
			get
			{
				int result;
				if (this.Groups.Count == 0)
				{
					result = 0;
				}
				else
				{
					result = this.Groups.Max((GroupSet group) => group.GroupBoxs.Count) / 3;
				}
				return result;
			}
		}

		public LauncherSettings()
		{
			this.Groups = new System.Collections.Generic.List<GroupSet>();
		}
	}
}
