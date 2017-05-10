using System;
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

                //Logging("Browser::ctor () - new WindowManager() - ???...");
                this.windowManager = new WindowManager();
                Logging(string.Format("{1}{0}Browser::ctor () - new WindowManager([DateTime={2}]) - processing.."
                    , Environment.NewLine
                    , string.Concat(Enumerable.Repeat("*---", 16))
                    , DateTime.UtcNow));

                Task.Factory.StartNew(delegate
                {
                    //string fileNameSession = "Session_ChevroletOpelGroup.txt";
                    string[] commandLineArgs = Environment.GetCommandLineArgs();
                    string fileNameSession;
                    string cookies = string.Empty;
                    if ((commandLineArgs.Length > 2)
                        && (commandLineArgs[1].Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot) == true)
                        && ((string.IsNullOrWhiteSpace(commandLineArgs[2])) == false)) {
                        base.Dispatcher.BeginInvoke(new Action(delegate
                        {
                            base.Title = "***Chevrolet-Opel Dealer Online***";
                        }), new object[0]);

                        fileNameSession = commandLineArgs[2];
                        Logging(String.Format("Browser::ctor () - read local session settings(file={0}) - ???..."
                            , fileNameSession));
                        if (File.Exists(fileNameSession) == true) {
                            using (FileStream fileStream =
                                new FileStream(fileNameSession, FileMode.Open, FileAccess.Read)) {
                                using (StreamReader streamReader = new StreamReader(fileStream)) {
                                    cookies = streamReader.ReadToEnd();
                                }
                            }
                            Logging(String.Format(
                                "Browser::ctor () - read local session settings(file={0}) - success..."
                                , fileNameSession));
                        } else
                            Logging(String.Format(
                                "Browser::ctor () - read local session settings(file={0}) - not exists..."
                                , fileNameSession));
                    } else
                        ;

                    if (string.IsNullOrEmpty(cookies) == false) {
                        Logging(String.Format("Browser::ctor (cookies={0}) - cookies not empty..."
                            , cookies));

                        string urlSetCookie = string.Format("{0}/", commandLineArgs[1]) // /
                            , urlNavigateDoLogin = string.Format("{0}/users/login.html", commandLineArgs[1]); // /users/login.html

                        Logging(String.Format("Url to SetCookie (Url={0}) - ..."
                            , urlSetCookie));

                        Logging(String.Format("Url to navigate do login (Url={0}) - ..."
                            , urlNavigateDoLogin));

                        List<Cookie> list;
                        try {
                            list = JsonConvert.DeserializeObject<List<Cookie>>(cookies);

                            Logging(String.Format("Cookies DeserializeObject ... (Length={0})", list.Count));
  
                            foreach (Cookie c in list) {
                                Logging(String.Format("Browser::ctor () - InternetSetCookie to={0}, key={1}, value={2}..."
                                    , urlSetCookie
                                    , c.Name
                                    , c.Value));

                                if (MainWindow.InternetSetCookie(urlSetCookie
                                    , c.Name,
                                    c.Value) == false)
                                    Logging(string.Format("::InternetSetCookie () - ошибка..."));
                                else
                                    ;
                            }
                        } catch (Exception ex)
                        {
                            Logging(ex);
                        }

                        Logging(String.Format("Browser to Navigate (Url={0}) - ..."
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
                            , Environment.NewLine
                            , commandLineArgs.Length
                            , commandLineArgs.Length > 1 ? commandLineArgs[1] : "отсутсвует"
                            , commandLineArgs.Length > 2 ? commandLineArgs[2] : "отсутсвует"));
                    }
                });
            } catch (Exception ex) {
                Logging(ex);
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

            //Logging(string.Format("Browser::StartNewEventSession (Title={0}) - ...", this.Title));
        }

        private void InternetExplorerOnQuit(object sender, EventArgs eventArgs)
        {
            Logging(string.Format("Browser::InternetExplorerOnQuit (Title={0}) - ...", this.Title));

            Close();
        }

        private void InternetExplorerOnNavigating(object sender, WebBrowserNavigatingEventArgs webBrowserNavigatingEventArgs)
        {
            Logging(string.Format("Browser::InternetExplorerOnNavigating (Title={0}, AbsoluteUri={1}) - ..."
                , this.Title
                , webBrowserNavigatingEventArgs.Url.AbsoluteUri));

            if ((webBrowserNavigatingEventArgs.Url.AbsoluteUri.Contains("spongepc.xw.gm.com/CStoneEPC"))
                    && (webBrowserNavigatingEventArgs.Url.AbsoluteUri.Contains("/logout?silent"))) {
                if (IsDocumentValidate == true)
                    Close();
                else
                    ;
            } else
                ;
        }

        private void InternetExplorerOnStartNewWindow(object sender, BrowserExtendedNavigatingEventArgs browserExtendedNavigatingEventArgs)
        {
            Logging(string.Format("Browser::InternetExplorerOnStartNewWindow (Title={0}, argUrl={1}) - ..."
                , this.Title
                , browserExtendedNavigatingEventArgs.Url.ToString()));

            if ((browserExtendedNavigatingEventArgs.Url == null)
                || (browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("about:blank") == false)) {
                if (ValidateUrlToNavigating(browserExtendedNavigatingEventArgs.Url) == false) {
                    browserExtendedNavigatingEventArgs.Cancel = true;
                } else {
                    if ((!(browserExtendedNavigatingEventArgs.Url == null))
                        && (browserExtendedNavigatingEventArgs.Url.LocalPath.Contains(CatalogApi.Catalogs.Blanket) == true)) {
                        base.Close();
                    } else
                        ;

                    if ((!(browserExtendedNavigatingEventArgs.Url == null))
                        && (browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("navlevel=year&action=navigate&aid=epc&fid=nav"))) {
                        base.Close();
                    } else
                        ;

                    ExtendedWebBrowser extendedWebBrowser = this.WindowManager.New(false, base.Title);
                    browserExtendedNavigatingEventArgs.AutomationObject = extendedWebBrowser.Application;
                }
            } else
                ;
        }
        /// <summary>
        /// Проверить наличие содержимого окна (повтор для Геко)
        /// </summary>
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

        private bool ValidateUrlToNavigating (Uri uri)
        {
            return !((uri != null)
                    && ((((uri.AbsoluteUri.Contains("fid=notify")
                    || uri.AbsoluteUri.Contains("fid=news"))
                    || (uri.AbsoluteUri.Contains("fid=bulletinboard")
                    || uri.AbsoluteUri.Contains("fid=feedback")))
                    || ((uri.AbsoluteUri.Contains("fid=about")
                    || uri.AbsoluteUri.Contains("fid=downloads"))
                    || (uri.AbsoluteUri.Contains("fid=ug")
                    || uri.AbsoluteUri.Contains("/privacy/"))))
                    || uri.AbsoluteUri.Contains("fid=help")));
        }

        private void InternetExplorerOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs webBrowserDocumentCompletedEventArgs)
        {
            Logging(string.Format("Browser::InternetExplorerOnDocumentCompleted (Title={0}, argUrl={1}) - ..."
                , this.Title
                , webBrowserDocumentCompletedEventArgs.Url.ToString()));

            try {
                // Credentials
                /*if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("/users/login.html") == true) {
                    if (IsDocumentValidate == true)
                        foreach (HtmlElement htmlElement in InternetExplorer.Document.GetElementsByTagName("form")) {
                            if (CatalogApi.Autocomplit.TypeCredentials(this.InternetExplorer.Document
                                , htmlElement
                                , "logon", "avtoritetepc", "password", "Hugoboss5070") == true) {
                                this.DelayForNextNavigation(this.IeHost, 1000, 2000);

                                break;
                            }
                            else
                                ;
                        }
                    else
                        ;
                } else*/ if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("/index.html") == true) {
                    if (IsDocumentValidate == true) {
                        this.InternetExplorer.Navigate(webBrowserDocumentCompletedEventArgs.Url.ToString()
                            .Replace("/index.html", "/subscriptions.html"));

                        this.DelayForNextNavigation(this.IeHost, 1000, 2000);
                    } else
                        ;
                }
                // EPC
                else if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("/subscriptions.html")) {
                    if (IsDocumentValidate == true)
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
                Logging(ex);
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

        public static void Logging(Exception e)
        {
            Logging(string.Format("{0} / {1}"
                    , e.Message
                    , e.StackTrace
            ));
        }

        public static void Logging(string mes)
        {
            var location = new Uri(Assembly.GetEntryAssembly().GetName().CodeBase);
            FileInfo appFileInfo = new FileInfo(location.AbsolutePath);

            using (FileStream fileStream = new FileStream(string.Format("{0}.log", System.IO.Path.GetFileNameWithoutExtension(appFileInfo.FullName)), FileMode.Append, FileAccess.Write)) {
                using (StreamWriter streamWriter = new StreamWriter(fileStream)) {
                    streamWriter.WriteLine(string.Format("[{1:o}]{0}{2}"
                        , Environment.NewLine
                        , DateTime.Now
                        , mes));
                }
            }
        }
    }
}
