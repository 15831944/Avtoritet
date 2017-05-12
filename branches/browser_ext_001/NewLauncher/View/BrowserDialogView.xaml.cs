using Gecko;
using Gecko.Events;
using NewLauncher.Extension;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Markup;

namespace NewLauncher.View
{
    /// <summary>
    /// Логика взаимодействия для BrowserDialogView.xaml
    /// </summary>
    public partial class BrowserDialogView : Window
    {
        public BrowserDialogView()
        {
            this.InitializeComponent();
            base.Title = "";
            this.IeWeb.ScriptErrorsSuppressed = true;
            StartNewEventSession();
        }

        private void GeckoWebOnCreateWindow(object sender, GeckoCreateWindowEventArgs geckoCreateWindowEventArgs)
        {
        }

        private void GeckoWebOnCreateWindow2(object sender, GeckoCreateWindow2EventArgs geckoCreateWindow2EventArgs)
        {
            geckoCreateWindow2EventArgs.Cancel = true;
        }

        private void GeckoWebOnDocumentCompleted(object sender, GeckoDocumentCompletedEventArgs geckoDocumentCompletedEventArgs)
        {
        }

        private void GeckoWebOnShowContextMenu(object sender, GeckoContextMenuEventArgs geckoContextMenuEventArgs)
        {
            try
            {
                System.Windows.Forms.ContextMenu contextMenu = geckoContextMenuEventArgs.ContextMenu.GetContextMenu();
                if (contextMenu != null)
                {
                    foreach (System.Windows.Forms.MenuItem menuItem2 in from System.Windows.Forms.MenuItem menuItem in contextMenu.MenuItems
                                                                        where menuItem != null
                                                                        select menuItem)
                    {
                        menuItem2.Enabled = false;
                        menuItem2.Visible = false;
                    }
                    contextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Копировать", (o, args) =>//delegate(object o, System.EventArgs args)
                    {
                        this.GeckoWeb.CopySelection();
                    }));
                    contextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Вставить", (o, args) => //delegate(object o, System.EventArgs args)
                    {
                       
                        this.GeckoWeb.Paste();
                    }));
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[{0}] {1} / {2}", new object[]
				{
					System.DateTime.Now,
					ex.Message,
					ex.StackTrace
				});
            }
            //EventHandler onClick = null;
            //EventHandler handler2 = null;
            //try
            //{
            //    ContextMenu contextMenu = geckoContextMenuEventArgs.ContextMenu.GetContextMenu();
            //    if (contextMenu != null)
            //    {
            //        foreach (MenuItem item in from menuItem in contextMenu.MenuItems.Cast<MenuItem>()
            //                                  where menuItem != null
            //                                  select menuItem)
            //        {
            //            item.Enabled = false;
            //            item.Visible = false;
            //        }
            //        if (onClick == null)
            //        {
            //            onClick = (o, args) => this.GeckoWeb.CopySelection();
            //        }
            //        contextMenu.MenuItems.Add(new MenuItem("Копировать", onClick));
            //        if (handler2 == null)
            //        {
            //            handler2 = (o, args) => this.GeckoWeb.Paste();
            //        }
            //        contextMenu.MenuItems.Add(new MenuItem("Вставить", handler2));
            //    }
            //}
            //catch (Exception exception)
            //{
            //    Debug.WriteLine("[{0}] {1} / {2}", new object[] { DateTime.Now, exception.Message, exception.StackTrace });
            //}
        }

        private void IeWebOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs webBrowserDocumentCompletedEventArgs)
        {
            if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("https://spoepc.xw.gm.com/PQMace/appf.fve"))
            {
            }
        }

        private void IeWebOnQuit(object sender, EventArgs eventArgs)
        {
            base.Close();
        }

        private void IeWebOnStartNewWindow(object sender, BrowserExtendedNavigatingEventArgs browserExtendedNavigatingEventArgs)
        {
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
        public void SetCookies(string url, string cookies)
        {
            string[] strArray = cookies.Split(new char[] { '=', ';' });
            InternetSetCookie(url, strArray[0], strArray[1]);
        }

        public ExtendedWebBrowser WebBrowser
        {
            get
            {
                return this.IeWeb;
            }
        }

        private void StartNewEventSession()
        {
            this.GeckoWeb.CreateWindow += new EventHandler<GeckoCreateWindowEventArgs>(this.GeckoWebOnCreateWindow);
            this.GeckoWeb.CreateWindow2 += new EventHandler<GeckoCreateWindow2EventArgs>(this.GeckoWebOnCreateWindow2);
            this.GeckoWeb.ShowContextMenu += new EventHandler<GeckoContextMenuEventArgs>(this.GeckoWebOnShowContextMenu);
            this.GeckoWeb.DocumentCompleted += new EventHandler<GeckoDocumentCompletedEventArgs>(this.GeckoWebOnDocumentCompleted);
            this.IeWeb.Quit += new EventHandler(this.IeWebOnQuit);
            this.IeWeb.StartNewWindow += new EventHandler<BrowserExtendedNavigatingEventArgs>(this.IeWebOnStartNewWindow);
            this.IeWeb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.IeWebOnDocumentCompleted);
        }
    }
}
