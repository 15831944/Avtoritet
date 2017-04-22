using System;

namespace RelayServer.Settings
{
	internal interface ISettingsZipper
	{
		void UnzipToRoot(string sourceZipFile);

		string CreateZipFromSettings(string json);
	}
}
