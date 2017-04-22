using System;
using System.Collections.Generic;

namespace RelayServer.Models
{
	public class Brand
	{
		public int BrandId
		{
			get;
			set;
		}

		public string NameAndFolder
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

		public byte[] IconPathImg
		{
			get;
			set;
		}

		public byte[] IconPath2Img
		{
			get;
			set;
		}

		public System.Collections.Generic.List<Provider> Providers
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

		public string ButtonStyle
		{
			get;
			set;
		}

		public bool MenuWindow
		{
			get;
			set;
		}
	}
}
