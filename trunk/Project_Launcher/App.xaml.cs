using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Gecko;

namespace NewLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _mutex;

        //private static bool IsSingleInstance()
        //{
        //    _mutex = new Mutex(false, "RelayMutex");
        //    GC.KeepAlive(_mutex);
        //    try
        //    {
        //        return _mutex.WaitOne(0, false);
        //    }
        //    catch (AbandonedMutexException)
        //    {
        //        _mutex.ReleaseMutex();
        //        return _mutex.WaitOne(0, false);
        //    }
        //}

        protected override void OnStartup(System.Windows.StartupEventArgs e)
        {
            bool createdMutex;
            _mutex = new Mutex(true, "MyApplicationMutex", out createdMutex);
            	GeckoWebBrowser.UseCustomPrompt();
				Xpcom.Initialize(System.AppDomain.CurrentDomain.BaseDirectory + "xulrunner");
				GeckoPreferences.Default["extensions.blocklist.enabled"] = false;
				RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
            if (!createdMutex)
            {
                _mutex = null;
                MessageBox.Show("The application is already running.");
                Application.Current.Shutdown();
                return;
            }
            base.OnStartup(e);
        }

        protected override void OnExit(System.Windows.ExitEventArgs e)
        {
            if(_mutex != null)
                _mutex.ReleaseMutex();
            base.OnExit(e);
        }

        public static bool IsRelease(Assembly assembly)
        {
            object[] attributes = assembly.GetCustomAttributes(typeof(DebuggableAttribute), true);
            if (attributes == null || attributes.Length == 0)
                return true;

            var d = (DebuggableAttribute)attributes[0];
            if ((d.DebuggingFlags & DebuggableAttribute.DebuggingModes.Default) == DebuggableAttribute.DebuggingModes.None)
                return true;

            return false;
        }
    }
}
