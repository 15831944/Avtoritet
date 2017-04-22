namespace NewLauncher.Helper
{
    using Gecko;
    using Gecko.DOM;
    using NewLauncher.Extension;
    using System;

    public class HtmlHelper : IHtmlHelper
    {
        private readonly ExtendedWebBrowser extendedWeb;
        private readonly GeckoWebBrowser geckoWeb;
        private string password;
        private string userName;

        public HtmlHelper(GeckoWebBrowser geckoWeb, string userName, string password)
        {
            this.geckoWeb = geckoWeb;
            this.userName = userName;
            this.password = password;
        }

        public HtmlHelper(ExtendedWebBrowser extendedWeb, string userName, string password)
        {
            this.userName = userName;
            this.password = password;
            this.extendedWeb = extendedWeb;
        }

        public void ChangeTarget()
        {
            throw new NotImplementedException();
        }

        public void FilterContent()
        {
            throw new NotImplementedException();
        }

        public void SignIn(string userId, string passId, string buttonId)
        {
            GeckoElement elementById = this.geckoWeb.Document.GetElementById("Userid");
            if (elementById != null)
            {
                elementById.SetAttribute("value", "AVTORITET");
            }
            GeckoElement element2 = this.geckoWeb.Document.GetElementById("Passwd");
            if (element2 != null)
            {
                element2.SetAttribute("value", "Hugoboss2030");
            }
            GeckoElement element3 = this.geckoWeb.Document.GetElementById("LoginButton");
            if (element3 != null)
            {
                new GeckoInputElement(element3.DomObject).Click();
            }
        }
    }
}

