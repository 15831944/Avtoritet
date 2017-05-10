using Gecko;
using Gecko.DOM;
using Gecko.Events;
using mshtml;
using NewLauncher.Extension;
using NewLauncher.Helper;
using NewLauncher.Interop;
using NewLauncher.Manager;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Markup;

namespace NewLauncher.View
{
    /// <summary>
    /// Логика взаимодействия для BrowserLauncherView.xaml
    /// </summary>
    public partial class BrowserLauncherView : Window
    {
        private bool cargobullPageLoading;
        private int count_show = 0;
        private bool flag1 = false;
        private bool languageChanged;
        private readonly string login;
        private int onlyOnceEntrance;
        private readonly string password;
        private SystemTime time;
        private readonly string url;
        public static string UrlBmwSession = "";
        private bool viewChanged;
        public NewLauncher.Extension.WindowManager WindowManager { get; set; }

        public BrowserLauncherView(SystemTime startTime, string titleBrand, string url, string login, string password)
        {
            try
            {
                this.InitializeComponent();
                this.time = startTime;
                base.Title = titleBrand;
                this.url = url;
                this.login = login;
                this.password = password;
                this.WindowManager = new NewLauncher.Extension.WindowManager();
                StartNewEventSession();
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("BrowserLauncherView", exception.Message + " | " + exception.StackTrace);
                System.Windows.MessageBox.Show(exception.Message + " | " + exception.StackTrace);
            }
        }

        private void StartNewEventSession()
        {
            base.Loaded += new RoutedEventHandler(this.OnLoaded);
            base.Closing += new CancelEventHandler(this.OnClosing);
            base.KeyDown += new System.Windows.Input.KeyEventHandler(this.OnKeyDown);
        }

        private void ClearCookies()
        {
            Xpcom.QueryInterface<nsICookieManager>(Xpcom.GetService<nsICookieManager>("@mozilla.org/cookiemanager;1")).RemoveAll();
        }

        private static double DateTimeToUnixTime(DateTime date)
        {
            TimeSpan span = (TimeSpan)(date - new DateTime(0x7b2, 1, 1, 0, 0, 0, 0));
            return Math.Floor(span.TotalSeconds);
        }

        private void DelayForNextNavigation(UIElement uie, int min, int max)
        {
            new Thread(() =>
            {
                Action method = null;
                try
                {
                    Thread.Sleep(new Random().Next(min, max));
                    if (method == null)
                    {
                        method = () => uie.Visibility = Visibility.Visible;
                    }
                    this.Dispatcher.Invoke(method, new object[0]);
                }
                catch (Exception exception)
                {
                    ErrorLogHelper.AddErrorInLog("DelayForNextNavigation", exception.Message + " | " + exception.StackTrace);
                    System.Windows.MessageBox.Show(string.Format("[{0}] {1} / {2}", DateTime.Now, exception.Message, exception.StackTrace));
                }
            }).Start();
        }

        private void Document_ContextMenuShowing(object sender, HtmlElementEventArgs e)
        {
            e.ReturnValue = false;
        }

        private static void FilterContent(GeckoDomDocument document, string tag, string attr)
        {
            Func<GeckoHtmlElement, bool> predicate = null;
            if (string.Equals(attr, "empty"))
            {
                foreach (GeckoHtmlElement element in document.GetElementsByTagName("li"))
                {
                    if (element.InnerHtml.Contains("avtoritet.epc@gmail.com"))
                    {
                        element.InnerHtml = element.InnerHtml.Replace("avtoritet.epc@gmail.com", string.Empty);
                    }
                }
            }
            else if (string.IsNullOrEmpty(attr))
            {
                foreach (GeckoHtmlElement element in document.GetElementsByTagName(tag))
                {
                    element.Style.SetPropertyValue("display", "none");
                }
            }
            else
            {
                if (predicate == null)
                {
                    predicate = x => ((x != null) && x.Id.Contains(attr)) || ((x != null) && x.ClassName.Contains(attr));
                }
                foreach (GeckoHtmlElement element in document.GetElementsByTagName(tag).Where<GeckoHtmlElement>(predicate))
                {
                    element.Style.SetPropertyValue("display", "none");
                }
            }
        }

        private void GeckoWeb_DomClick(object sender, DomMouseEventArgs e)
        {
            FilterContent(this.GeckoWeb.Window.Document, "img", "imgBtn order");
            FilterContent(this.GeckoWeb.Window.Document, "img", "imgBtn myParts");
            FilterContent(this.GeckoWeb.Window.Document, "img", "imgBtn compare");
            FilterContent(this.GeckoWeb.Window.Document, "span", "WORDSDELETE");
            foreach (GeckoHtmlElement element in from x in this.GeckoWeb.Window.Document.GetElementsByTagName("div")
                                                 where (x != null) && x.Id.Contains("VinModel_subInfoMenuDIV")
                                                 select x)
            {
                element.Style.SetPropertyValue("top", "0px");
                element.Style.SetPropertyValue("left", "0px");
                element.Style.SetPropertyValue("width", "0px");
                element.Style.SetPropertyValue("height", "0px");
            }
        }

        private void GeckoWebOnCreateWindow2(object sender, GeckoCreateWindow2EventArgs geckoCreateWindow2EventArgs)
        {
            try
            {
                BrowserDialogView view;
                if (geckoCreateWindow2EventArgs.Uri.Contains(CatalogApi.UrlConstants.Partslink24Root) == true)
                {
                    if ((geckoCreateWindow2EventArgs.Uri != null) && geckoCreateWindow2EventArgs.Uri.Contains("popupcheck.html"))
                    {
                        geckoCreateWindow2EventArgs.Cancel = false;
                    }
                    else
                    {
                        geckoCreateWindow2EventArgs.Cancel = true;
                        view = new BrowserDialogView
                        {
                            IeHost = { Visibility = Visibility.Collapsed },
                            GeckoHost = { Visibility = Visibility.Visible },
                            Width = 700.0,
                            Height = 600.0,
                            WindowState = WindowState.Normal,
                            WindowStartupLocation = WindowStartupLocation.CenterOwner
                        };
                        view.GeckoWeb.Navigate(geckoCreateWindow2EventArgs.Uri);
                        view.Show();
                    }
                }

                if (geckoCreateWindow2EventArgs.Uri.Contains(CatalogApi.UrlConstants.PeugeotRoot)
                    || geckoCreateWindow2EventArgs.Uri.Contains(CatalogApi.UrlConstants.CitroenRoot))
                {
                    geckoCreateWindow2EventArgs.Cancel = true;
                    view = new BrowserDialogView
                    {
                        IeHost = { Visibility = Visibility.Collapsed },
                        GeckoHost = { Visibility = Visibility.Visible },
                        WindowState = WindowState.Maximized,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    view.GeckoWeb.Navigate(geckoCreateWindow2EventArgs.Uri);
                    view.Show();
                }

                if (geckoCreateWindow2EventArgs.Uri.Contains(CatalogApi.UrlConstants.MazdaRoot) == true) {
                    geckoCreateWindow2EventArgs.Cancel = true;
                } else
                    ;

                if (geckoCreateWindow2EventArgs.Uri.Contains("payment")
                    || geckoCreateWindow2EventArgs.Uri.Contains("VendorLogin"))
                {
                    geckoCreateWindow2EventArgs.Cancel = true;
                }

                if (geckoCreateWindow2EventArgs.Uri.Contains(CatalogApi.UrlConstants.SsangYongRoot) == true)
                {
                    geckoCreateWindow2EventArgs.Cancel = true;
                    view = new BrowserDialogView
                    {
                        IeHost = { Visibility = Visibility.Collapsed },
                        GeckoHost = { Visibility = Visibility.Visible },
                        WindowState = WindowState.Maximized,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner
                    };
                    view.GeckoWeb.Navigate(geckoCreateWindow2EventArgs.Uri);
                    view.Show();
                }
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("GeckoWebOnCreateWindow2", exception.Message + " | " + exception.StackTrace);
                System.Windows.MessageBox.Show(string.Format("[{0}] {1} / {2}", DateTime.Now, exception.Message, exception.StackTrace));
            }
        }

        private void GeckoWebOnDocumentCompleted(object sender, GeckoDocumentCompletedEventArgs geckoEventArgs)
        {
            try
            {
                GeckoElement element3;
                GeckoHtmlElement element;
                GeckoHtmlElement[] elementArray;
                IEnumerable<GeckoElement> enumerable;

                this.Captcha.Text = "Загрузка...";
                this.Message.Text = "Пожалуйста, ждите...";

#if DEBUG
                MainWindow.Logging(string.Format("Browser::GeckoWebOnDocumentCompleted (Title={0}, AbsoluteUri={1}) - ..."
                    , this.Title
                    , geckoEventArgs.Uri.AbsoluteUri));
#endif

                #region wpc.mobis.co.kr
                if ((this.url.Contains(CatalogApi.UrlConstants.Kia_Http_root) == true)
                    || (this.url.Contains(CatalogApi.UrlConstants.HyundaiRoot) == true)) {
                    if ((from x in this.GeckoWeb.Document.GetElementsByTagName("form")
                        where x.GetAttribute("name").Equals("CheckForm")
                        select x).Any<GeckoHtmlElement>()) {
                        GeckoElement elementById = this.GeckoWeb.Document.GetElementById("chkID");
                        if (elementById != null) {
                            elementById.SetAttribute("value", this.login);
                        }
                        GeckoElement element2 = this.GeckoWeb.Document.GetElementById("chkPW");
                        if (element2 != null) {
                            element2.SetAttribute("value", this.password);
                        }
                        element3 = this.GeckoWeb.Document.GetElementById("logon");
                        if (element3 == null) {
                            return;
                        }
                        new GeckoInputElement(element3.DomObject).Click();
                    } else {
                        FilterContent(geckoEventArgs.Window.Document, "a", "Searching-CKD");
                        FilterContent(geckoEventArgs.Window.Document, "a", "Searching-PDF");
                        FilterContent(geckoEventArgs.Window.Document, "a", "Searching-Tools");
                        FilterContent(geckoEventArgs.Window.Document, "a", "Searching-MyParts");
                        FilterContent(geckoEventArgs.Window.Document, "a", "Searching-Memo");
                        FilterContent(geckoEventArgs.Window.Document, "span", "USERMANUAL");
                        FilterContent(geckoEventArgs.Window.Document, "span", "WORDSDELETE");
                        FilterContent(geckoEventArgs.Window.Document, "span", "USERINFO");
                        FilterContent(geckoEventArgs.Window.Document, "span", "LOGOUT");
                        this.DelayForNextNavigation(this.GeckoHost, 0x1388, 0x1b58);
                    }
                }
                #endregion

                #region partslink24
                if (this.url.Contains(CatalogApi.UrlConstants.Partslink24Root))
                {
                    if (this.PartsLink24NeedRefresh(geckoEventArgs.Uri) == true) {
                        if (this.SetCookiesToPath(string.Format(".{0}.com", CatalogApi.UrlConstants.Partslink24Root)) == true) {
                            //this.GeckoWeb.Navigate(geckoEventArgs.Uri.AbsoluteUri);
                            this.GeckoWeb.Navigate(this.url);
                        } else {
                            this.Captcha.Text = "Ошибка подключения!";
                            this.Message.Text = "Сервер не доступен, попробуйте подключиться позже...";
                            this.GeckoHost.Visibility = Visibility.Collapsed;
                            return;
                        }
                    } else
                        ;

                    if (this.PartsLink24NeedNavigation(geckoEventArgs.Uri)) {
                        this.GeckoWeb.Navigate(this.url);
                    } else
                        ;

                    if (this.PartsLink24PageHasDemoString(this.GeckoWeb.Document.Body.OuterHtml)) {
                        this.Captcha.Text = "Тайм-аут сессии";
                        this.Message.Text = "ДЕМО-режим. Перезапустите web-браузер";
                        this.GeckoHost.Visibility = Visibility.Collapsed;
                    } else
                        ;
                    
                    if (this.PartsLink24PageHasHeaderLogoString(this.GeckoWeb.Document.Body.OuterHtml)) {
                        GeckoElement element4 = this.GeckoWeb.Document.GetElementById("headerLogo");
                        if (element4 != null) {
                            element4.SetAttribute("style", "display:none");
                        } else
                            ;

                        GeckoElement element5 = this.GeckoWeb.Document.GetElementById("headerLinks");
                        if (element5 != null) {
                            element5.SetAttribute("style", "display:none");
                        } else
                            ;

                        GeckoElement element6 = this.GeckoWeb.Document.GetElementById("headerText");
                        if (element6 != null) {
                            element6.SetAttribute("style", "display:none");
                        } else
                            ;

                        GeckoElement element7 = this.GeckoWeb.Document.GetElementById("portalMenu");
                        if (element7 != null) {
                            element7.SetAttribute("style", "display:none");
                        } else
                            ;

                        GeckoElement element8 = this.GeckoWeb.Document.GetElementById("main-content");
                        if (element8 != null) {
                            element8.SetAttribute("style", "display:none");
                        } else
                            ;

                        GeckoElement element9 = this.GeckoWeb.Document.GetElementById("otherInfoBox");
                        if (element9 != null) {
                            element9.SetAttribute("style", "display:none");
                        } else
                            ;
                    } else {
                        if (geckoEventArgs.Window.Frames.Count > 0)
                        {
                            foreach (GeckoWindow window in geckoEventArgs.Window.Frames)
                            {
                                if (this.PartsLink24PageHasDemoString(window.Document.ActiveElement.OuterHtml))
                                {
                                    this.Captcha.Text = "Тайм-аут сессии";
                                    this.Message.Text = "Перезапустите web-браузер";
                                    return;
                                }
                                if (string.Compare(window.Name, "PARTS_HEAD", StringComparison.InvariantCultureIgnoreCase) == 0)
                                {
                                    FilterContent(window.Document, "div", "cartInfo");
                                    FilterContent(window.Document, "div", "pl24buttonRowHead");
                                }
                                if ((window.Name.Contains("PARTS_ITEMLIST") || window.Name.Contains("PARTS_PHYSICAL")) || window.Name.Contains("PARTS_ENTRIES"))
                                {
                                    FilterContent(window.Document, "ul", string.Empty);
                                }
                            }
                        }
                        else
                        {
                            string outerHtml = geckoEventArgs.Window.Document.ActiveElement.OuterHtml;
                            if (outerHtml.Contains("ERROR 500") || outerHtml.Contains("System Error"))
                            {
                                geckoEventArgs.Window.Document.ActiveElement.InnerHtml = string.Empty;
                            }
                            GeckoNode node = this.GeckoWeb.Document.GetElementsByClassName("ui-dialog ui-widget ui-widget-content ui-corner-all ui-draggable ui-resizable ui-dialog-buttons").FirstOrDefault<GeckoNode>();
                            if (node != null)
                            {
                                this.GeckoWeb.Document.Body.RemoveChild(node);
                            }
                            GeckoNodeCollection elementsByClassName = this.GeckoWeb.Document.GetElementsByClassName("ui-dialog-title");
                            if (elementsByClassName != null)
                            {
                                foreach (GeckoNode node2 in elementsByClassName)
                                {
                                    if (!(string.IsNullOrEmpty(node2.TextContent) || !node2.TextContent.Contains("partslink")))
                                    {
                                        node2.TextContent = node2.TextContent.Replace("partslink24 - ", string.Empty);
                                    }
                                }
                            }
                            if (this.GeckoWeb.Document.Body.OuterHtml.Contains("headerLogo"))
                            {
                                FilterContent(this.GeckoWeb.Document, "div", "headerLogo");
                                FilterContent(this.GeckoWeb.Document, "div", "headerLinks");
                                FilterContent(this.GeckoWeb.Document, "div", "headerText");
                                FilterContent(this.GeckoWeb.Document, "div", "portalMenu");
                                FilterContent(this.GeckoWeb.Document, "div", "main-content");
                                FilterContent(this.GeckoWeb.Document, "div", "brandsBox");
                                FilterContent(this.GeckoWeb.Document, "div", "otherInfoBox");
                            }
                            if ((geckoEventArgs.Window.Name == "PARTS_ITEMLIST") || (geckoEventArgs.Window.Name == "PARTS_ENTRIES"))
                            {
                                FilterContent(this.GeckoWeb.Document, "ul", "iconList");
                                FilterContent(geckoEventArgs.Window.Document, "ul", "iconList");
                            }
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-1");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-2");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-3");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-4");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-5");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-6");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-7");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-8");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-9");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-10");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-11");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-12");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-13");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-14");
                            FilterContent(this.GeckoWeb.Document, "span", "uie-id-15");
                            FilterContent(this.GeckoWeb.Document, "div", "cartInfo");
                            FilterContent(this.GeckoWeb.Document, "div", "pl24buttonRowHead");
                            FilterContent(this.GeckoWeb.Document, "span", "listActionButton partinfoButton");
                            FilterContent(this.GeckoWeb.Document, "div", "pl24TopLinks");
                            FilterContent(this.GeckoWeb.Document, "span", "partinfoButton");
                            FilterContent(this.GeckoWeb.Document, "ul", string.Empty);
                            foreach (GeckoHtmlElement element10 in from f in this.GeckoWeb.Document.DefaultView.Frames
                                                                   select from e in f.Frames
                                                                          where string.Compare(e.Name, "PARTS_PHYSICAL", StringComparison.InvariantCultureIgnoreCase) == 0
                                                                          select e.Document.GetElementsByTagName("ul"))
                            {
                                element10.Style.SetPropertyValue("display", "none");
                            }
                        }
                        this.DelayForNextNavigation(this.GeckoHost, 2000, 3000);
                    }
                }
                #endregion

                #region Peugeot
                // в OnLoaded
                #endregion

                #region Citroen
                // в OnLoaded
                #endregion

                #region Saf-axles
                if (this.url.Contains(string.Format("{0}", CatalogApi.UrlConstants.SafAxlesRoot)) == true) {
                    this.DelayForNextNavigation(this.GeckoHost, 300, 500);
                } else
                    ;
                #endregion

                #region Inforanger.Roadrenger
                if (this.url.Contains(string.Format("{0}", CatalogApi.UrlConstants.RangerRoot)) == true) {
                    FilterContent(this.GeckoWeb.Document, "td", "nav_top");
                    this.DelayForNextNavigation(this.GeckoHost, 300, 500);
                } else
                    ;
                #endregion

                #region Gigant-group
                if (this.url.Contains(string.Format("{0}.com", CatalogApi.UrlConstants.GigantGroupRoot)) == true) {
                    this.DelayForNextNavigation(this.GeckoHost, 300, 500);
                } else
                    ;
                #endregion

                #region ssangyong
                if (this.url.Contains(CatalogApi.UrlConstants.SsangYongRoot) == true)
                {
                    GeckoElement element11 = this.GeckoWeb.Document.GetElementById("login");
                    if (element11 != null) {
                        element11.SetAttribute("value", this.login);
                    } else
                        ;

                    GeckoElement element12 = this.GeckoWeb.Document.GetElementById("pass");
                    if (element12 != null) {
                        element12.SetAttribute("value", this.password);
                    } else
                        ;

                    element3 = this.GeckoWeb.Document.GetElementById("loginOrRegistration");
                    if (element3 != null)
                    {
                        new GeckoInputElement(element3.DomObject).Click();
                    } else {
                        FilterContent(this.GeckoWeb.Document, "div", "news");
                        FilterContent(this.GeckoWeb.Document, "div", "footer");
                        FilterContent(this.GeckoWeb.Document, "li", "link_bug");
                        FilterContent(this.GeckoWeb.Document, "li", "exit");
                        FilterContent(this.GeckoWeb.Document, "li", "empty");

                        if (this.GeckoWeb.Document.Body.OuterHtml.Contains("Купить")) {
                            this.GeckoWeb.Document.Body.InnerHtml = this.GeckoWeb.Document.Body.InnerHtml.Replace("Купить", string.Empty);
                        } else
                            ;

                        this.DelayForNextNavigation(this.GeckoHost, 300, 500);
                    }
                }
                #endregion
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("GeckoWebOnDocumentCompleted", exception.Message + " | " + exception.StackTrace);
                this.Captcha.Text = "Ошибка подключения!";
                this.Message.Text = "Сервер не доступен, попробуйте подключиться позже...";
                this.GeckoHost.Visibility = Visibility.Collapsed;
            }
        }

        private void GeckoWebOnDomClick(object sender, DomMouseEventArgs domMouseEventArgs)
        {
            try
            {
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("GeckoWebOnDomClick", exception.Message + " | " + exception.StackTrace);
                System.Windows.MessageBox.Show(string.Format("[{0}] {1} / {2}", DateTime.Now, exception.Message, exception.StackTrace));
            }
        }

        private void GeckoWebOnDomContentLoaded(object sender, DomEventArgs domEventArgs)
        {
            GeckoHtmlElement element;
            GeckoHtmlElement[] elementArray;
            IEnumerable<GeckoElement> enumerable;

            #region Citroen
            if (this.url.Contains(CatalogApi.UrlConstants.CitroenRoot)) {
                if ((this.GeckoWeb.Document.Body.InnerHtml.IndexOf("Your session time expired. Please reconnect.") > 0)
                    || (this.GeckoWeb.Url.AbsoluteUri.Contains("index.jsp") == true)) {
                    if (this.RefreshSession(string.Format(".{0}.com", CatalogApi.UrlConstants.CitroenRoot)) == true) {
                        this.GeckoWeb.Navigate(this.url);
                    } else
                        ;
                } else if (this.GeckoWeb.Document.Body.InnerHtml.IndexOf("A technical problem occurred. Please retry to connect later.") > 0) {
                    this.DelayForNextNavigation(this.GeckoHost, 1000, 2000);
                } else {
                    element = this.GeckoWeb.Document.GetElementsByTagName("span").First<GeckoHtmlElement>(x => x.Id == "libelleflag");
                    if (!((element == null) || element.OuterHtml.Contains("русский"))) {
                        this.GeckoWeb.Navigate("javascript:validChoixLangue('ru_RU')");
                    } else {
                        elementArray = (from x in this.GeckoWeb.Document.GetElementsByTagName("ul")
                                        where x.Id.Contains("menu")
                                        select x).ToArray<GeckoHtmlElement>();
                        if (elementArray.Length > 0) {
                            enumerable = elementArray[0].GetElementsByTagName("li").Skip<GeckoElement>(3);
                            foreach (GeckoElement element2 in enumerable) {
                                element2.SetAttribute("style", "display:none");
                            }
                        }

                        FilterContent(this.GeckoWeb.Document, "a", "bandeau_panier");
                        foreach (GeckoHtmlElement element4 in from x in this.GeckoWeb.Document.GetElementsByTagName("div")
                                                              where (((x.Id.Contains("tools") || x.Id.Contains("aide")) || (x.Id.Contains("contact") || x.Id.Contains("param"))) || (x.Id.Contains("promos") || x.Id.Contains("contenu"))) || x.Id.Contains("ip")
                                                              select x) {
                            element4.Style.SetPropertyValue("display", "none");
                        }

                        this.DelayForNextNavigation(this.GeckoHost, 1000, 2000);
                    }
                }
            }
            #endregion

            #region Peugeot
            if (this.url.Contains(CatalogApi.UrlConstants.PeugeotRoot)) {
                if ((this.GeckoWeb.Document.Body.InnerHtml.IndexOf("Your session time expired. Please reconnect.") > 0)
                    || (this.GeckoWeb.Url.AbsoluteUri.Contains("index.jsp") == true)) {
                    if (this.RefreshSession(string.Format(".{0}.com", CatalogApi.UrlConstants.PeugeotRoot)) == true) {
                        this.GeckoWeb.Navigate(this.url);
                    } else
                        ;
                } else if (this.GeckoWeb.Document.Body.InnerHtml.IndexOf("A technical problem occurred. Please retry to connect later.") > 0) {
                    this.DelayForNextNavigation(this.GeckoHost, 1000, 2000);
                } else {
                    //if (this.GeckoWeb.Document.Body.InnerHtml.IndexOf("A technical problem occurred. Please retry to connect later.") > 0) {
                    //    this.DelayForNextNavigation(this.GeckoHost, 1000, 2000);
                    //    return;
                    //} else
                    //    ;

                    element = this.GeckoWeb.Document.GetElementsByTagName("span").First<GeckoHtmlElement>(x => x.Id == "libelleflag");
                    if (!((element == null) || element.OuterHtml.Contains("русский"))) {
                        this.GeckoWeb.Navigate("javascript:validChoixLangue('ru_RU')");
                    } else {
                        elementArray = (from x in this.GeckoWeb.Document.GetElementsByTagName("ul")
                                        where x.Id.Contains("menu")
                                        select x).ToArray<GeckoHtmlElement>();
                        if (elementArray.Length > 0) {
                            enumerable = elementArray[0].GetElementsByTagName("li").Skip<GeckoElement>(3);
                            foreach (GeckoElement element2 in enumerable) {
                                element2.SetAttribute("style", "display:none");
                            }
                        }
                        FilterContent(this.GeckoWeb.Document, "a", "bandeau_panier");
                        foreach (GeckoHtmlElement element4 in from x in this.GeckoWeb.Document.GetElementsByTagName("div")
                                                              where (((x.Id.Contains("tools") || x.Id.Contains("aide")) || (x.Id.Contains("contact") || x.Id.Contains("param"))) || (x.Id.Contains("promos") || x.Id.Contains("contenu"))) || x.Id.Contains("ip")
                                                              select x) {
                            element4.Style.SetPropertyValue("display", "none");
                        }
                        this.DelayForNextNavigation(this.GeckoHost, 1000, 2000);
                    }
                }
            }
            #endregion
        }

        private void GeckoWebOnFrameNavigating(object sender, GeckoNavigatingEventArgs geckoNavigatingEventArgs)
        {
            try
            {
                this.GeckoHost.Visibility = Visibility.Collapsed;
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("GeckoWebOnFrameNavigating", exception.Message + " | " + exception.StackTrace);
                System.Windows.MessageBox.Show(string.Format("[{0}] {1} / {2}", DateTime.Now, exception.Message, exception.StackTrace));
            }
        }

        private void GeckoWebOnNavigating(object sender, GeckoNavigatingEventArgs geckoNavigatingEventArgs)
        {
#if DEBUG
            MainWindow.Logging(string.Format("Browser::GeckoWebOnNavigating (Title={0}, AbsoluteUri={1}) - ..."
                , this.Title
                , geckoNavigatingEventArgs.Uri.AbsoluteUri));
#endif

            if ((this.url.Contains(CatalogApi.UrlConstants.EtkaRoot) == true)
                && (geckoNavigatingEventArgs.Uri.AbsoluteUri.Contains(string.Format("{0}.ru/shop", CatalogApi.UrlConstants.EtkaRoot)) == true))
            {
                geckoNavigatingEventArgs.Cancel = true;
            }
            else
            {
                this.GeckoHost.Visibility = Visibility.Collapsed;
            }
        }

        private void GeckoWebOnShowContextMenu(object sender, GeckoContextMenuEventArgs geckoContextMenuEventArgs)
        {
            try
            {
                System.Windows.Forms.ContextMenu contextMenu = geckoContextMenuEventArgs.ContextMenu.GetContextMenu();
                if (contextMenu != null) {
                    foreach (System.Windows.Forms.MenuItem menuItem2 in from System.Windows.Forms.MenuItem menuItem in contextMenu.MenuItems
                                                                        where menuItem != null
                                                                        select menuItem) {
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
                } else
                    ;
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("GeckoWebOnShowContextMenu", exception.Message + " | " + exception.StackTrace);
                System.Windows.MessageBox.Show(string.Format("[{0}] {1} / {2}", DateTime.Now, exception.Message, exception.StackTrace));
            }

            //EventHandler onClick = null;
            //EventHandler handler2 = null;
            //try
            //{
            //    System.Windows.Forms.ContextMenu contextMenu = geckoContextMenuEventArgs.ContextMenu.GetContextMenu();
            //    if (contextMenu != null)
            //    {
            //        foreach (System.Windows.Forms.MenuItem item in from menuItem in contextMenu.MenuItems.Cast<System.Windows.Forms.MenuItem>()
            //                                                       where menuItem != null
            //                                                       select menuItem)
            //        {
            //            item.Enabled = false;
            //            item.Visible = false;
            //        }
            //        if (onClick == null)
            //        {
            //            onClick = (o, args) => this.GeckoWeb.CopySelection();
            //        }
            //        contextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Копировать", onClick));
            //        if (handler2 == null)
            //        {
            //            handler2 = (o, args) => this.GeckoWeb.Paste();
            //        }
            //        contextMenu.MenuItems.Add(new System.Windows.Forms.MenuItem("Вставить", handler2));
            //    }
            //}
            //catch (Exception exception)
            //{
            //    ErrorLogHelper.AddErrorInLog("GeckoWebOnShowContextMenu", exception.Message + " | " + exception.StackTrace);
            //    System.Windows.MessageBox.Show(string.Format("[{0}] {1} / {2}", DateTime.Now, exception.Message, exception.StackTrace));
            //}
        }

        private void GeckoWebOnWindowClosed(object sender, EventArgs eventArgs)
        {
            try
            {
            }
            catch (Exception)
            {
            }
        }

        private List<System.Net.Cookie> GetCookies(string geckoUrl)
        {
            string cookies = RequestHelper.Client.GetCookies(geckoUrl);

            NewLauncher.MainWindow.Logging(string.Format(@"BrowserLauncherView::GetCookies (Url={1}{0}, cookies={2})"
                , Environment.NewLine
                , geckoUrl
                , cookies));

            return JsonConvert.DeserializeObject<List<System.Net.Cookie>>(cookies);
        }

        private void IeWebOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs webBrowserDocumentCompletedEventArgs)
        {
            try
            {
                MainWindow.Logging(string.Format("Browser::IeWebOnDocumentCompleted (Title={0}, AbsoluteUri={1}) - ..."
                    , this.Title
                    , webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri));

                HtmlElement[] elementArray;
                HtmlElement elementById;
                HtmlElement element3;
                HtmlElement element4;

                this.IeWeb.Document.ContextMenuShowing += new HtmlElementEventHandler(this.Document_ContextMenuShowing);

                bool flag = false;
                this.Captcha.Text = "Загрузка...";
                this.Message.Text = "Пожалуйста, ждите...";

                if (this.url.Contains(CatalogApi.Catalogs.VolvoImpact))
                {
                    flag = true;
                    this.DelayForNextNavigation(this.IeHost, 2000, 3000);
                }

                if (this.url.Contains(CatalogApi.Catalogs.RenoImpact))
                {
                    flag = true;
                    this.DelayForNextNavigation(this.IeHost, 500, 3000);
                }

                #region bmwgroup
                if (this.url.Contains(CatalogApi.UrlConstants.BmwGroupRoot) == true)
                {
                    flag = true;
                    string absoluteUri = this.IeWeb.Document.Url.AbsoluteUri;
                    if (absoluteUri.Contains("PSESSID")) {
                        UrlBmwSession = absoluteUri;
                    } else
                        ;

                    if (!absoluteUri.Contains("PSESSID")
                        & (this.url == CatalogApi.UrlConstants.BMW_Internet))
                    {
                        bool flag2 = false;
                        if (!this.flag1)
                        {
                            IEnumerable<HtmlElement> source = from form in this.IeWeb.Document.GetElementsByTagName("input").Cast<HtmlElement>()
                                                              where form.GetAttribute("Name").Equals("LOGON_USERID")
                                                              select form;
                            elementArray = (source as HtmlElement[]) ?? source.ToArray<HtmlElement>();
                            if (elementArray.Any<HtmlElement>())
                            {
                                foreach (HtmlElement element in elementArray)
                                {
                                    element.SetAttribute("value", this.login);
                                }
                            }
                            IEnumerable<HtmlElement> enumerable2 = from form in this.IeWeb.Document.GetElementsByTagName("input").Cast<HtmlElement>()
                                                                   where form.GetAttribute("Name").Equals("LOGON_PASSWD")
                                                                   select form;
                            HtmlElement[] elementArray2 = (enumerable2 as HtmlElement[]) ?? enumerable2.ToArray<HtmlElement>();
                            if (elementArray2.Any<HtmlElement>())
                            {
                                foreach (HtmlElement element in elementArray2)
                                {
                                    element.SetAttribute("value", this.password);
                                    flag2 = true;
                                    this.flag1 = false;
                                }
                            }
                            if ((this.count_show == 0) && !flag2)
                            {
                                this.flag1 = true;
                                this.IeWeb.Navigate(UrlBmwSession);
                                return;
                            }
                            this.count_show++;
                        }
                        if (flag2)
                        {
                            this.IeWeb.Document.InvokeScript("CopyFields");
                        }
                        else if (!this.flag1)
                        {
                            this.IeWeb.Navigate(CatalogApi.UrlConstants.BMW_WebETKStartNodeRoot);
                            this.flag1 = true;
                        }
                    }
                }
                #endregion

                #region Ford
                if (this.url.Contains(CatalogApi.UrlConstants.FordRoot))
                {
                    flag = true;
                    if (this.IeWeb.Document != null)
                    {
                        IEnumerable<HtmlElement> enumerable3 = from form in this.IeWeb.Document.GetElementsByTagName("form").Cast<HtmlElement>()
                                                               where form.GetAttribute("name").Equals("LoginForm")
                                                               select form;
                        elementArray = (enumerable3 as HtmlElement[]) ?? enumerable3.ToArray<HtmlElement>();
                        if (elementArray.Any<HtmlElement>())
                        {
                            foreach (HtmlElement element in elementArray)
                            {
                                if (element.Document != null)
                                {
                                    elementById = element.Document.GetElementById("tUsername");
                                    if (elementById != null)
                                    {
                                        elementById.SetAttribute("value", this.login);
                                    }
                                    element3 = element.Document.GetElementById("tPassword");
                                    if (element3 != null)
                                    {
                                        element3.SetAttribute("value", this.password);
                                    }
                                    element4 = element.Document.GetElementById("tLoginButton");
                                    if (element4 != null)
                                    {
                                        element4.InvokeMember("click");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("IsoViewSplashLoad")) {
                            } else
                                ;

                            if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("VehicleSelectorList")) {
                                if (!this.viewChanged && (this.IeWeb.Document != null)) {
                                    IEnumerable<HtmlElement> enumerable4 = this.IeWeb.Document.GetElementsByTagName("a").Cast<HtmlElement>();
                                    foreach (HtmlElement element5 in from tag in enumerable4
                                                                     where (tag != null) && (tag.Id == "VisualAccessId")
                                                                     select tag) {
                                        element5.InvokeMember("click");
                                        return;
                                    }
                                } else
                                    ;

                                this.viewChanged = true;
                            } else
                                ;

                            if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("LoginSubmit")) {
                                if (!this.languageChanged) {
                                    Uri uri = new Uri(this.url);
                                    this.IeWeb.Navigate(uri.Scheme + "://" + uri.Authority + "/Ford/SessionData?language=ru&reload=true");
                                } else
                                    ;

                                this.languageChanged = true;
                                this.DelayForNextNavigation(this.IeHost, 0x1388, 0x1f40);
                            } else
                                ;
                        }
                    }
                }
                #endregion

                #region mazdaeur
                if (this.url.Contains(CatalogApi.UrlConstants.MazdaRoot))
                {
                    flag = true;
                    if (this.IeWeb.Document != null)
                    {
                        if ((this.IeWeb.Document.Body != null) && this.IeWeb.Document.Body.OuterHtml.Contains("class=\"errors\""))
                        {
                            return;
                        }
                        var element = this.IeWeb.Document.GetElementById("fm1");
                        if ((element != null) && (element.Document != null))
                        {
                            elementById = element.Document.GetElementById("username");
                            if (elementById != null)
                            {
                                elementById.SetAttribute("value", this.login);
                            }
                            element3 = element.Document.GetElementById("password");
                            if (element3 != null)
                            {
                                element3.SetAttribute("value", this.password);
                            }
                            element4 = element.Document.GetElementById("btnLogin");
                            if ((element4 != null) && (this.onlyOnceEntrance == 0))
                            {
                                this.onlyOnceEntrance++;
                                element4.InvokeMember("click");
                            }
                        }
                    }

                    if (((this.IeWeb.Document != null) && (this.IeWeb.Document.Url != null)) && this.IeWeb.Document.Url.AbsoluteUri.Contains("DisplayWarning.html"))
                    {
                        flag = true;
                        HtmlElement element6 = this.IeWeb.Document.GetElementsByTagName("head")[0];
                        HtmlElement newElement = this.IeWeb.Document.CreateElement("script");
                        if (newElement != null)
                        {
                            IHTMLScriptElement domElement = (IHTMLScriptElement)newElement.DomElement;
                            domElement.text = "function openCatalog() { Continue2(); }";
                            element6.AppendChild(newElement);
                            this.IeWeb.Document.InvokeScript("openCatalog");
                        }
                        this.DelayForNextNavigation(this.IeHost, 3000, 4000);
                    }
                    if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("servlet/BackHome"))
                    {
                        this.DelayForNextNavigation(this.IeHost, 1000, 2000);
                    }
                    if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("servlet/LoginEPC"))
                    {
                        this.DelayForNextNavigation(this.IeHost, 1000, 2000);
                    }
                    if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("servlet/DataMaintenance"))
                    {
                        this.DelayForNextNavigation(this.IeHost, 1000, 2000);
                    }
                }
                #endregion

                #region EWA-net
                if (this.url.Contains(CatalogApi.UrlConstants.MercedezRoot))
                {
                    flag = true;
                    if (this.IeWeb.Document != null)
                    {
                        HtmlElementCollection elementsByTagName = this.IeWeb.Document.GetElementsByTagName("form");
                        if (elementsByTagName.Count > 0)
                        {
                            foreach (HtmlElement element in elementsByTagName)
                            {
                                if (element.GetAttribute("name").Equals("userLogonForm", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    elementById = this.IeWeb.Document.GetElementsByTagName("input").Cast<HtmlElement>().First<HtmlElement>(x => x.GetAttribute("name").Equals("userid"));
                                    if (elementById != null)
                                    {
                                        elementById.SetAttribute("value", AccountManager.Account.Name);
                                    }
                                    element3 = this.IeWeb.Document.GetElementsByTagName("input").Cast<HtmlElement>().First<HtmlElement>(x => x.GetAttribute("name").Equals("password"));
                                    if (element3 != null)
                                    {
                                        element3.SetAttribute("value", AccountManager.Account.Password);
                                    }
                                    element4 = this.IeWeb.Document.GetElementsByTagName("input").Cast<HtmlElement>().First<HtmlElement>(x => x.GetAttribute("type").Equals("submit"));
                                    if (element4 != null)
                                    {
                                        element4.InvokeMember("click");
                                        this.DelayForNextNavigation(this.IeHost, 1000, 2000);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Chevrolet-Opel Group
                if (this.url.Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot) == true)
                {
                    flag = true;

                    if (((this.IeWeb.Document == null) || (this.IeWeb.Document.Body == null))
                        || ((this.IeWeb.Document.Window == null) || (this.IeWeb.Document.Window.Frames == null)))
                    {
                        return;
                    }
                    else
                        ;
                    // Navigate To scriptions - login is before success
                    if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("/index.html"))
                    {
                        string navigateToLogin = string.Empty;

                        navigateToLogin =
                            webBrowserDocumentCompletedEventArgs.Url.ToString().Replace("/index.html", "/subscriptions.html")
                                ;

                        this.IeWeb.Navigate(navigateToLogin);
                        this.DelayForNextNavigation(this.IeHost, 0x1388, 0x1f40);

                        return;
                    }
                    else
                        ;
                    //// Auto Log-on
                    //if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("/users/login.html"))
                    //    foreach (HtmlElement element in this.IeWeb.Document.GetElementsByTagName("form")) {
                    //        if (CatalogApi.Autocomplit.TypeCredentials(this.IeWeb.Document
                    //            , element
                    //            , "logon", login, "password", password) == true) {
                    //            this.DelayForNextNavigation(this.IeHost, 1000, 2000);

                    //            return;
                    //        } else
                    //            ;
                    //    } else
                    //    ;
                    // EPC
                    if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("/subscriptions.html"))
                        foreach (HtmlElement element in this.IeWeb.Document.GetElementsByTagName("form"))
                        {
                            if (CatalogApi.Autocomplit.ClickEPCSubmit(element) == true)
                            {
                                this.DelayForNextNavigation(this.IeHost, 1000, 2000);

                                return;
                            }
                            else
                                ;
                        }
                    else
                        ;
                    //??? Log Out
                    if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("/spongepc") == true)
                    {
                        if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("/logout") == true)
                        {
                            this.DelayForNextNavigation(this.IeHost, 1000, 2000);

                            this.Close();

                            return;
                        }
                        else
                            ;
                    }
                    else
                        ;
                }
                #endregion

                #region BMW Group
                if (this.url.Contains(CatalogApi.UrlConstants.BmwGroupRoot) == true)
                {
                    flag = true;
                    this.DelayForNextNavigation(this.IeHost, 1000, 2000);
                }
                else
                    ;
                #endregion

                #region Catalog locale
                if (
                        (
                            (
                                (
                                    (this.url.Contains(CatalogApi.Catalogs.AlfaRomeo)
                                        || this.url.Contains(CatalogApi.Catalogs.Fiat)
                                    )
                                    || (this.url.Contains(CatalogApi.Catalogs.GeneralMotors)
                                            || this.url.Contains(CatalogApi.Catalogs.FiatProfessional)
                                    )
                                )
                                || (
                                    (this.url.Contains(CatalogApi.Catalogs.Lancia)
                                        || this.url.Contains(CatalogApi.Catalogs.Abarth)
                                    )
                                    || (
                                        this.url.Contains(CatalogApi.Catalogs.Fiat)
                                            || this.url.Contains(CatalogApi.Catalogs.Opel_main)
                                    )
                                )
                            )
                            || (
                                (
                                    (this.url.Contains(CatalogApi.Catalogs.Volvo)
                                        || this.url.Contains(CatalogApi.Catalogs.Rover)
                                    )
                                    || (this.url.Contains(CatalogApi.Catalogs.Chrysler)
                                        || this.url.Contains(CatalogApi.Catalogs.Chevrolet)
                                    )
                                )
                                || (this.url.Contains(CatalogApi.Catalogs.IvecoTruck)
                                    || this.url.Contains(CatalogApi.Catalogs.IvecoBus)
                                )
                            )
                        )
                    || (this.url.Contains(CatalogApi.Catalogs.Blanket) == true)
                )
                {
                    flag = true;
                    this.DelayForNextNavigation(this.IeHost, 1000, 2000);
                }
                #endregion

                #region cargobull
                if (this.url.Contains(CatalogApi.UrlConstants.CargoBullRoot) == true)
                {
                    flag = true;
                    elementById = this.IeWeb.Document.GetElementById("ctl00_cphLogin_tbUsername");
                    if (elementById != null)
                    {
                        elementById.SetAttribute("value", this.login);
                    }
                    element3 = this.IeWeb.Document.GetElementById("ctl00_cphLogin_tbPassword");
                    if (element3 != null)
                    {
                        element3.SetAttribute("value", this.password);
                    }
                    HtmlElement element10 = this.IeWeb.Document.GetElementById("ctl00_cphLogin_btnLogin");
                    if (element10 == null)
                    {
                        if (!(this.IeWeb.Document.Body.InnerHtml.Contains("EPOSArticleSearch") || this.cargobullPageLoading))
                        {
                            this.cargobullPageLoading = true;
                            this.IeWeb.Navigate(CatalogApi.UrlConstants.CargoBullArticleSearch);
                        }
                        else
                        {
                            HtmlElement element11 = this.IeWeb.Document.GetElementById("ctl00_cxNavigation_ulMenu");
                            if (element11 == null)
                            {
                                return;
                            }
                            foreach (HtmlElement element5 in element11.GetElementsByTagName("a"))
                            {
                                if (!string.IsNullOrEmpty(element5.GetAttribute("target")))
                                {
                                    element5.SetAttribute("target", "_self");
                                    element5.InvokeMember("click");
                                    this.DelayForNextNavigation(this.IeHost, 2000, 3000);
                                }
                            }
                        }
                    }
                    else
                    {
                        element10.InvokeMember("click");
                    }
                }
                #endregion

                #region Citroen
                if (this.url.Contains(CatalogApi.UrlConstants.CitroenRoot)) {
                    flag = true;
                    this.DelayForNextNavigation(this.IeHost, 1000, 2000);
                } else
                    ;
                #endregion

                #region Peugeot
                #endregion

                if (!flag)
                {
                    this.DelayForNextNavigation(this.IeHost, 500, 3000);
                }
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("IeWebOnDocumentCompleted", exception.Message + " | " + exception.StackTrace);
                Debug.WriteLine("[{0}] {1} / {2}", new object[] { DateTime.Now, exception.Message, exception.StackTrace });
            }
        }

        private void IeWebOnNavigating(object sender, WebBrowserNavigatingEventArgs webBrowserNavigatingEventArgs)
        {
            try
            {
#if DEBUG
                MainWindow.Logging(string.Format("Browser::IeWebOnNavigating (Title={0}, AbsoluteUri={1}) - ..."
                    , this.Title
                    , webBrowserNavigatingEventArgs.Url.AbsoluteUri));
#endif
                #region ChevroletOpel Group
                if ((webBrowserNavigatingEventArgs.Url.AbsoluteUri.Contains("spongepc.xw.gm.com/CStoneEPC"))
                    && (webBrowserNavigatingEventArgs.Url.AbsoluteUri.Contains("/logout?silent"))) {
                    if (IsDocumentValidate == true)
                        Close();
                    else
                        ;
                } else
                    ;
                #endregion
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("IeWebOnNavigating", exception.Message + " | " + exception.StackTrace);
                System.Windows.MessageBox.Show(string.Format("[{0}] {1} / {2}", DateTime.Now, exception.Message, exception.StackTrace));
            }
        }

        private void IeWebOnQuit(object sender, EventArgs eventArgs)
        {
            if (this.url.Contains(CatalogApi.UrlConstants.MazdaRoot) == true) {
                base.Close();
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
                return (!(this.IeWeb.Document == null))
                    && (!(this.IeWeb.Document.Body == null))
                    && (!(this.IeWeb.Document.Window == null))
                    && (!(this.IeWeb.Document.Window.Frames == null));
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

        private void IeWebOnStartNewWindow(object sender, BrowserExtendedNavigatingEventArgs browserExtendedNavigatingEventArgs)
        {
            if ((browserExtendedNavigatingEventArgs.Url == null)
                || (browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("about:blank") == false))
            {
                if ((browserExtendedNavigatingEventArgs.Url != null)
                    && browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains(CatalogApi.UrlConstants.MazdaRoot))
                {
                    browserExtendedNavigatingEventArgs.Cancel = true;
                }
                else if (ValidateUrlToNavigating(browserExtendedNavigatingEventArgs.Url) == false)
                {
                    browserExtendedNavigatingEventArgs.Cancel = true;
                }
                else
                {
                    if ((browserExtendedNavigatingEventArgs.Url != null)
                        && browserExtendedNavigatingEventArgs.Url.LocalPath.Contains(CatalogApi.Catalogs.Blanket) == true)
                    {
                        base.Close();
                    }

                    if ((browserExtendedNavigatingEventArgs.Url != null)
                        && browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains(CatalogApi.UrlConstants.BMW_WebETKStart))
                    {
                        this.IeWeb.Navigate(CatalogApi.UrlConstants.BMW_WebETKStart);
                        browserExtendedNavigatingEventArgs.Cancel = true;
                    }
                    else if (this.url.Contains(CatalogApi.UrlConstants.BmwGroupRoot))
                        browserExtendedNavigatingEventArgs.Cancel = true;
                    else
                    {
                        ExtendedWebBrowser browser = this.WindowManager.New(false);
                        browserExtendedNavigatingEventArgs.AutomationObject = browser.Application;
                    }
                }
            }
        }

        private void InsertCookies(string cookieDomain, IEnumerable<System.Net.Cookie> cookies)
        {
            foreach (System.Net.Cookie cookie in cookies)
            {
                CookieManager.Add(cookieDomain, cookie.Path, cookie.Name, cookie.Value, false, true, false,
                    (long)DateTimeToUnixTime(DateTime.Now.AddDays(10F)));
            }
        }

        //[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        //public static extern void InternetClearCookie();

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        private bool IsValidCookies(List<System.Net.Cookie> cookies)
        {
            return ((cookies != null) && (cookies.Count > 0));
        }

        private bool PartsLink24NeedNavigation(Uri uri)
        {
            return uri.AbsoluteUri.Contains("brandMenu.do");
        }

        private bool PartsLink24NeedRefresh(Uri uri)
        {
            return uri.AbsoluteUri.Contains("login.do");
        }

        private void OnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            try
            {
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("OnClosing", exception.Message + " | " + exception.StackTrace);
                Debug.WriteLine(exception.Message + " " + exception.StackTrace);
            }
        }

        protected override void OnContentRendered(EventArgs e)
        {
            try
            {
                bool flag = false;
                this.IeHost.Visibility = this.GeckoHost.Visibility = Visibility.Collapsed;

                #region Ford
                if (this.url.Contains(CatalogApi.UrlConstants.FordRoot) == true)
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region Mazda
                if (this.url.Contains(CatalogApi.UrlConstants.MazdaRoot) == true)
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region Peugeot
                if (this.url.Contains(CatalogApi.UrlConstants.PeugeotRoot) == true)
                {
                    flag = true;
                    this.OpenSession(false/*, string.Format(".{0}.com", CatalogApi.UrlConstants.PeugeotRoot)*/);
                    this.GeckoWeb.Navigate(this.url);
                }
                #endregion

                #region Citroen
                if (this.url.Contains(CatalogApi.UrlConstants.CitroenRoot) ==  true)
                {
                    flag = true;
                    this.OpenSession(false/*, string.Format(".{0}.com", CatalogApi.UrlConstants.CitroenRoot)*/);
                    this.GeckoWeb.Navigate(this.url);
                }
                #endregion

                #region MOBIS (Kia, Hundai)
                if ((this.url.Contains(CatalogApi.UrlConstants.Kia_Http_root) == true)
                    || (this.url.Contains(CatalogApi.UrlConstants.HyundaiRoot) == true))
                {
                    flag = true;
                    this.GeckoWeb.Navigate(this.url);
                }
                #endregion

                #region EWA-net
                if (this.url.Contains(CatalogApi.UrlConstants.MercedezRoot) == true)
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region Partslink24
                if (this.url.Contains(CatalogApi.UrlConstants.Partslink24Root) == true)
                {
                    flag = true;
                    InteropHelper.SetSystemTime(ref this.time);
                    if (RequestHelper.Client.IsServiceAvailable(CatalogApi.UrlConstants.Partslink24Com))
                    {
                        this.OpenSession(false/*, string.Format(".{0}.com", CatalogApi.UrlConstants.Partslink24Root)*/);
                        this.GeckoWeb.Navigate(string.Format("{0}/", CatalogApi.UrlConstants.Partslink24Com));
                    }
                    else
                    {
                        this.Captcha.Text = "Технические работы";
                        this.Message.Text = "У нас ведутся технические работы но уже скоро мы снова будем с вами.";
                    }
                }
                #endregion

                #region Chevrolet/Opel Group
                if (this.url.Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot) == true)
                {
                    flag = true;

                    this.OpenSession(false);
                    this.IeWeb.Navigate(string.Format("{0}/users/login.html", url)); // /users/login.html
                }
                #endregion

                #region AlfaRomeo
                if (this.url.Contains(CatalogApi.Catalogs.AlfaRomeo))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region OpelMain
                if (this.url.Contains(CatalogApi.Catalogs.Opel_main))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region IvecoTruck
                if (this.url.Contains(CatalogApi.Catalogs.IvecoTruck)
                    || this.url.Contains(CatalogApi.Catalogs.IvecoBus))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region Volvo
                if (this.url.Contains(CatalogApi.Catalogs.Volvo))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region Volvo Impact
                if (this.url.Contains(CatalogApi.Catalogs.VolvoImpact))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region Reno Impact
                if (this.url.Contains(CatalogApi.Catalogs.RenoImpact))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region Rover
                if (this.url.Contains(CatalogApi.Catalogs.Rover))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region Chrysler
                if (this.url.Contains(CatalogApi.Catalogs.Chrysler))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region Chevrolet
                if (this.url.Contains(CatalogApi.Catalogs.Chevrolet))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion


                if (this.url.Contains(CatalogApi.Catalogs.Blanket) == true) {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                } else
                    ;

                #region BMW
                if (this.url.Contains(CatalogApi.UrlConstants.BmwGroupRoot))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region CargoBull
                if (this.url.Contains(CatalogApi.UrlConstants.CargoBullRoot) == true)
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                #endregion

                #region Gigant-Group
                if (this.url.Contains(string.Format("{0}.com", CatalogApi.UrlConstants.GigantGroupRoot)) == true)
                {
                    flag = true;
                    this.GeckoWeb.Navigate(this.url);
                }
                #endregion

                #region Saf-axles
                if (this.url.Contains(CatalogApi.UrlConstants.SafAxlesRoot) == true) {
                    flag = true;
                    this.GeckoWeb.Navigate(this.url);
                } else
                    ;
                #endregion

                #region Ranger
                if (this.url.Contains(CatalogApi.UrlConstants.RangerRoot) == true) {
                    flag = true;
                    this.GeckoWeb.Navigate(this.url);
                } else
                    ;
                #endregion

                #region SsangYong
                if (this.url.Contains(CatalogApi.UrlConstants.SsangYongRoot) == true) {
                    flag = true;
                    this.GeckoWeb.Navigate(this.url);
                } else
                    ;
                #endregion

                if (!flag)
                {
                    this.IeWeb.Navigate(this.url);
                }
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("OnContentRendered", exception.Message + " | " + exception.StackTrace);
                this.Captcha.Text = "Ошибка подключения!";
                this.Message.Text = "Сервер не доступен, попробуйте подключиться позже...";
                this.IeHost.Visibility = Visibility.Collapsed;
                this.GeckoHost.Visibility = Visibility.Collapsed;
            }
            base.OnContentRendered(e);
        }

        private void OnKeyDown(object sender, System.Windows.Input.KeyEventArgs keyEventArgs)
        {
            try
            {
                if (this.url.Contains(CatalogApi.UrlConstants.Partslink24Root))
                {
                    keyEventArgs.Handled = true;
                }
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("OnKeyDown", exception.Message + " | " + exception.StackTrace);
                System.Windows.MessageBox.Show(string.Format("[{0}] {1} / {2}", DateTime.Now, exception.Message, exception.StackTrace));
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            try
            {
                this.IeWeb.ScriptErrorsSuppressed = true;
                this.IeWeb.Quit += new EventHandler(this.IeWebOnQuit);
                this.IeWeb.Navigating += new WebBrowserNavigatingEventHandler(this.IeWebOnNavigating);
                this.IeWeb.NewWindow += delegate (object o, CancelEventArgs args)
                {
                };
                this.IeWeb.StartNewWindow += new EventHandler<BrowserExtendedNavigatingEventArgs>(this.IeWebOnStartNewWindow);
                this.IeWeb.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.IeWebOnDocumentCompleted);
                this.IeWeb.IsWebBrowserContextMenuEnabled = false;
                if (this.url.Contains(CatalogApi.Catalogs.RenoImpact))
                {
                    this.IeWeb.IsWebBrowserContextMenuEnabled = true;
                }
                this.GeckoWeb.DomClick += new EventHandler<DomMouseEventArgs>(this.GeckoWebOnDomClick);
                this.GeckoWeb.Navigating += new EventHandler<GeckoNavigatingEventArgs>(this.GeckoWebOnNavigating);
                this.GeckoWeb.WindowClosed += new EventHandler(this.GeckoWebOnWindowClosed);
                this.GeckoWeb.CreateWindow2 += new EventHandler<GeckoCreateWindow2EventArgs>(this.GeckoWebOnCreateWindow2);
                this.GeckoWeb.ShowContextMenu += new EventHandler<GeckoContextMenuEventArgs>(this.GeckoWebOnShowContextMenu);
                this.GeckoWeb.FrameNavigating += new EventHandler<GeckoNavigatingEventArgs>(this.GeckoWebOnFrameNavigating);
                this.GeckoWeb.DOMContentLoaded += new EventHandler<DomEventArgs>(this.GeckoWebOnDomContentLoaded);
                this.GeckoWeb.DocumentCompleted += new EventHandler<GeckoDocumentCompletedEventArgs>(this.GeckoWebOnDocumentCompleted);
                this.GeckoWeb.DomClick += new EventHandler<DomMouseEventArgs>(this.GeckoWeb_DomClick);
                this.url.Contains(CatalogApi.UrlConstants.Partslink24Root);
                if (0 != 0)
                {
                    GeckoPreferences.User["network.proxy.type"] = 1;
                    GeckoPreferences.User["network.proxy.http"] = "http://40d002f8ae14.sn.mynetname.net";
                    GeckoPreferences.User["network.proxy.http_port"] = 0x1446;
                    System.Windows.MessageBox.Show("Включаем прокси сервер http://40d002f8ae14.sn.mynetname.net:5190");
                }
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("OnLoaded", exception.Message + " | " + exception.StackTrace);
                System.Windows.MessageBox.Show(string.Format("[{0}] {1} / {2}", DateTime.Now, exception.Message, exception.StackTrace));
            }
        }

        private void OpenSession(bool force = false, string host_cookies = "")
        {
            string session_cookies = string.Empty;

            RequestHelper.Client.OpenSession(this.url, force);

            session_cookies = RequestHelper.Client.GetCookies(this.url);
            List<System.Net.Cookie> cookies = JsonConvert.DeserializeObject<List<System.Net.Cookie>>(session_cookies);
            if ((cookies != null)
                && (cookies.Count > 0))
            {
                this.ClearCookies();

                if (this.url.Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot) == true) {
                //??? только для Chevrolet-Opel Group
                    host_cookies = this.url;

                    //InternetClearCookie();

                    foreach (System.Net.Cookie cookie in cookies) {
                        bool success = InternetSetCookie(
                                host_cookies
                                , cookie.Name
                                , cookie.Value);

                        MainWindow.Logging(string.Format(@"::OpenSession () - InternetSetCookie ({3} - host={0}, cookie-name={1}, cookie-value={2}) - ..."
                            , host_cookies, cookie.Name, cookie.Value
                            , success == true ? "Ok" : "ERROR"));
                    }
                } else {
                    if (host_cookies.Equals(string.Empty) == true)
                        host_cookies =
                            string.Format("{0}://{1}/", new Uri(this.url).Scheme, new Uri(this.url).Host)
                            ;
                    else
                        ;

                    this.InsertCookies(host_cookies, cookies);

                    foreach (System.Net.Cookie cookie in cookies)
                        MainWindow.Logging(string.Format(@"::OpenSession () - InsertCookies (host={0}, cookie-name={1}, cookie-value={2}) - ..."
                            , host_cookies, cookie.Name, cookie.Value));
                }
            } else
                ErrorLogHelper.AddErrorInLog(
                    string.Format("::OpenSession (host={0}, host_cookies={1})", this.url, host_cookies)
                    , string.Format("cookies: {0}", cookies == null ? "null" : "count = 0")
                );
        }

        private bool PartsLink24PageHasDemoString(string outerHtml)
        {
            return outerHtml.Contains("pl24demo");
        }

        private bool PartsLink24PageHasHeaderLogoString(string outerHtml)
        {
            return outerHtml.Contains("headerLogo");
        }

        private bool RefreshSession(string cookieHost)
        {
            this.OpenSession(true, cookieHost);

            return SetCookiesToPath(cookieHost);
        }

        private bool SetCookiesToPath(string cookieHost)
        {
            List<System.Net.Cookie> cookies = this.GetCookies(this.url);
            if (!this.IsValidCookies(cookies)) {
                return false;
            } else
                ;

            this.ClearCookies();
            this.InsertCookies(cookieHost, cookies);

            return true;
        }
    }
}

