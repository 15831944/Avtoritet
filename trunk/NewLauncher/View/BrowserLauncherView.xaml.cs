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


        public BrowserLauncherView(string url, string brand, SystemTime startTime, string Login, string Password)
        {
            try
            {
                this.InitializeComponent();
                this.time = startTime;
                base.Title = brand;
                this.url = url;
                this.login = Login;
                this.password = Password;
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
                if (geckoCreateWindow2EventArgs.Uri.Contains("partslink24"))
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
                if (geckoCreateWindow2EventArgs.Uri.Contains("peugeot") || geckoCreateWindow2EventArgs.Uri.Contains("citroen"))
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
                if (geckoCreateWindow2EventArgs.Uri.Contains("mazdaeur"))
                {
                    geckoCreateWindow2EventArgs.Cancel = true;
                }
                if (geckoCreateWindow2EventArgs.Uri.Contains("payment") || geckoCreateWindow2EventArgs.Uri.Contains("VendorLogin"))
                {
                    geckoCreateWindow2EventArgs.Cancel = true;
                }
                if (geckoCreateWindow2EventArgs.Uri.Contains("ssangyong"))
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
                this.Captcha.Text = "Загрузка...";
                this.Message.Text = "Пожалуйста, ждите...";

                #region wpc.mobis.co.kr
                if (this.url.Contains("wpc.mobis.co.kr") || this.url.Contains("wpc.mobis.co.kr")) {
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
                    if (this.NeedRefresh(geckoEventArgs.Uri))
                    {
                        if (this.RefreshSession(string.Format(".{0}", CatalogApi.UrlConstants.Partslink24Com), geckoEventArgs.Uri.AbsoluteUri))
                        {
                            this.GeckoWeb.Navigate(this.url);
                        }
                        else
                        {
                            this.Captcha.Text = "Ошибка подключения!";
                            this.Message.Text = "Сервер не доступен, попробуйте подключиться позже...";
                            this.GeckoHost.Visibility = Visibility.Collapsed;
                            return;
                        }
                    }
                    if (this.NeedNavigation(geckoEventArgs.Uri))
                    {
                        this.GeckoWeb.Navigate(this.url);
                    }
                    if (this.PageHasDemoString(this.GeckoWeb.Document.Body.OuterHtml))
                    {
                        this.Captcha.Text = "Тайм-аут сессии";
                        this.Message.Text = "Перезапустите web-браузер";
                        this.GeckoHost.Visibility = Visibility.Collapsed;
                    }
                    else if (this.PageHasHeaderLogoString(this.GeckoWeb.Document.Body.OuterHtml))
                    {
                        GeckoElement element4 = this.GeckoWeb.Document.GetElementById("headerLogo");
                        if (element4 != null)
                        {
                            element4.SetAttribute("style", "display:none");
                        }
                        GeckoElement element5 = this.GeckoWeb.Document.GetElementById("headerLinks");
                        if (element5 != null)
                        {
                            element5.SetAttribute("style", "display:none");
                        }
                        GeckoElement element6 = this.GeckoWeb.Document.GetElementById("headerText");
                        if (element6 != null)
                        {
                            element6.SetAttribute("style", "display:none");
                        }
                        GeckoElement element7 = this.GeckoWeb.Document.GetElementById("portalMenu");
                        if (element7 != null)
                        {
                            element7.SetAttribute("style", "display:none");
                        }
                        GeckoElement element8 = this.GeckoWeb.Document.GetElementById("main-content");
                        if (element8 != null)
                        {
                            element8.SetAttribute("style", "display:none");
                        }
                        GeckoElement element9 = this.GeckoWeb.Document.GetElementById("otherInfoBox");
                        if (element9 != null)
                        {
                            element9.SetAttribute("style", "display:none");
                        }
                    }
                    else
                    {
                        if (geckoEventArgs.Window.Frames.Count > 0)
                        {
                            foreach (GeckoWindow window in geckoEventArgs.Window.Frames)
                            {
                                if (this.PageHasDemoString(window.Document.ActiveElement.OuterHtml))
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
                        this.DelayForNextNavigation(this.GeckoHost, 0x7d0, 0xbb8);
                    }
                }
                #endregion

                if (this.url.Contains("saf-axles"))
                {
                    this.DelayForNextNavigation(this.GeckoHost, 300, 500);
                }
                if (this.url.Contains("inforanger.roadranger"))
                {
                    FilterContent(this.GeckoWeb.Document, "td", "nav_top");
                    this.DelayForNextNavigation(this.GeckoHost, 300, 500);
                }
                if (this.url.Contains("gigant-group.com"))
                {
                    this.DelayForNextNavigation(this.GeckoHost, 300, 500);
                }

                #region ssangyong
                if (this.url.Contains("ssangyong"))
                {
                    GeckoElement element11 = this.GeckoWeb.Document.GetElementById("login");
                    if (element11 != null)
                    {
                        element11.SetAttribute("value", this.login);
                    }
                    GeckoElement element12 = this.GeckoWeb.Document.GetElementById("pass");
                    if (element12 != null)
                    {
                        element12.SetAttribute("value", this.password);
                    }
                    element3 = this.GeckoWeb.Document.GetElementById("loginOrRegistration");
                    if (element3 != null)
                    {
                        new GeckoInputElement(element3.DomObject).Click();
                    }
                    else
                    {
                        FilterContent(this.GeckoWeb.Document, "div", "news");
                        FilterContent(this.GeckoWeb.Document, "div", "footer");
                        FilterContent(this.GeckoWeb.Document, "li", "link_bug");
                        FilterContent(this.GeckoWeb.Document, "li", "exit");
                        FilterContent(this.GeckoWeb.Document, "li", "empty");
                        if (this.GeckoWeb.Document.Body.OuterHtml.Contains("Купить"))
                        {
                            this.GeckoWeb.Document.Body.InnerHtml = this.GeckoWeb.Document.Body.InnerHtml.Replace("Купить", string.Empty);
                        }
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
            if (this.url.Contains("peugeot"))
            {
                if (this.GeckoWeb.Url.AbsoluteUri.Contains("docacc"))
                {
                    this.GeckoWeb.Navigate("http://public.servicebox.peugeot.com/docpr/");
                }
                if ((this.GeckoWeb.Document.Body.InnerHtml.IndexOf("Your session time expired. Please reconnect.") > 0) || this.GeckoWeb.Url.AbsoluteUri.Contains("index.jsp"))
                {
                    this.RefreshSession(".peugeot.com", this.url);
                    this.GeckoWeb.Navigate("http://public.servicebox.peugeot.com/docpr/");
                }
                else
                {
                    if (this.GeckoWeb.Document.Body.InnerHtml.IndexOf("A technical problem occurred. Please retry to connect later.") > 0)
                    {
                        this.DelayForNextNavigation(this.GeckoHost, 0x3e8, 0x7d0);
                        return;
                    }
                    element = this.GeckoWeb.Document.GetElementsByTagName("span").First<GeckoHtmlElement>(x => x.Id == "libelleflag");
                    if (!((element == null) || element.OuterHtml.Contains("русский")))
                    {
                        this.GeckoWeb.Navigate("javascript:validChoixLangue('ru_RU')");
                    }
                    else
                    {
                        elementArray = (from x in this.GeckoWeb.Document.GetElementsByTagName("ul")
                                        where x.Id.Contains("menu")
                                        select x).ToArray<GeckoHtmlElement>();
                        if (elementArray.Length > 0)
                        {
                            enumerable = elementArray[0].GetElementsByTagName("li").Skip<GeckoElement>(3);
                            foreach (GeckoElement element2 in enumerable)
                            {
                                element2.SetAttribute("style", "display:none");
                            }
                        }
                        FilterContent(this.GeckoWeb.Document, "a", "bandeau_panier");
                        foreach (GeckoHtmlElement element3 in from x in this.GeckoWeb.Document.GetElementsByTagName("div")
                                                              where (((x.Id.Contains("tools") || x.Id.Contains("aide")) || (x.Id.Contains("contact") || x.Id.Contains("param"))) || (x.Id.Contains("promos") || x.Id.Contains("contenu"))) || x.Id.Contains("ip")
                                                              select x)
                        {
                            element3.Style.SetPropertyValue("display", "none");
                        }
                        this.DelayForNextNavigation(this.GeckoHost, 0x3e8, 0x7d0);
                    }
                }
            }
            if (this.url.Contains("citroen"))
            {
                if (this.GeckoWeb.Url.AbsoluteUri.Contains("docacc"))
                {
                    this.GeckoWeb.Navigate("http://service.citroen.com/docpr/");
                }
                if ((this.GeckoWeb.Document.Body.InnerHtml.IndexOf("Your session time expired. Please reconnect.") > 0) || this.GeckoWeb.Url.AbsoluteUri.Contains("index.jsp"))
                {
                    this.RefreshSession(".citroen.com", this.url);
                    this.GeckoWeb.Navigate("http://service.citroen.com/docpr/");
                }
                else if (this.GeckoWeb.Document.Body.InnerHtml.IndexOf("A technical problem occurred. Please retry to connect later.") > 0)
                {
                    this.DelayForNextNavigation(this.GeckoHost, 0x3e8, 0x7d0);
                }
                else
                {
                    element = this.GeckoWeb.Document.GetElementsByTagName("span").First<GeckoHtmlElement>(x => x.Id == "libelleflag");
                    if (!((element == null) || element.OuterHtml.Contains("русский")))
                    {
                        this.GeckoWeb.Navigate("javascript:validChoixLangue('ru_RU')");
                    }
                    else
                    {
                        elementArray = (from x in this.GeckoWeb.Document.GetElementsByTagName("ul")
                                        where x.Id.Contains("menu")
                                        select x).ToArray<GeckoHtmlElement>();
                        if (elementArray.Length > 0)
                        {
                            enumerable = elementArray[0].GetElementsByTagName("li").Skip<GeckoElement>(3);
                            foreach (GeckoElement element2 in enumerable)
                            {
                                element2.SetAttribute("style", "display:none");
                            }
                        }
                        FilterContent(this.GeckoWeb.Document, "a", "bandeau_panier");
                        foreach (GeckoHtmlElement element3 in from x in this.GeckoWeb.Document.GetElementsByTagName("div")
                                                              where (((x.Id.Contains("tools") || x.Id.Contains("aide")) || (x.Id.Contains("contact") || x.Id.Contains("param"))) || (x.Id.Contains("promos") || x.Id.Contains("contenu"))) || x.Id.Contains("ip")
                                                              select x)
                        {
                            element3.Style.SetPropertyValue("display", "none");
                        }
                        this.DelayForNextNavigation(this.GeckoHost, 0x3e8, 0x7d0);
                    }
                }
            }
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
            if (this.url.Contains("vin-online") && geckoNavigatingEventArgs.Uri.AbsoluteUri.Contains("vin-online.ru/shop"))
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
            return JsonConvert.DeserializeObject<List<System.Net.Cookie>>(RequestHelper.Client.GetCookies(geckoUrl));
        }

        private void IeWebOnDocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs webBrowserDocumentCompletedEventArgs)
        {
            try
            {
                logging(string.Format("Browser::IeWebOnDocumentCompleted (Title={0}, AbsoluteUri={1}) - ..."
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
                    this.DelayForNextNavigation(this.IeHost, 0x7d0, 0xbb8);
                }
                if (this.url.Contains(CatalogApi.Catalogs.RenoImpact))
                {
                    flag = true;
                    this.DelayForNextNavigation(this.IeHost, 500, 0xbb8);
                }

                #region bmwgroup
                if (this.url.Contains("bmwgroup"))
                {
                    flag = true;
                    string absoluteUri = this.IeWeb.Document.Url.AbsoluteUri;
                    if (absoluteUri.Contains("PSESSID"))
                    {
                        UrlBmwSession = absoluteUri;
                    }
                    if (!absoluteUri.Contains("PSESSID") & (this.url == "https://www.parts.bmwgroup.com/tetis/startTetisAction.do?DOMAIN=Internet"))
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
                            this.IeWeb.Navigate("https://www.parts.bmwgroup.com/tetis/startNode.do?APP=WebETK&ENTRY_ID=WebETK_START&NODE=ROOT:Favorite:WebETK:WebETK_START");
                            this.flag1 = true;
                        }
                    }
                }
                #endregion

                #region Ford
                if (this.url.Contains("Ford"))
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
                            if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("IsoViewSplashLoad"))
                            {
                            }
                            if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("VehicleSelectorList"))
                            {
                                if (!this.viewChanged && (this.IeWeb.Document != null))
                                {
                                    IEnumerable<HtmlElement> enumerable4 = this.IeWeb.Document.GetElementsByTagName("a").Cast<HtmlElement>();
                                    foreach (HtmlElement element5 in from tag in enumerable4
                                                                     where (tag != null) && (tag.Id == "VisualAccessId")
                                                                     select tag)
                                    {
                                        element5.InvokeMember("click");
                                        return;
                                    }
                                }
                                this.viewChanged = true;
                            }
                            if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("LoginSubmit"))
                            {
                                if (!this.languageChanged)
                                {
                                    Uri uri = new Uri(this.url);
                                    this.IeWeb.Navigate(uri.Scheme + "://" + uri.Authority + "/Ford/SessionData?language=ru&reload=true");
                                }
                                this.languageChanged = true;
                                this.DelayForNextNavigation(this.IeHost, 0x1388, 0x1f40);
                            }
                        }
                    }
                }
                #endregion

                #region mazdaeur
                if (this.url.Contains("mazdaeur"))
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
                        this.DelayForNextNavigation(this.IeHost, 0xbb8, 0xfa0);
                    }
                    if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("servlet/BackHome"))
                    {
                        this.DelayForNextNavigation(this.IeHost, 0x3e8, 0x7d0);
                    }
                    if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("servlet/LoginEPC"))
                    {
                        this.DelayForNextNavigation(this.IeHost, 0x3e8, 0x7d0);
                    }
                    if (webBrowserDocumentCompletedEventArgs.Url.AbsoluteUri.Contains("servlet/DataMaintenance"))
                    {
                        this.DelayForNextNavigation(this.IeHost, 0x3e8, 0x7d0);
                    }
                }
                #endregion

                #region EWA-net
                if (this.url.Contains("EWA-net"))
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
                                        this.DelayForNextNavigation(this.IeHost, 0x3e8, 0x7d0);
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
                            //"https://gme-infotech.com/users/login.html"
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
                    //            this.DelayForNextNavigation(this.IeHost, 0x3e8, 0x7d0);

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
                                this.DelayForNextNavigation(this.IeHost, 0x3e8, 0x7d0);

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
                            this.DelayForNextNavigation(this.IeHost, 0x3e8, 0x7d0);

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

                #region BMW
                if (this.url.Contains("bmwgroup"))
                {
                    flag = true;
                    this.DelayForNextNavigation(this.IeHost, 0x3e8, 0x7d0);
                }
                else
                    ;
                #endregion

                if (
                        (
                            (
                                (
                                    (this.url.Contains(CatalogApi.Catalogs.AlfaRomeo)
                                        || this.url.Contains(CatalogApi.Catalogs.Fiat)
                                    )
                                    || (this.url.Contains("http://172.16.24.38:351/PQMace/")
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
                    || this.url.Contains("10.0.0.10:351")
                )
                {
                    flag = true;
                    this.DelayForNextNavigation(this.IeHost, 0x3e8, 0x7d0);
                }

                #region cargobull
                if (this.url.Contains("cargobull"))
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
                            this.IeWeb.Navigate("https://www.cargobull-serviceportal.de/Applications/ServicePortal/ArticleSearch.aspx");
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
                                    this.DelayForNextNavigation(this.IeHost, 0x7d0, 0xbb8);
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

                if (!flag)
                {
                    this.DelayForNextNavigation(this.IeHost, 500, 0xbb8);
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
                logging(string.Format("Browser::IeWebOnNavigating (Title={0}, AbsoluteUri={1}) - ..."
                    , this.Title
                    , webBrowserNavigatingEventArgs.Url.AbsoluteUri));
#endif

                if ((webBrowserNavigatingEventArgs.Url.AbsoluteUri.Contains("spongepc.xw.gm.com/CStoneEPC"))
                    && (webBrowserNavigatingEventArgs.Url.AbsoluteUri.Contains("/logout?silent"))) {
                    if (IsDocumentValidate == true)
                        Close();
                    else
                        ;
                } else
                    ;
            }
            catch (Exception exception)
            {
                ErrorLogHelper.AddErrorInLog("IeWebOnNavigating", exception.Message + " | " + exception.StackTrace);
                System.Windows.MessageBox.Show(string.Format("[{0}] {1} / {2}", DateTime.Now, exception.Message, exception.StackTrace));
            }
        }

        private void IeWebOnQuit(object sender, EventArgs eventArgs)
        {
            if (this.url.Contains("mazdaeur"))
            {
                base.Close();
            }
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
                    && browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains("mazdaeur"))
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
                        && browserExtendedNavigatingEventArgs.Url.LocalPath.Contains("http://10.0.0.10:351/PQMace/login.fve"))
                    {
                        base.Close();
                    }
                    if ((browserExtendedNavigatingEventArgs.Url != null)
                        && browserExtendedNavigatingEventArgs.Url.AbsoluteUri.Contains(CatalogApi.UrlConstants.BMW_ETKEntry))
                    {
                        this.IeWeb.Navigate(CatalogApi.UrlConstants.BMW_ETKEntry);
                        browserExtendedNavigatingEventArgs.Cancel = true;
                    }
                    else if (this.url.Contains(CatalogApi.UrlConstants.BMW_Root))
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
                    (long)DateTimeToUnixTime(DateTime.Now.AddDays(10.0)));
            }
        }

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);
        private bool IsValidCookies(List<System.Net.Cookie> cookies)
        {
            return ((cookies != null) && (cookies.Count > 0));
        }

        private bool NeedNavigation(Uri uri)
        {
            return uri.AbsoluteUri.Contains("brandMenu.do");
        }

        private bool NeedRefresh(Uri uri)
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
                if (this.url.Contains("wpc.mobis.co.kr"))
                {
                    flag = true;
                    this.GeckoWeb.Navigate(this.url);
                }
                if (this.url.Contains("Ford"))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains("mazdaeur"))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains("peugeot"))
                {
                    flag = true;
                    this.OpenSession(".peugeot.com", false);
                    this.GeckoWeb.Navigate(this.url);
                }
                if (this.url.Contains("citroen"))
                {
                    flag = true;
                    this.OpenSession(".citroen.com", false);
                    this.GeckoWeb.Navigate(this.url);
                }
                if (this.url.Contains("wpc.mobis.co.kr"))
                {
                    flag = true;
                    this.GeckoWeb.Navigate(this.url);
                }
                if (this.url.Contains("EWA-net"))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains(CatalogApi.UrlConstants.Partslink24Root))
                {
                    flag = true;
                    InteropHelper.SetSystemTime(ref this.time);
                    if (RequestHelper.Client.IsServiceAvailable())
                    {
                        this.OpenSession(string.Format(".{0}.com", CatalogApi.UrlConstants.Partslink24Root), false);
                        this.GeckoWeb.Navigate(string.Format("{0}/", CatalogApi.UrlConstants.Partslink24Com));
                    }
                    else
                    {
                        this.Captcha.Text = "Технические работы";
                        this.Message.Text = "У нас ведутся технические работы но уже скоро мы снова будем с вами.";
                    }
                }

                #region Chevrolet/Opel Group
                if (this.url.Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot) == true)
                {
                    flag = true;

                    this.OpenSession(url, false);
                    this.IeWeb.Navigate(string.Format("{0}/users/login.html", url)); // /users/login.html
                }
                #endregion

                if (this.url.Contains(CatalogApi.Catalogs.AlfaRomeo))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains(CatalogApi.Catalogs.Opel_main))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains(CatalogApi.Catalogs.IvecoTruck)
                    || this.url.Contains(CatalogApi.Catalogs.IvecoBus))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains(CatalogApi.Catalogs.Volvo))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains(CatalogApi.Catalogs.VolvoImpact))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains(CatalogApi.Catalogs.RenoImpact))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains(CatalogApi.Catalogs.Rover))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains(CatalogApi.Catalogs.Chrysler))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains(CatalogApi.Catalogs.Chevrolet))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains("10.0.0.10:351"))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains(CatalogApi.UrlConstants.BMW_Root))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains("cargobull"))
                {
                    flag = true;
                    this.IeWeb.Navigate(this.url);
                }
                if (this.url.Contains("gigant-group.com"))
                {
                    flag = true;
                    this.GeckoWeb.Navigate(this.url);
                }
                if (this.url.Contains("saf-axles"))
                {
                    flag = true;
                    this.GeckoWeb.Navigate(this.url);
                }
                if (this.url.Contains("inforanger.roadranger"))
                {
                    flag = true;
                    this.GeckoWeb.Navigate(this.url);
                }
                if (this.url.Contains("ssangyong"))
                {
                    flag = true;
                    this.GeckoWeb.Navigate(this.url);
                }
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
                if (this.url.Contains("partslink24"))
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
                this.url.Contains("partslink24");
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

        private void OpenSession(string host, bool force = false)
        {
            string session_cookies = string.Empty;

            RequestHelper.Client.OpenSession(this.url, force);
            session_cookies = RequestHelper.Client.GetCookies(this.url);
            List<System.Net.Cookie> cookies = JsonConvert.DeserializeObject<List<System.Net.Cookie>>(session_cookies);
            if ((cookies != null) && (cookies.Count > 0))
            {
                if (!host.Contains(CatalogApi.UrlConstants.ChevroletOpelGroupRoot)) // "gme-infotech.com"
                {
                    this.ClearCookies();
                    this.InsertCookies(host, cookies);
                }
                else
                //??? только для Chevrolet-Opel Group
                    foreach (System.Net.Cookie cookie in cookies)
                        if (InternetSetCookie(host, cookie.Name, cookie.Value) == false)
                            logging(string.Format(@"::OpenSession () - InternetSetCookie (host={0}, cookie-name={1}, cookie-value={2}) - ...", host, cookie.Name, cookie.Value));
                        else
                            ;
            }
        }

        private bool PageHasDemoString(string outerHtml)
        {
            return outerHtml.Contains("pl24demo");
        }

        private bool PageHasHeaderLogoString(string outerHtml)
        {
            return outerHtml.Contains("headerLogo");
        }

        private bool RefreshSession(string cookieHost, string geckoUrl)
        {
            RequestHelper.Client.OpenSession(this.url, true);
            List<System.Net.Cookie> cookies = this.GetCookies(this.url);
            if (!this.IsValidCookies(cookies))
            {
                return false;
            }
            this.ClearCookies();
            this.InsertCookies(cookieHost, cookies);
            this.GeckoWeb.Navigate(geckoUrl);
            this.GeckoHost.Visibility = Visibility.Visible;
            return true;
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

