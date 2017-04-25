﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
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

using CatalogApi;

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
            try {
                this.InitializeComponent();
                this.StartNewEventSession();

                //logging("Browser::ctor () - new WindowManager() - ???...");
                this.windowManager = new WindowManager();
                logging(string.Format("{1}{0}Browser::ctor () - new WindowManager([DateTime={2}]) - processing.."
                    , Environment.NewLine
                    , string.Concat(Enumerable.Repeat("*---", 16))
                    , DateTime.UtcNow));

                Task.Factory.StartNew(delegate
                {
                    string fileNameSession = "Session_ChevroletOpelGroup.txt";
                    string[] commandLineArgs = Environment.GetCommandLineArgs();
                    string value = string.Empty;
                    if ((commandLineArgs.Length > 1)
                        && (commandLineArgs[1].Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot) == true)) {
                        base.Dispatcher.BeginInvoke(new Action(delegate
                        {
                            base.Title = "***Chevrolet-Opel Dealer Online***";
                        }), new object[0]);

                        logging(String.Format("Browser::ctor () - read local session settings(file={0}) - ???..."
                            , fileNameSession));
                        if (File.Exists(fileNameSession) == true)
                        {
                            using (FileStream fileStream =
                                new FileStream(fileNameSession, FileMode.Open, FileAccess.Read))
                            {
                                using (StreamReader streamReader = new StreamReader(fileStream))
                                {
                                    value = streamReader.ReadToEnd();
                                }
                            }
                            logging(String.Format(
                                "Browser::ctor () - read local session settings(file={0}) - success..."
                                , fileNameSession));
                        }
                        else
                            logging(String.Format(
                                "Browser::ctor () - read local session settings(file={0}) - not exists..."
                                , fileNameSession));
                    } else
                        ;

                    if (string.IsNullOrEmpty(value) == false) {
                        logging(String.Format("Browser::ctor (cookies={0}) - cookis not empty..."
                            , value));

                        string urlSetCookie = string.Format("{0}/", commandLineArgs[1])
                            , urlNavigateDoLogin = string.Format("{0}/users/login.html", commandLineArgs[1]);

                        List<Cookie> list = JsonConvert.DeserializeObject<List<Cookie>>(value);
                        try
                        {
                            foreach (Cookie current in list)
                            {
                                logging(String.Format("Browser::ctor () - InternetSetCookie to={0}, key={1}, value={2}..."
                                    , urlSetCookie
                                    , current.Name,
                                    current.Value));

                                MainWindow.InternetSetCookie(urlSetCookie
                                    , current.Name,
                                    current.Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            logging(ex);
                        }

                        logging(String.Format("Browser to Navigate (Url={0}) - ..."
                            , urlNavigateDoLogin));

                        base.Dispatcher.BeginInvoke(new Action(delegate
                        {
                            this.InternetExplorer.Navigate(urlNavigateDoLogin);
                        }), new object[0]);
                    } else {
                        System.Windows.MessageBox.Show(string.Format("Error opening catalog. Please report to administrator.{0}"
                                + "Кол-во аргументов: {1}{0}"
                                + "1-ый аргумент={2}{0}"
                                + "2-ой аргумент={3}{0}"
                                + "cookie={4}"
                            , Environment.NewLine
                            , commandLineArgs.Length
                            , commandLineArgs.Length > 1 ? commandLineArgs[1] : "отсутсвует"
                            , commandLineArgs.Length > 2 ? commandLineArgs[2] : "отсутсвует"
                            , string.IsNullOrWhiteSpace(value) == false ? value : "отсутсвуют"));
                    }
                });
            } catch (Exception ex) {
                logging(ex);
            }
        }

        private void StartNewEventSession()
        {
            this.InternetExplorer.Quit += new EventHandler(this.InternetExplorerOnQuit);
            this.InternetExplorer.Navigating += new WebBrowserNavigatingEventHandler(this.InternetExplorerOnNavigating);
            this.InternetExplorer.NewWindow += delegate (object o, CancelEventArgs args)
            {
            };
            this.InternetExplorer.StartNewWindow += new EventHandler<BrowserExtendedNavigatingEventArgs>(this.InternetExplorerOnStartNewWindow);
            this.InternetExplorer.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.InternetExplorerOnDocumentCompleted);

            //logging(string.Format("Browser::StartNewEventSession (Title={0}) - ...", this.Title));
        }

        private void InternetExplorerOnQuit(object sender, EventArgs eventArgs)
        {
            logging(string.Format("Browser::InternetExplorerOnQuit (Title={0}) - ...", this.Title));

            Close();
        }

        private void InternetExplorerOnNavigating(object sender, WebBrowserNavigatingEventArgs webBrowserNavigatingEventArgs)
        {
            logging(string.Format("Browser::InternetExplorerOnNavigating (Title={0}, AbsoluteUri={1}) - ..."
                , this.Title
                , webBrowserNavigatingEventArgs.Url.AbsoluteUri));
        }

        private void InternetExplorerOnStartNewWindow(object sender, BrowserExtendedNavigatingEventArgs browserExtendedNavigatingEventArgs)
        {
            logging(string.Format("Browser::InternetExplorerOnStartNewWindow (Title={0}, argUrl={1}) - ..."
                , this.Title
                , browserExtendedNavigatingEventArgs.Url.ToString()));

            if (!(browserExtendedNavigatingEventArgs.Url != null)
                || (!(browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("about:blank")))) {
                if ((browserExtendedNavigatingEventArgs.Url != null)
                    && (browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=notify")
                    || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=news")
                    || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=bulletinboard")
                    || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=feedback")
                    || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=about")
                    || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=downloads")
                    || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=ug")
                    || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("/privacy/")
                    || browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("fid=help"))) {
                    browserExtendedNavigatingEventArgs.Cancel = true;
                } else {
                    if ((!(browserExtendedNavigatingEventArgs.Url == null))
                        && (browserExtendedNavigatingEventArgs.Url.LocalPath.Contains("http://10.0.0.10:351/PQMace/login.fve"))) {
                        base.Close();
                    }
                    if ((!(browserExtendedNavigatingEventArgs.Url == null))
                        && (browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("navlevel=year&action=navigate&aid=epc&fid=nav"))) {
                        base.Close();
                    }
                    ExtendedWebBrowser extendedWebBrowser = this.WindowManager.New(false, base.Title);
                    browserExtendedNavigatingEventArgs.AutomationObject = extendedWebBrowser.Application;
                }
            }
        }

        private bool IsDocumentValidate
        {
            get
            {
                return (!(this.InternetExplorer.Document == null))
                    && (!(this.InternetExplorer.Document.Body == null))
                    && (!(this.InternetExplorer.Document.Window == null))
                    && (!(this.InternetExplorer.Document.Window.Frames == null));
            }
        }

        private void InternetExplorerOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs webBrowserDocumentCompletedEventArgs)
        {
            logging(string.Format("Browser::InternetExplorerOnDocumentCompleted (Title={0}, argUrl={1}) - ..."
                , this.Title
                , webBrowserDocumentCompletedEventArgs.Url.ToString()));

            try {
                if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("/users/login.html") == true) {
                    if (IsDocumentValidate == true)
                    // Credentials
                        foreach (HtmlElement htmlElement in InternetExplorer.Document.GetElementsByTagName("form"))
                        {
                            if (CatalogApi.Autocomplit.TypeCredentials(this.InternetExplorer.Document
                                , htmlElement
                                , "logon", "avtoritetepc", "password", "Hugoboss5070") == true)
                            {
                                this.DelayForNextNavigation(this.IeHost, 0x3e8, 0x7d0);

                                break;
                            }
                            else
                                ;
                        }
                    else
                            ;
                } else if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("/subscriptions.html"))
                    {
                    if (IsDocumentValidate == true)
                        // EPC
                        foreach (HtmlElement htmlElement in this.InternetExplorer.Document.GetElementsByTagName("form")) {
                            if (CatalogApi.Autocomplit.ClickEPCSubmit(htmlElement) == true) {
                                this.DelayForNextNavigation(this.IeHost, 3000, 4000);

                                break;
                            } else
                                ;
                        }
                    else
                        ;
                }
                else
                    ;
            } catch (Exception ex) {
                logging(ex);
            }
        }

        private void DelayForNextNavigation(UIElement uie, int min, int max)
        {
            new System.Threading.Thread(() => {
                try {
                    Thread.Sleep(new Random().Next(min, max));
                    this.Dispatcher.Invoke(new Action(delegate
                    {
                        uie.Visibility = Visibility.Visible;
                    }), new object[0]);
                } catch (Exception ex) {
                    System.Windows.MessageBox.Show(string.Format("[{0}] {1} / {2}", DateTime.Now, ex.Message, ex.StackTrace));
                }
            }).Start();
        }

        private void logging(string mes)
        {
            using (FileStream fileStream = new FileStream(string.Format("{0}.log", System.IO.Path.GetFileNameWithoutExtension(Assembly.GetAssembly(this.GetType()).FullName)), FileMode.Append, FileAccess.Write)) {
                using (StreamWriter streamWriter = new StreamWriter(fileStream)) {
                    streamWriter.WriteLine(mes);
                }
            }
        }

        private void logging(Exception e)
        {
            logging(string.Format("[{0}] {1} / {2}"
                , DateTime.Now
                , e.Message
                , e.StackTrace
            ));
        }
    }
}
