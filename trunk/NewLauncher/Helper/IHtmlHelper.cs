namespace NewLauncher.Helper
{
    using System;

    internal interface IHtmlHelper
    {
        void ChangeTarget();
        void FilterContent();
        void SignIn(string userId, string passId, string buttonId);
    }
}

