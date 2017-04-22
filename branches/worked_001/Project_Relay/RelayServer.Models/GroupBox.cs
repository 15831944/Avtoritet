using System;
using System.Collections.Generic;

namespace RelayServer.Models
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

		public bool VisibleBorder
		{
			get;
			set;
		}

		public System.Collections.Generic.List<Brand> Brands
		{
			get;
			set;
		}
	}
}
