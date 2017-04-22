using System;
using System.Collections.Generic;

namespace RelayServer.Settings
{
	public class GroupBox
	{
		public string Title
		{
			get;
			set;
		}

		public int Top
		{
			get;
			set;
		}

		public int Left
		{
			get;
			set;
		}

		public int Height
		{
			get;
			set;
		}

		public int Width
		{
			get;
			set;
		}

		public System.Collections.Generic.IList<Brand> Brands
		{
			get;
			private set;
		}

		public GroupBox()
		{
			this.Brands = new System.Collections.Generic.List<Brand>();
		}
	}
}
