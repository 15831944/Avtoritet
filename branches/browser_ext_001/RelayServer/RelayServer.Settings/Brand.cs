using System;
using System.Collections.Generic;

namespace RelayServer.Settings
{
	public class Brand
	{
		public int Left
		{
			get;
			set;
		}

		public int Top
		{
			get;
			set;
		}

		public string IconPath
		{
			get;
			set;
		}

		public string IconPath2
		{
			get;
			set;
		}

		public string NameAndFolder
		{
			get;
			set;
		}

		public bool MenuWindow
		{
			get;
			set;
		}

		public System.Collections.Generic.IList<BrandProvider> Providers
		{
			get;
			set;
		}

		public Brand()
		{
			this.Providers = new System.Collections.Generic.List<BrandProvider>();
		}
	}
}
