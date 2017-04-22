using System;
using System.ComponentModel;

namespace BrowserExtension.Extensions
{
	public class BrowserExtendedNavigatingEventArgs : CancelEventArgs
	{
		private Uri _Url;

		private string _Frame;

		private UrlContext navigationContext;

		private object _pDisp;

		public Uri Url
		{
			get
			{
				return this._Url;
			}
		}

		public string Frame
		{
			get
			{
				return this._Frame;
			}
		}

		public UrlContext NavigationContext
		{
			get
			{
				return this.navigationContext;
			}
		}

		public object AutomationObject
		{
			get
			{
				return this._pDisp;
			}
			set
			{
				this._pDisp = value;
			}
		}

		public BrowserExtendedNavigatingEventArgs(object automation, Uri url, string frame, UrlContext navigationContext)
		{
			this._Url = url;
			this._Frame = frame;
			this.navigationContext = navigationContext;
			this._pDisp = automation;
		}
	}
}
