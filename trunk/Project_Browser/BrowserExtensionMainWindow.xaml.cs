using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BrowserExtension.Extensions;
using Newtonsoft.Json;

namespace BrowserExtension
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly WindowManager windowManager;

        public WindowManager WindowManager
        {
            get
            {
                return this.windowManager;
            }
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        public MainWindow()
        {
            try
            {
                this.InitializeComponent();
                this.StartNewEventSession();
                this.windowManager = new WindowManager();
                Task.Factory.StartNew(delegate
                {
                    string[] commandLineArgs = Environment.GetCommandLineArgs();
                    string value = string.Empty;
                    if ((commandLineArgs.Length > 1)
                        && (commandLineArgs[1].Contains("opel") == true))
                    {
                        base.Dispatcher.BeginInvoke(new Action(delegate
                        {
                            base.Title = "***Opel Dealer Online***";
                        }), new object[0]);
                        using (FileStream fileStream =
                            new FileStream("Session_Opel.txt", FileMode.Open, FileAccess.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(fileStream))
                            {
                                value = streamReader.ReadToEnd();
                            }
                        }
                    }
                    else
                        ;

                    if ((commandLineArgs.Length > 1)
                        && (commandLineArgs[1].Contains("chevrolet")))
                    {
                        base.Dispatcher.BeginInvoke(new Action(delegate
                        {
                            base.Title = "***Chevrolet Dealer Online***";
                        }), new object[0]);
                        using (FileStream fileStream =
                            new FileStream("Session_Chevrolet.txt", FileMode.Open, FileAccess.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(fileStream))
                            {
                                value = streamReader.ReadToEnd();
                            }
                        }
                    }
                    else
                        ;

                    if (string.IsNullOrEmpty(value) == false)
                    {
                        List<Cookie> list = JsonConvert.DeserializeObject<List<Cookie>>(value);
                        foreach (Cookie current in list)
                        {
                            MainWindow.InternetSetCookie(string.Format("{0}/", CatalogApi.UrlConstants.ChevroletOpelGroup), current.Name, current.Value);
                        }
                        base.Dispatcher.BeginInvoke(new Action(delegate
                        {
                            this.InternetExplorer.Navigate(CatalogApi.UrlConstants.ChevroletOpelGroupUserLoginDo);
                        }), new object[0]);
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Error opening catalog. Please report to administrator.");
                    }
                });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Message: " + ex.Message + " || StackTrace: " + ex.StackTrace);
            }
        }

        private void StartNewEventSession()
        {
            this.InternetExplorer.Quit += new EventHandler(this.InternetExplorerOnQuit);
            this.InternetExplorer.Navigating += new WebBrowserNavigatingEventHandler(this.InternetExplorerOnNavigating);
            this.InternetExplorer.NewWindow += delegate(object o, CancelEventArgs args)
            {
            };
            this.InternetExplorer.StartNewWindow += new EventHandler<BrowserExtendedNavigatingEventArgs>(this.InternetExplorerOnStartNewWindow);
            this.InternetExplorer.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.InternetExplorerOnDocumentCompleted);
        }

        private void InternetExplorerOnQuit(object sender, EventArgs eventArgs)
        {
        }

        private void InternetExplorerOnNavigating(object sender, WebBrowserNavigatingEventArgs webBrowserNavigatingEventArgs)
        {
            try
            {
                using (FileStream fileStream = new FileStream("Log.txt", FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.WriteLine(webBrowserNavigatingEventArgs.Url.AbsoluteUri);
                    }
                }
            }
            catch (Exception ex)
            {
                using (FileStream fileStream = new FileStream("Log.txt", FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.WriteLine(ex.Message);
                    }
                }
            }
        }

        private void InternetExplorerOnStartNewWindow(object sender, BrowserExtendedNavigatingEventArgs browserExtendedNavigatingEventArgs)
        {
            if (!(browserExtendedNavigatingEventArgs.Url != null) || !browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("about:blank"))
            {
                if (browserExtendedNavigatingEventArgs.Url != null && (browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=notify") || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=news") || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=bulletinboard") || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=feedback") || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=about") || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=downloads") || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=ug") || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("/privacy/") || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=help")))
                {
                    browserExtendedNavigatingEventArgs.Cancel = true;
                }
                else
                {
                    if (browserExtendedNavigatingEventArgs.Url != null && browserExtendedNavigatingEventArgs.Url.LocalPath.Contains("http://10.0.0.10:351/PQMace/login.fve"))
                    {
                        base.Close();
                    }
                    if (browserExtendedNavigatingEventArgs.Url != null && browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("navlevel=year&action=navigate&aid=epc&fid=nav"))
                    {
                        base.Close();
                    }
                    ExtendedWebBrowser extendedWebBrowser = this.WindowManager.New(false, base.Title);
                    browserExtendedNavigatingEventArgs.AutomationObject = extendedWebBrowser.Application;
                }
            }
        }

        private void InternetExplorerOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs webBrowserDocumentCompletedEventArgs)
        {
            try
            {
                if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("index.html"))
                {
                    this.InternetExplorer.Navigate(string.Format("{0}/subscriptions.html", CatalogApi.UrlConstants.ChevroletOpelGroup));
                }
                else if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("/users/login.html"))
                {
                    this.InternetExplorer.Navigate(string.Format("{0}/subscriptions.html", CatalogApi.UrlConstants.ChevroletOpelGroup));
                }
                else if ((!(this.InternetExplorer.Document == null))
                         && (!(this.InternetExplorer.Document.Body == null))
                         && (!(this.InternetExplorer.Document.Window == null))
                         && (!(this.InternetExplorer.Document.Window.Frames == null)))
                    {
                    // EPC
                        foreach (HtmlElement htmlElement in InternetExplorer.Document.GetElementsByTagName("form"))
                        {
                            if (AutoClickEPCSubmit(htmlElement) == true)
                            {
                                this.DelayForNextNavigation(this.IeHost, 3000, 4000);

                                return;
                            }
                            else
                                ;
                        }
                    }
                    else
                        ;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[{0}] {1} / {2}", new object[]
				{
					DateTime.Now,
					ex.Message,
					ex.StackTrace
				});
            }
        }

        public static bool AutoClickEPCSubmit(HtmlElement htmlElement)
        {
            htmlElement.SetAttribute("target", "_self");
            if (htmlElement.Document != null) {
                HtmlElementCollection elementsByTagName =
                    htmlElement.Document.GetElementsByTagName("input");
                foreach (HtmlElement current in from HtmlElement element in elementsByTagName
                    where element.GetAttribute("value")
                        .Equals("EPC", StringComparison.InvariantCultureIgnoreCase)
                    select element) {
                    current.InvokeMember("click");

                    return true;
                }
            } else
                ;

            return false;
        }

        public static bool AutoTypeCredentials(HtmlDocument document, HtmlElement element, params string[] credentionals)
        {
            HtmlElement elementById;

            element.SetAttribute("target", "_self"); //???
            if (element.Document != null) {
                elementById = document.GetElementsByTagName("input")
                    .Cast<HtmlElement>()
                    .First<HtmlElement>(x => x.GetAttribute("name").Equals(credentionals[0]));
                if (elementById != null)
                {
                    elementById.SetAttribute("value", credentionals[1]);
                }
                else
                    return false;

                elementById = document.GetElementsByTagName("input")
                    .Cast<HtmlElement>()
                    .First<HtmlElement>(x => x.GetAttribute("name").Equals(credentionals[2]));
                if (elementById != null)
                {
                    elementById.SetAttribute("value", credentionals[3]);
                }
                else
                    return false;

                elementById = document.GetElementsByTagName("input")
                    .Cast<HtmlElement>()
                    .First<HtmlElement>(x => x.GetAttribute("type").Equals("submit"));
                if (elementById != null)
                {
                    elementById.InvokeMember("click");
                }
                else
                    return false;
            } else
                ;

            return true;
        }

        private void DelayForNextNavigation(UIElement uie, int min, int max)
        {
            new System.Threading.Thread(() =>
            {
                try
                {
                    Thread.Sleep(new Random().Next(min, max));
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        uie.Visibility = Visibility.Visible;
                    }), new object[0]);
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(string.Format("[{0}] {1} / {2}", DateTime.Now, ex.Message, ex.StackTrace));
                }
            }).Start();
        }
    }
}
